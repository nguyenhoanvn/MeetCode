using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandResults.Profile;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandEntities.Profile
{
    public sealed record AvatarUploadCommand(Guid UserId, IFormFile File) : IRequest<Result<AvatarUploadCommandResult>>;
}
