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

namespace MeetCode.Infrastructure.Messagings
{
    public class RabbitMqSender : IJobSender
    {
        private readonly ConnectionFactory _factory;
        private readonly ILogger<RabbitMqSender> _logger;
        private readonly IConfigurationSection _rabbitMqConfiguration;
        public RabbitMqSender(
            IConfiguration configuration,
            ILogger<RabbitMqSender> logger)
        {
            _logger = logger;
            _rabbitMqConfiguration = configuration.GetSection("RabbitMq");
            _factory = new ConnectionFactory
            {
                HostName = _rabbitMqConfiguration.GetValue<string>("HostName"),
                UserName = _rabbitMqConfiguration.GetValue<string>("UserName"),
                Password = _rabbitMqConfiguration.GetValue<string>("Password"),
                Port = _rabbitMqConfiguration.GetValue<int>("Port")
            };
        }

        /*
         * Enqueue an object T to the RabbitMQ
         * T must be declare when using
         * The queue is queueName parameter
         */
        public async Task EnqueueJobAsync<T>(T job, string queueName, CancellationToken ct)
        {
            using var cnn = await _factory.CreateConnectionAsync(ct);
            using var channel = await cnn.CreateChannelAsync();
            Console.WriteLine($"Connected to {cnn.Endpoint.HostName}");

            await channel.QueueDeclareAsync(queueName, true, false, false, null);

            await using var ms = new MemoryStream();
            await JsonSerializer.SerializeAsync(ms, job, cancellationToken: ct);
            var body = ms.ToArray();

            var exchangeKey = _rabbitMqConfiguration.GetSection("ExchangeKey").Get<string>();
            var routingKey = _rabbitMqConfiguration.GetSection("RoutingKey").Get<string>();

            
            await channel.ExchangeDeclareAsync(exchangeKey, ExchangeType.Direct, true);

            await channel.QueueBindAsync(queueName, exchangeKey, routingKey);

            await channel.BasicPublishAsync(exchangeKey, routingKey, body, ct);
        }
    }
}
