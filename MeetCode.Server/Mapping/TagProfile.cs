using AutoMapper;
using MeetCode.Application.Commands.CommandEntities.Tag;
using MeetCode.Application.Commands.CommandResults.Tag;
using MeetCode.Application.Queries.QueryEntities.Tag;
using MeetCode.Application.Queries.QueryResults.Tag;
using MeetCode.Server.DTOs.Request.Tag;
using MeetCode.Server.DTOs.Response.Tag;

namespace MeetCode.Server.Mapping
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            // Add tag
            CreateMap<TagAddRequest, TagAddCommand>();
            CreateMap<TagAddCommandResult, TagAddResponse>()
                .ConstructUsing(src => new TagAddResponse(
                    src.Tag.TagId,
                    src.Tag.Name,
                    "Tag created successfully"
                    ));

            // All tag
            CreateMap<TagAllRequest, TagAllQuery>();
            CreateMap<TagAllQueryResult, TagAllResponse>();

            // Read tag
            CreateMap<TagReadRequest, TagReadQuery>()
                .ForCtorParam("Name", opt => opt.MapFrom((src, context) =>
                    context.Items["Name"]));
            CreateMap<TagReadQueryResult, TagReadResponse>()
                .ForCtorParam("Name", opt => opt.MapFrom(src => src.Tag.Name));

            // Update tag
            CreateMap<TagUpdateRequest, TagUpdateCommand>()
                .ForCtorParam("TagId", opt => opt.MapFrom((src, context) =>
                    context.Items["TagId"]))
                .ForCtorParam("Name", opt => opt.MapFrom(src => src.NewTagName));
            CreateMap<TagUpdateCommandResult, TagUpdateResponse>()
                .ForCtorParam("Name", opt => opt.MapFrom(src => src.Tag.Name));

            // Delete tag
            CreateMap<TagDeleteRequest, TagDeleteCommand>()
                .ForCtorParam("TagId", opt => opt.MapFrom((src, context) =>
                    context.Items["TagId"]));
            CreateMap<TagDeleteCommandResult, TagDeleteResponse>();
        }
    }
}
