namespace MeetCode.Server.DTOs.Response.Tag
{
    public sealed record TagAddResponse(
        Guid TagId,
        string Name,
        string Message
        );
}
