using MeetCode.Application.Commands.CommandResults.Submit;
using MeetCode.Application.Interfaces.Messagings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MeetCode.Server.Messaging
{
    public class RunResultConsumer : BackgroundService
    {
        private readonly ILogger<RunResultConsumer> _logger;
        private readonly IJobWebSocketRegistry _wsRegistry;
        private readonly ConnectionFactory _factory;
        private IConnection? _connection;
        private IChannel? _channel;

        public RunResultConsumer(ILogger<RunResultConsumer> logger, IJobWebSocketRegistry wsRegistry)
        {
            _logger = logger;
            _wsRegistry = wsRegistry;
            _factory = new ConnectionFactory
            {
                HostName = "localhost", // or read from config
                Port = 5672,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _connection = await _factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync("run_result_queue", durable: true, exclusive: false, autoDelete: false);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);
                    var result = JsonSerializer.Deserialize<RunCodeCommandResult>(json);

                    if (result != null)
                    {
                        await _wsRegistry.SendToJobAsync(result.JobId, result);
                        _logger.LogInformation($"Sent result to WebSocket for JobId {result.JobId}");
                    }

                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to push result to WebSocket");
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
                }
            };

            await _channel.BasicConsumeAsync("run_result_queue", autoAck: false, consumer: consumer);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            base.Dispose();
        }
    }
}
