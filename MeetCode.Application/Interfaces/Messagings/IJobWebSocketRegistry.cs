using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Interfaces.Messagings
{
    public interface IJobWebSocketRegistry
    {
        Task RegisterAsync(Guid jobId, IWebSocketConnection socket);
        Task SendToJobAsync(Guid jobId, object payload);
        Task RemoveAsync(Guid jobId);
    }
}
