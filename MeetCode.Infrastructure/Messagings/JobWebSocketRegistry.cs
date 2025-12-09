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
        private readonly ConcurrentDictionary<Guid, WebSocket> _connections = new();

        public async Task HandleConnectionAsync(WebSocket socket)
        {
            var buffer = new byte[4096];

            try
            {
                while (socket.State == WebSocketState.Open)
                {
                    // Just keep the socket alive. No need to parse messages.
                    var result = await socket.ReceiveAsync(buffer, CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                        break;
                }
            }
            finally
            {
                // Remove socket on disconnect
                foreach (var (jobId, ws) in _connections)
                {
                    if (ws == socket)
                    {
                        _connections.TryRemove(jobId, out _);
                        break;
                    }
                }

                if (socket.State != WebSocketState.Closed)
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
            }
        }

        public Task SubscribeAsync(Guid jobId, WebSocket socket)
        {
            _connections[jobId] = socket;
            return Task.CompletedTask;
        }

        public async Task SendToJobAsync(Guid jobId, object message)
        {
            if (_connections.TryGetValue(jobId, out var socket) &&
                socket.State == WebSocketState.Open)
            {
                var json = JsonSerializer.Serialize(message);
                var bytes = Encoding.UTF8.GetBytes(json);

                await socket.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
