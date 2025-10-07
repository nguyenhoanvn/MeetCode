using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactASP.Application.Commands.ProblemAdd;
using ReactASP.Server.DTOs.ProblemAdd;

namespace ReactASP.Server.Controllers
{
    [Route("problems")]
    [ApiController]
    public class ProblemController : ControllerBase
    {
        private readonly ISender _mediator;

        public ProblemController(ISender mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "moderator")]
        [HttpPost("create")]
        [ProducesResponseType(typeof(ProblemAddResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddProblem([FromBody] ProblemAddRequest request, CancellationToken ct)
        {
            var cmd = new ProblemAddCommand(request.Title, request.StatementMd, request.Difficulty, request.TimeLimitMs, request.MemoryLimitMb);

            try
            {
                var result = await _mediator.Send(cmd, ct);
                var resp = new ProblemAddResponse
                {
                    ProblemId = result.Value.ProblemId,
                    Slug = result.Value.Slug,
                    Title = result.Value.Title,
                    Difficulty = result.Value.Difficulty,
                    CreatedAt = result.Value.CreatedAt
                };

                return CreatedAtAction(nameof(AddProblem), new { Id = resp.ProblemId }, resp);
            } catch (InvalidOperationException ex)
            {
                return ValidationProblem(new ValidationProblemDetails
                {
                    Title = "Problem add failed",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest            
                });
            }
        }
    }
}
