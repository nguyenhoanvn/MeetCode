using Ardalis.Result.AspNetCore;
using AutoMapper;
using MediatR;
using MeetCode.Application.DTOs.Response.Profile;
using Microsoft.AspNetCore.Http;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using MeetCode.Application.DTOs.Request.Profile;
using MeetCode.Application.Queries.QueryEntities.Profile;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MeetCode.Server.ControllersAdmin
{
    [Route("profile")]
    [ApiController]
    public class ProfileUserController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<ProfileUserController> _logger;

        public ProfileUserController(
            ISender mediator,
            IMapper mapper,
            ILogger<ProfileUserController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("whoami")]
        [Authorize]
        public IActionResult WhoAmI()
        {
            return Ok(new
            {
                Name = User.Identity.Name,
                hehe = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList(),
                IsModerator = User.IsInRole("moderator")
            });
        }

        [Authorize]
        [HttpGet("me")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(ProfileMinimalResponse), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.Error, ResultStatus.NotFound)]
        public async Task<Result<ProfileMinimalResponse>> MinimalProfile(CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Cannot find userId in cookies");
                return Result.Success();
            }

            var cmd = new ProfileUserQuery(Guid.Parse(userId));

            var result = await _mediator.Send(cmd, ct);

            var resp = result.Map(value => _mapper.Map<ProfileMinimalResponse>(value));

            return resp;
        }
    }
}
