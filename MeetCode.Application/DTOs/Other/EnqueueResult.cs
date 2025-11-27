using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.DTOs.Other
{
    public sealed record EnqueueResult(object MessageSent, string QueueName);

}
