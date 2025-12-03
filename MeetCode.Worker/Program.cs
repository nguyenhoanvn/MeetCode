using MeetCode.Application.Commands.CommandEntities.Submit;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Infrastructure.Persistence;
using MeetCode.Infrastructure.Persistence.Configurations;
using MeetCode.Infrastructure.Repositories;
using MeetCode.Infrastructure.Services;
using MeetCode.Worker.Consumers.Submit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<RunCodeConsumer>();
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DB")
));

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<RunCodeCommand>();
});

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
builder.Services.AddScoped<IProblemTemplateRepository, ProblemTemplateRepository>();

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
builder.Services.AddScoped<IProblemTemplateService, ProblemTemplateService>();
builder.Services.AddScoped<ISubmitService, SubmitService>();
builder.Services.AddScoped<IDockerValidator, DockerValidator>();

builder.Services.AddHttpClient();
var host = builder.Build();
await host.RunAsync();
