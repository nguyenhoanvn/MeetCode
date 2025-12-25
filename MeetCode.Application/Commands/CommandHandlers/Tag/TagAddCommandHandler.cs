using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Tag;
using MeetCode.Application.Commands.CommandResults.Tag;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MeetCode.Application.Commands.CommandHandlers.Tag
{
    public class TagAddCommandHandler : IRequestHandler<TagAddCommand, Result<TagAddCommandResult>>
    {
        private readonly ILogger<TagAddCommandHandler> _logger;
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;
        public TagAddCommandHandler(
            ILogger<TagAddCommandHandler> logger,
            ITagRepository tagRepository,
            IUnitOfWork unitOfWork
            )
        {
            _logger = logger;
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<TagAddCommandResult>> Handle(TagAddCommand request, CancellationToken ct)
        {
            _logger.LogInformation($"Attempting to add new tag with name {request.Name}");

            if ((await _tagRepository.GetByNameAsync(request.Name, ct) != null))
            {
                _logger.LogWarning("Tag with name {Name} existed", request.Name);
                return Result.Invalid(new ValidationError(nameof(request.Name), $"Tag with name {request.Name} existed."));
            }

            var tag = new MeetCode.Domain.Entities.ProblemTag
            {
                Name = request.Name
            };

            await _tagRepository.AddAsync(tag, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            
            _logger.LogInformation($"Tag {tag.Name} added successfully");

            return Result.Created(new TagAddCommandResult(tag));
        }
    }
}
