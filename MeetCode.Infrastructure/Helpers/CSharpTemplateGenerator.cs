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
            var sb = new StringBuilder();
            var paramNames = new string[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var param = parameters[i].Trim();
                var parts = param.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var type = string.Join(" ", parts.Take(parts.Length - 1));
                var name = parts.Last();
                paramNames[i] = name;

                sb.AppendLine($"\t\t{type} {name} = {GenerateParametersDeserializer.GenerateDeserializer(type, i)}");
            }

            var methodCall = $"{methodName}({string.Join(", ", paramNames)})";

            return $@"namespace UserSubmission
{{
    // USER_CODE
}}

public class Runner
{{
    public static void Main()
    {{
        try
        {{
            string[] lines = System.IO.File.ReadAllLines(""testCase.txt"");
            
            if (lines.Length < {parameters.Length})
                throw new System.Exception($""Expected at least {parameters.Length} lines in testCase.txt, got {{lines.Length}}"");
            
{sb.ToString().TrimEnd()}
            
            var solution = new UserSubmission.Solution();
            var result = solution.{methodCall};

            Console.WriteLine(result);
        }}
        catch (System.Exception ex)
        {{
            Console.Error.WriteLine(""Error: "" + ex.Message);
            System.Environment.Exit(1);
        }}
    }}
}}";
        }
    }
}
