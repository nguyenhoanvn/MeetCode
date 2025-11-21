using MeetCode.Domain.Entities;

namespace MeetCode.Application.DTOs.Response.Tag
{
    public sealed record TagAllResponse(IEnumerable<TagResponse> TagList);
}
