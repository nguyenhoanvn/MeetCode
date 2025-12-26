using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using MeetCode.Application.DTOs.Request.Auth;
using MeetCode.Application.Queries.QueryEntities.Auth;
using MeetCode.Application.Queries.QueryResults.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeetCode.Server.ControllersAdmin
{
    [Route("admin/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISender _mediator;
        public AuthController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(LoginAdminQueryResult), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.Error, ResultStatus.Invalid)]
        public async Task<Result<LoginAdminQueryResult>> Login([FromBody] LoginRequest request, CancellationToken ct)
        {
            var cmd = new LoginAdminQuery(
                request.Email,
                request.Password
            );

            var result = await _mediator.Send(cmd, ct);

            if (result.IsSuccess)
            {
                HttpContext.Response.Cookies.Append(
                    "accessToken",
                    result.Value.AccessToken,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTimeOffset.UtcNow.AddMinutes(30)
                    });
                HttpContext.Response.Cookies.Append(
                    "refreshToken",
                    result.Value.RefreshToken,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTimeOffset.UtcNow.AddDays(30)
                    });
            }

            return result;
        }
    }
}
