using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using MeetCode.Application.Queries.QueryEntities;
using MeetCode.Application.Queries.QueryResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MeetCode.Server.ControllersAdmin
{
    [Route("admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ISender _mediator;

        public AdminController(
            ISender mediator
            )
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "admin,moderator")]
        [HttpGet("status")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(CurrentUserQueryResult), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.Error)]

        public async Task<Result<CurrentUserQueryResult>> GetCurrentUser(CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Result.Unauthorized();
            }

            var cmd = new CurrentUserQuery(Guid.Parse(userId));

            var result = await _mediator.Send(cmd, ct);

            return result;
        }
    }
}
