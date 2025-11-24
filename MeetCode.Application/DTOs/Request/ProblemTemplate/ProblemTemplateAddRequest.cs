using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.DTOs.Request.ProblemTemplate
{
    public class Variable {
        public string DataType { get; set; } = default!;
        public string Name { get; set; } = default!;

        public override string ToString()
        {
            return $"{DataType} {Name}";
        }
    }
    public sealed record ProblemTemplateAddRequest(string MethodName, string ReturnType, Variable[] Parameters, Guid LangId);
}
