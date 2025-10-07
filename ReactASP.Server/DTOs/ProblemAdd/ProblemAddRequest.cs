namespace ReactASP.Server.DTOs.ProblemAdd
{
    public sealed class ProblemAddRequest
    {
        public string Title { get; set; } = default!;
        public string StatementMd { get; set; } = default!;
        public string Difficulty { get; set; } = default!;
        public int TimeLimitMs { get; set; } = default!;
        public int MemoryLimitMb { get; set; } = default!;
    }
}
