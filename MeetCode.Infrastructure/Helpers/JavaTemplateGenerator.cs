using MeetCode.Application.Interfaces.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Infrastructure.Helpers
{
    public class JavaTemplateGenerator : ILanguageTemplateGenerator
    {
        public string LanguageName => "java";
        public string GenerateTemplate(string methodName, string returnType, string[] parameters)
        {
            var paramList = string.Join(", ", parameters);
            return $@"
class Solution
{{
    public {returnType} {methodName}({paramList})
    {{
        
    }}
}}
            ";
        }
    }
}
