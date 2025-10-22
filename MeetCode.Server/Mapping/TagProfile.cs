using AutoMapper;
using MeetCode.Application.Commands.CommandEntities.Tag;
using MeetCode.Application.Commands.CommandResults.Tag;
using MeetCode.Application.Queries.QueryEntities.Tag;
using MeetCode.Application.Queries.QueryResults.Tag;
using MeetCode.Domain.Entities;
using MeetCode.Server.DTOs.Request.Tag;
using MeetCode.Server.DTOs.Response.Tag;

namespace MeetCode.Server.Mapping
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<ProblemTag, TagResponse>()
                .ForMember(dest => dest.TagId, opt => opt.MapFrom(src => src.TagId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
