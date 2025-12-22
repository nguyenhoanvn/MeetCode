using MeetCode.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace MeetCode.Domain.Entities;

public partial class Language
{
    public Guid LangId { get; set; }
    
    public string Name { get; set; } = null!;

    public string Version { get; set; } = null!;
    public string FileExtension { get; set; } = null!;
    public string CompileImage { get; set; } = default!;

    public string RuntimeImage { get; set; } = null!;

    public string? CompileCommand { get; set; }

    public string? RunCommand { get; set; }

    public bool IsEnabled { get; set; }

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();

    public virtual ICollection<ProblemTemplate> ProblemTemplates { get; set; } = new List<ProblemTemplate>();
    public override string ToString()
    {
        return this.ToGenericString();
    }

    public void ToggleStatus()
    {
        IsEnabled = !IsEnabled;
    }

    public void UpdateBasicInfo(LanguageBasicUpdateObject info)
    {
        if (info.Name is not null)
        {
            Name = info.Name;
        }
        if (info.Version is not null)
        {
            Version = info.Version;
        }
        if (info.FileExtension is not null)
        {
            FileExtension = info.FileExtension;
        }
        if (info.CompileImage is not null)
        {
            CompileImage = info.CompileImage;
        }
        if (info.RuntimeImage is not null)
        {
            RuntimeImage = info.RuntimeImage;
        }
        if (info.CompileCommand is not null)
        {
            CompileCommand = info.CompileCommand;
        }
        if (info.RunCommand is not null)
        {
            RunCommand = info.RunCommand;
        }
    }
}
