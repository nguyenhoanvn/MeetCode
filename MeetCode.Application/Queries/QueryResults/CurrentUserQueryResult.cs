using MeetCode.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Queries.QueryResults
{
    public sealed record CurrentUserQueryResult(
        MeetCode.Domain.Entities.User User
        );
}