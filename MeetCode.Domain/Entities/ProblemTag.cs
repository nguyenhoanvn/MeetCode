using System;
using System.Collections.Generic;

namespace MeetCode.Domain.Entities;

public partial class ProblemTag
{
    public Guid TagId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Problem> Problems { get; set; } = new List<Problem>();
    public override string ToString()
    {
        return this.ToGenericString();
    }
}
