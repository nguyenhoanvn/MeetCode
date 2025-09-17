using System;
using System.Collections.Generic;

namespace ReactASP.Domain.Entities;

public partial class TestCase
{
    public Guid TestId { get; set; }

    public Guid ProblemId { get; set; }

    public string Visibility { get; set; } = null!;

    public string InputText { get; set; } = null!;

    public string ExpectedOutputText { get; set; } = null!;

    public int Weight { get; set; }

    public virtual Problem Problem { get; set; } = null!;
}
