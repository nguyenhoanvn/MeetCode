namespace ReactASP.Server.DTOs.ProblemAdd
{
    public sealed class ProblemAddResponse
    {
        public Guid ProblemId { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Difficulty { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; set; } = default!;
    }
}
