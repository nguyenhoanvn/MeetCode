using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using ReactASP.Application.Commands.CommandEntities.Problem;
using ReactASP.Application.Commands.CommandResults.Problem;
using ReactASP.Application.Interfaces.Services;

namespace ReactASP.Application.Commands.CommandHandlers.Problem
{
    public class ProblemUpdateHandler : IRequestHandler<ProblemUpdateCommand, Result<ProblemUpdateCommandResult>>
    {
        private readonly ILogger<ProblemUpdateHandler> _logger;
        private readonly IProblemService _problemService;
        public ProblemUpdateHandler(
            ILogger<ProblemUpdateHandler> logger,
            IProblemService problemService
            )
        {
            _logger = logger;
            _problemService = problemService;
        }
        public async Task<Result<ProblemUpdateCommandResult>> Handle(ProblemUpdateCommand request, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
