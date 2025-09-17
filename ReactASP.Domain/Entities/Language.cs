using System;
using System.Collections.Generic;

namespace ReactASP.Domain.Entities;

public partial class Language
{
    public Guid LangId { get; set; }

    public string Name { get; set; } = null!;

    public string Version { get; set; } = null!;

    public string RuntimeImage { get; set; } = null!;

    public string? CompileCommand { get; set; }

    public string? RunCommand { get; set; }

    public bool IsEnabled { get; set; }

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}
