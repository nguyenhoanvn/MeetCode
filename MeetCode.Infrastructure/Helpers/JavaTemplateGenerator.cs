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
            return $@"class Solution
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
            return $@"import java.nio.file.*;
import java.io.*;

public class Runner {{
    public static void main(String[] args) throws Exception {{
        String[] lines = Files.readAllLines(Paths.get(""testCase.txt"")).toArray(new String[0]);

        int a = lines[0];
        int b = lines[1];

        Solution s = new Solution();
        System.out.println(s.{methodCall});
    }}
}}
            ";
        }
    }
}
