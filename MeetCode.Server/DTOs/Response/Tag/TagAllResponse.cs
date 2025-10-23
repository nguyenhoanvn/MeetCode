using MeetCode.Domain.Entities;

namespace MeetCode.Server.DTOs.Response.Tag
{
    public sealed record TagAllResponse(IEnumerable<TagResponse> TagList);
}
