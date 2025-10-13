using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactASP.Application.Commands.CommandEntities.Auth
{
    public sealed record ForgotPasswordRequestedEvent(string Email, string Code);
}
