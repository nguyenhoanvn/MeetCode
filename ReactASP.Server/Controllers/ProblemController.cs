using Ardalis.Result;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ReactASP.Application.Commands.CommandEntities.Problem;
using ReactASP.Application.Commands.CommandResults.Problem;
using ReactASP.Application.Queries.QueryEntities.Problem;
using ReactASP.Server.DTOs.Request.Problem;
using ReactASP.Server.DTOs.Response.Problem;

namespace ReactASP.Server.Controllers
{
    [Route("problems")]
    [ApiController]
    public class ProblemController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<ProblemController> _logger;

        public ProblemController(
            ISender mediator,
            IMapper mapper,
            ILogger<ProblemController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }
        [Authorize(Roles = "moderator")]
        [HttpPost("create")]
        [ProducesResponseType(typeof(ProblemAddResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddProblem([FromBody] ProblemAddRequest request, CancellationToken ct)
        {
            
            if (request == null)
            {
                _logger.LogWarning($"Add failed for problem because request is null");
                return BadRequest("Invalid request body");
            }
            _logger.LogInformation($"Add new problem attempt for the title {request.Title}");

            var cmd = _mapper.Map<ProblemAddCommand>(request);

            var result = await _mediator.Send(cmd, ct);

            _logger.LogInformation($"Problem add success {request.Title}");

            var resp = _mapper.Map<ProblemAddResponse>(result.Value);

            return CreatedAtAction(nameof(AddProblem), new { Id = resp.ProblemId }, resp);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ProblemAllResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ProblemList(CancellationToken ct)
        {
            _logger.LogInformation("Attempting to read all problems");

            var cmd = new ProblemAllQuery();
            var result = await _mediator.Send(cmd, ct);

            _logger.LogInformation("Read all problems completed");
            var resp = _mapper.Map<ProblemAllResponse>(result.Value);
            return Ok(resp);
        }

        [HttpGet("{slug}")]
        [ProducesResponseType(typeof(ProblemReadResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ProblemRead([FromRoute]string slug, CancellationToken ct)
        {
            _logger.LogInformation($"Attempting to read the problem with slug: {slug}");

            var cmd = new ProblemReadQuery(slug);
            var result = await _mediator.Send(cmd, ct);

            _logger.LogInformation($"Read problem with slug {slug} completed");
            var resp = _mapper.Map<ProblemReadResponse>(result.Value, opt =>
            {
                opt.Items["Title"] = result.Value.Problem.Title;
                opt.Items["StatementMd"] = result.Value.Problem.StatementMd;
                opt.Items["Difficulty"] = result.Value.Problem.Difficulty;
                opt.Items["TotalSubmissionCount"] = result.Value.Problem.TotalSubmissionCount;
                opt.Items["ScoreAcceptedCount"] = result.Value.Problem.ScoreAcceptedCount;
                opt.Items["AcceptanceRate"] = result.Value.Problem.AcceptanceRate;
            });
            return Ok(resp);
        }
    }
}
