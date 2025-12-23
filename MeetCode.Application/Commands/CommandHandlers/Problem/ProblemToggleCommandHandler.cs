using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Problem;
using MeetCode.Application.Commands.CommandResults.Problem;
using MeetCode.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandHandlers.Problem
{
    public class ProblemToggleCommandHandler : IRequestHandler<ProblemToggleCommand, Result<ProblemToggleCommandResult>>
    {
        private readonly IProblemRepository _problemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProblemToggleCommandHandler> _logger;
        public ProblemToggleCommandHandler(
            IProblemRepository problemRepository,
            IUnitOfWork unitOfWork,
            ILogger<ProblemToggleCommandHandler> logger
            )
        {
            _problemRepository = problemRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<ProblemToggleCommandResult>> Handle(ProblemToggleCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to toggle status of problem with Id {ProblemId}", request.ProblemId);

            var problem = await _problemRepository.GetByIdAsync(request.ProblemId, ct);
            if (problem == null)
            {
                _logger.LogWarning("Cannot find problem with Id {ProblemId}", request.ProblemId);
                return Result.Invalid(new ValidationError(nameof(request.ProblemId), $"Cannot find problem with Id {request.ProblemId}"));
            }

            problem.ToggleStatus();
            await _problemRepository.Update(problem);
            await _unitOfWork.SaveChangesAsync(ct);

            _logger.LogInformation("Problem {ProblemId} toggled to {Status}", request.ProblemId, problem.IsActive);

            return Result.Success(new ProblemToggleCommandResult(problem));
        }
    }
}
