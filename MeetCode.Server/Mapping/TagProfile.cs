using AutoMapper;
using MeetCode.Application.Commands.CommandEntities.Problem;
using MeetCode.Application.Commands.CommandEntities.Tag;
using MeetCode.Application.Commands.CommandResults.Problem;
using MeetCode.Application.Commands.CommandResults.Tag;
using MeetCode.Application.Queries.QueryEntities.Tag;
using MeetCode.Application.Queries.QueryResults.Tag;
using MeetCode.Domain.Entities;
using MeetCode.Application.DTOs.Request.Problem;
using MeetCode.Application.DTOs.Request.Tag;
using MeetCode.Application.DTOs.Response.Tag;

namespace MeetCode.Server.Mapping
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<ProblemTag, TagResponse>()
                .ConstructUsing(src => new TagResponse(
                    src.TagId,
                    src.Name));

            // Add
            CreateMap<TagAddRequest, TagAddCommand>();
            CreateMap<TagAddCommandResult, TagResponse>()
                .ConstructUsing((src, context) =>
                    context.Mapper.Map<TagResponse>(src.Tag));

            // Get all
            CreateMap<TagAllRequest, TagAllQuery>();
            CreateMap<TagAllQueryResult, TagAllResponse>()
                .ConstructUsing((src, context) => new TagAllResponse(
                    context.Mapper.Map<IEnumerable<TagResponse>>(src.TagList)
                    ));

            // Read
            CreateMap<TagReadQueryResult, TagResponse>()
                .ConstructUsing((src, context) =>
                    context.Mapper.Map<TagResponse>(src.Tag));

            // Update
            CreateMap<(Guid tagId, TagUpdateRequest request), TagUpdateCommand>()
                .ConstructUsing(src => new TagUpdateCommand(
                    src.tagId,
                    src.request.NewTagName
                    ));
            CreateMap<TagUpdateCommandResult, TagResponse>()
                .ConstructUsing((src, context) =>
                    context.Mapper.Map<TagResponse>(src.Tag));

            // Delete
            CreateMap<(Guid tagId, TagDeleteRequest request), TagDeleteCommand>()
                .ConstructUsing(src => new TagDeleteCommand(
                    src.tagId));
            CreateMap<TagDeleteCommandResult, TagMessageResponse>();
        }
    }
}
