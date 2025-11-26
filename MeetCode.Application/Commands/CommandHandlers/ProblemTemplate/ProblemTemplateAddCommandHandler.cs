using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.ProblemTemplate;
using MeetCode.Application.Commands.CommandResults.ProblemTemplate;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandHandlers.ProblemTemplate
{
    public class ProblemTemplateAddCommandHandler : IRequestHandler<ProblemTemplateAddCommand, Result<ProblemTemplateAddCommandResult>>
    {
        private readonly IProblemTemplateService _problemTemplateService;
        private readonly ILogger<ProblemTemplateAddCommandHandler> _logger;
        public ProblemTemplateAddCommandHandler(
            IProblemTemplateService problemTemplateService,
            ILogger<ProblemTemplateAddCommandHandler> logger)
        {
            _problemTemplateService = problemTemplateService;
            _logger = logger;
        }
        public async Task<Result<ProblemTemplateAddCommandResult>> Handle(ProblemTemplateAddCommand request, CancellationToken ct)
        {
            var problemTemplate = await _problemTemplateService.CreateTemplateAsync(
                request.MethodName,
                request.ReturnType,
                request.Parameters,
                request.ProblemId,
                request.LangId,
                ct);

            var result = new ProblemTemplateAddCommandResult(problemTemplate);

            return Result.Success(result);
        }
    }
}
