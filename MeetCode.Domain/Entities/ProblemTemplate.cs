using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Domain.Entities
{
    public partial class ProblemTemplate
    {
        public Guid TemplateId { get; set; }

        public Guid ProblemId { get; set; }
        public Guid LangId { get; set; }

        public string TemplateCode { get; set; } = null!;
        public string RunnerCode { get; set; } = null!;
        public string? CompileCommand { get; set; }
        public string? RunCommand { get; set; }
        public bool IsEnabled { get; set; } = true;

        public virtual Problem Problem { get; set; } = null!;
        public virtual Language Language { get; set; } = null!;

        public override string ToString()
        {
            return this.ToGenericString();
        }
        public void ToggleStatus()
        {
            IsEnabled = !IsEnabled;
        }
    }


}
