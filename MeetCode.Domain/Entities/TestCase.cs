using MeetCode.Domain.Helpers;
using System;
using System.Collections.Generic;

namespace MeetCode.Domain.Entities;

public partial class TestCase
{
    public Guid TestId { get; set; }

    public Guid ProblemId { get; set; }

    public string Visibility { get; set; } = null!;

    public string InputText { get; set; } = null!;
    public string InputJson => JSONSerializer.ConvertStringToJson(InputText);

    public string ExpectedOutputText { get; set; } = null!;
    public string ExpectedOutputJson => JSONSerializer.ConvertStringToJson(ExpectedOutputText);

    public int Weight { get; set; }

    public virtual Problem Problem { get; set; } = null!;
    public override string ToString()
    {
        return this.ToGenericString();
    }

    public void UpdateBasic(string visibility, string inputText, string expectedOutputText, int weight)
    {
        Visibility = visibility;
        InputText = inputText;
        ExpectedOutputText = expectedOutputText;
        Weight = weight;
    }
}
