using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetCode.Domain.Entities;

namespace MeetCode.Application.Queries.QueryResults.Auth
{
    public sealed record ForgotPasswordQueryResult(User CurrentUser, bool IsSuccess);
}
