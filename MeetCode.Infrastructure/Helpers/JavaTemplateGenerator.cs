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

            return $@"import java.io.*;
import java.util.*;
import com.google.gson.*;

class Solution {{
    // USER_CODE
}}

public class Runner {{
    public static void main(String[] args) {{
        try {{
            List<String> lines = new ArrayList<>();
            BufferedReader reader = new BufferedReader(new FileReader(""testCase.txt""));
            String line;
            while ((line = reader.readLine()) != null) {{
                lines.add(line);
            }}
            reader.close();
            
            if (lines.size() < {parameters.Length}) {{
                throw new Exception(""Expected at least {parameters.Length} lines in testCase.txt, got "" + lines.size());
            }}
            
{sb.ToString().TrimEnd()}
            
            Solution solution = new Solution();
            Object result = solution.{methodCall};
            
            // Handle different return types
            if (result instanceof int[]) {{
                System.out.println(Arrays.toString((int[]) result));
            }} else if (result instanceof long[]) {{
                System.out.println(Arrays.toString((long[]) result));
            }} else if (result instanceof double[]) {{
                System.out.println(Arrays.toString((double[]) result));
            }} else if (result instanceof boolean[]) {{
                System.out.println(Arrays.toString((boolean[]) result));
            }} else if (result instanceof String[]) {{
                System.out.println(Arrays.toString((String[]) result));
            }} else if (result instanceof Object[]) {{
                System.out.println(Arrays.deepToString((Object[]) result));
            }} else if (result instanceof List) {{
                System.out.println(result);
            }} else {{
                System.out.println(result);
            }}
        }} catch (Exception e) {{
            System.err.println(""Error: "" + e.getMessage());
            e.printStackTrace();
            System.exit(1);
        }}
    }}
}}";
        }
    }
}
