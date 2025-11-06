using AutoMapper;
using MeetCode.Application.Queries.QueryEntities.Profile;
using MeetCode.Application.Queries.QueryResults.Profile;
using MeetCode.Server.DTOs.Request.Profile;
using MeetCode.Server.DTOs.Response.Profile;

namespace MeetCode.Server.Mapping
{
    public class ProfileProfile : Profile
    {
        public ProfileProfile()
        {
            CreateMap<(Guid userId, ProfileUserRequest request), ProfileUserQuery>()
                .ConstructUsing(src => new ProfileUserQuery(
                    src.userId));
            CreateMap<ProfileUserQueryResult, ProfileMinimalResponse>();
        }
    }
}
