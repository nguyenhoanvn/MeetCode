using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactASP.Domain.Entities;

namespace ReactASP.Application.Queries.QueryResults.Auth
{
    public sealed record ForgotPasswordQueryResult(User CurrentUser, string Message);
}
