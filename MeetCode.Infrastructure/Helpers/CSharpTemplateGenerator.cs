using MeetCode.Application.Interfaces.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Infrastructure.Helpers
{
    public class CSharpTemplateGenerator : ILanguageTemplateGenerator
    {
        public string LanguageName => "csharp";
        public string GenerateTemplate(string methodName, string returnType, string[] parameters)
        {
            var paramList = string.Join(", ", parameters);
            return $@"
public class Solution
{{
    public {returnType} {methodName}({paramList})
    {{
        
    }}
}}
            ";
        }
    }
}
