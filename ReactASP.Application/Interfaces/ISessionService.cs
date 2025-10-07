using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ReactASP.Domain.Entities;

namespace ReactASP.Application.Interfaces
{
    public interface ISessionService
    {
        Claim? GetUserClaim(CancellationToken ct);
    }
}
