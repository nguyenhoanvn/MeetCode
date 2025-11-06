using Ardalis.Result.AspNetCore;
using AutoMapper;
using MediatR;
using MeetCode.Server.DTOs.Response.Profile;
using Microsoft.AspNetCore.Http;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using MeetCode.Server.DTOs.Request.Profile;
using MeetCode.Application.Queries.QueryEntities.Profile;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MeetCode.Server.Controllers
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
                return Result.Error("Cannot find userId in cookies");
            }

            var request = new ProfileUserRequest();
            var cmd = _mapper.Map<ProfileUserQuery>((Guid.Parse(userId), request));

            var result = await _mediator.Send(cmd, ct);

            var resp = result.Map(value => _mapper.Map<ProfileMinimalResponse>(value));

            return resp;
        }
    }
}
