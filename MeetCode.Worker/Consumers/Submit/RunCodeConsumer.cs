using MediatR;
using MeetCode.Application.Commands.CommandEntities.Submit;
using MeetCode.Application.Commands.CommandResults.Submit;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MeetCode.Worker.Consumers.Submit
{
    public class RunCodeConsumer : BackgroundService
    {
        private readonly ILogger<RunCodeConsumer> _logger;
        private readonly IConnectionFactory _factory;
        private readonly IServiceProvider _serviceProvider;
        private IConnection? _connection;
        private IChannel? _channel;

        public RunCodeConsumer(
            IServiceProvider serviceProvider,
            ILogger<RunCodeConsumer> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _factory = new ConnectionFactory
            {
                HostName = configuration["RabbitMQ:HostName"] ?? "localhost",
                Port = configuration.GetValue<int>("RabbitMQ:Port", 5672),
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };
        }

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            try
            {
                _connection = await _factory.CreateConnectionAsync(ct);
                _channel = await _connection.CreateChannelAsync(cancellationToken: ct);

                await _channel.BasicQosAsync(0, 1, false, ct);

                await _channel.QueueDeclareAsync("run_code_queue", true, false, false, null);
                await _channel.QueueDeclareAsync("run_result_queue", true, false, false, null);

                var consumer = new AsyncEventingBasicConsumer(_channel);

                consumer.ReceivedAsync += async (_, ea) =>
                {
                    await ProcessMessageAsync(ea, ct);
                };

                await _channel.BasicConsumeAsync("run_code_queue", false, consumer, ct);

                _logger.LogInformation("RunCodeConsumer started and listening for messages");

                await Task.Delay(Timeout.Infinite, ct);
            } catch (OperationCanceledException)
            {
                _logger.LogInformation("RunCodeWorker is stopping");
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception in RunCodeConsumer");
            }
        }

        private async Task ProcessMessageAsync(BasicDeliverEventArgs ea, CancellationToken ct)
        {
            var payloadJson = "";

            try
            {
                payloadJson = Encoding.UTF8.GetString(ea.Body.ToArray());
                _logger.LogInformation($"Received message: {payloadJson}");

                var cmd = JsonSerializer.Deserialize<RunCodeCommand>(payloadJson);

                if (cmd == null)
                {
                    _logger.LogWarning("Failed to deserialize command");
                    await _channel!.BasicNackAsync(ea.DeliveryTag, false, false, ct);
                    return;
                }

                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var result = await mediator.Send(cmd, ct);

                await PublishResultAsync(result, ct);

                await _channel!.BasicAckAsync(ea.DeliveryTag, false, ct);

                _logger.LogInformation("Successfully processed command {CommandType}", cmd.GetType().Name);
            } catch (JsonException ex)
            {
                _logger.LogError(ex, $"JSON deserialization failed for payload: {payloadJson}");
                await _channel!.BasicNackAsync(ea.DeliveryTag, false, false);
            } catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex, "Internal error occurred when processing message");
                await _channel!.BasicNackAsync(ea.DeliveryTag, false, false, ct);
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message");
                await _channel!.BasicNackAsync(ea.DeliveryTag, false, true, ct);
            }
        }

        private async Task PublishResultAsync(RunCodeCommandResult result, CancellationToken ct)
        {
            var json = JsonSerializer.Serialize(result);
            var body = Encoding.UTF8.GetBytes(json);

            var props = new BasicProperties
            {
                Persistent = true
            };

            await _channel!.BasicPublishAsync(
                exchange: "",
                routingKey: "run_result_queue",
                mandatory: false,
                basicProperties: props,
                body: body,
                cancellationToken: ct
            );
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            base.Dispose();
        }
    }
}
