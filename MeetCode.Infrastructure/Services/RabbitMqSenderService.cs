using MeetCode.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MeetCode.Infrastructure.Services
{
    public class RabbitMqSenderService : IJobSenderService
    {
        private readonly ConnectionFactory _factory;
        private readonly ILogger<RabbitMqSenderService> _logger;
        private readonly IConfiguration _configuration;
        public RabbitMqSenderService(
            IServiceProvider serviceProvider,
            ILogger<RabbitMqSenderService> logger)
        {
            _logger = logger;
            _configuration = serviceProvider.GetRequiredService<IConfiguration>();
            _factory = new ConnectionFactory { HostName = _configuration.GetSection("RabbitMq:HostName").Get<string>() };
        }

        public async Task EnqueueJobAsync<T>(T job, string queueName, CancellationToken ct)
        {
            using var cnn = await _factory.CreateConnectionAsync(ct);
            using var channel = await cnn.CreateChannelAsync();

            await channel.QueueDeclareAsync(queueName, true, false, false);

            await using var ms = new MemoryStream();
            await JsonSerializer.SerializeAsync(ms, job, cancellationToken: ct);
            var body = ms.ToArray();

            var exchangeKey = _configuration.GetSection("RabbitMq:ExchangeKey").Get<string>();
            var routingKey = _configuration.GetSection("RabbitMq:RoutingKey").Get<string>();
            await channel.BasicPublishAsync(exchangeKey, routingKey, body, ct);
        }
    }
}
