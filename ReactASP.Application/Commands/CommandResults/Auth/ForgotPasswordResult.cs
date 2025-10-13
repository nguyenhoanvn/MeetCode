using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactASP.Domain.Entities;

namespace ReactASP.Application.Commands.CommandResults.Auth
{
    public sealed record ForgotPasswordResult(User CurrentUser, string Message);
}
