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
            return $@"public class Solution
{{
    public {returnType} {methodName}({paramList})
    {{
        
    }}
}}
            ";
        }
        public string GenerateRunner(string methodName, string[] parameters)
        {
            var paramNames = parameters
                            .Select(p => p.Split(' ', StringSplitOptions.RemoveEmptyEntries).Last())
                            .ToArray();
            var methodCall = $"{methodName}({string.Join(", ", paramNames)})";
            return $@"namespace UserSubmission
{{
    {{USER_CODE}}
}}
public class Runner
{{
    public static void Main()
    {{
        string[] lines = File.ReadAllLines(""testCase.txt"");
        int a = int.Parse(lines[0]);
        int b = int.Parse(lines[1]);

        var s = new UserSubmission.Solution();
        Console.WriteLine(s.{methodCall});
    }}
}}
            ";
        }
    }
}
