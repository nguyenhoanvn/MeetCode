using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.DTOs.Request.ProblemTemplate
{
    public class Variable {
        public string DataType = default!;
        public string Name = default!;

        public override string ToString()
        {
            return $"{DataType} {Name}";
        }
    }
    public sealed record ProblemTemplateAddRequest(string MethodName, string ReturnType, Variable[] Parameters, Guid LangId);
}
