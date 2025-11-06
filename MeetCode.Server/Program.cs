using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MeetCode.Server.Mapping;
using System.IdentityModel.Tokens.Jwt;
using StackExchange.Redis;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Application.Commands.CommandEntities.Auth;
using MeetCode.Infrastructure.Services;
using MeetCode.Infrastructure.Repositories;
using MeetCode.Infrastructure.Persistence;
using MeetCode.Infrastructure.Persistence.Configurations;
using MeetCode.Server.Middlewares;
using MeetCode.Server.Helpers;
using System.Text.Json.Serialization;
using MeetCode.Application.Commands.CommandValidators.Language;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("https://localhost:4679")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

// EF Core (SQL Server)
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DB")
));

// Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var config = builder.Configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(config!);
});

// MediatR (scan app assembly)
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly);
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ResultBehaviorHandler<,>));
});

// AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(Program).Assembly));

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(RegisterUserCommand).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Logger
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProblemRepository, ProblemRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ITestCaseRepository, TestCaseRepository>();
builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();

// Services 
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IProblemService, ProblemService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICacheService, RedisService>();
builder.Services.AddScoped<IEmailService, GmailService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ITestCaseService, TestCaseService>();
builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<IProfileService, ProfileService>();

// Validators
builder.Services.AddScoped<IDockerValidator, DockerValidator>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<TrimStringsFilter>();
})
    .AddApplicationPart(typeof(Program).Assembly)
    .AddControllersAsServices()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid JWT token."
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Add JTW Validation
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = "nguyenhoanvn",
            ValidAudience = "megumi",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["accessToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }

                return Task.CompletedTask;
            }
        };
    });

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        CancellationTokenSource ctSource = new CancellationTokenSource();
        CancellationToken ct = ctSource.Token;
        await AppDbInit.SeedUsersAndRolesAsync(services, ct);
        await AppDbInit.SeedLanguagesAsync(services, ct);
    } catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError($"Failed to seed the database: {ex.Message}");
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseDefaultFiles();   // Looks for index.html
app.UseStaticFiles();    // Serves React static files

app.MapFallbackToFile("index.html");

app.UseCors("AllowReactApp");

// Middlewares
app.UseMiddleware<ValidationExceptionHandler>();
app.UseMiddleware<ExceptionHandler>();
app.UseMiddleware<RequestLoggingHandler>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();