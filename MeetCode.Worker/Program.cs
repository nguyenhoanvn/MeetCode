using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<RunCodeConsumer>();

var host = builder.Build();
await host.RunAsync();
