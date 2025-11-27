using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Interfaces.Services
{
    public interface IJobSenderService
    {
        Task EnqueueJobAsync<T>(T job, string queueName, CancellationToken ct);
    }
}
