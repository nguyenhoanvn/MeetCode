using Fleck;
using MeetCode.Application.Interfaces.Messagings;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MeetCode.Infrastructure.Messagings
{
    public class JobWebSocketRegistry : IJobWebSocketRegistry
    {
        private readonly ConcurrentDictionary<Guid, IWebSocketConnection> _connections = new();

        public Task RegisterAsync(Guid jobId, IWebSocketConnection socket)
        {
            _connections[jobId] = socket;
            return Task.CompletedTask;
        }

        public Task SendToJobAsync(Guid jobId, object payload)
        {
            if (_connections.TryGetValue(jobId, out var ws))
            {
                ws.Send(JsonSerializer.Serialize(payload,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    }));
            }
            return Task.CompletedTask;
        }

        public Task RemoveAsync(Guid jobId)
        {
            _connections.TryRemove(jobId, out _);
            return Task.CompletedTask;
        }
    }
}
