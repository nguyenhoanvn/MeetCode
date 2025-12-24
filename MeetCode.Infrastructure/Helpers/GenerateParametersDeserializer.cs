using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Infrastructure.Helpers
{
    public static class GenerateParametersDeserializer
    {
        public static string GenerateDeserializer(string type, int lineIndex)
        {
            // Remove array brackets and handle generic types
            var baseType = type.Replace("[]", "").Replace("?", "").Trim();
            var isArray = type.Contains("[]");
            var isNullable = type.Contains("?");

            if (isArray)
            {
                return GenerateArrayDeserializer(baseType, lineIndex);
            }

            // Handle common types
            return baseType.ToLower() switch
            {
                "int" or "int32" => $"int.Parse(lines[{lineIndex}])",
                "long" or "int64" => $"long.Parse(lines[{lineIndex}])",
                "double" => $"double.Parse(lines[{lineIndex}])",
                "float" => $"float.Parse(lines[{lineIndex}])",
                "bool" or "boolean" => $"bool.Parse(lines[{lineIndex}])",
                "string" => $"lines[{lineIndex}]",
                "char" => $"char.Parse(lines[{lineIndex}])",
                _ => $"System.Text.Json.JsonSerializer.Deserialize<{type}>(lines[{lineIndex}])"
            };
        }

        private static string GenerateArrayDeserializer(string elementType, int lineIndex)
        {
            var deserializer = elementType.ToLower() switch
            {
                "int" or "int32" => "int.Parse",
                "long" or "int64" => "long.Parse",
                "double" => "double.Parse",
                "float" => "float.Parse",
                "bool" or "boolean" => "bool.Parse",
                "string" => "s => s",
                "char" => "char.Parse",
                _ => $"s => System.Text.Json.JsonSerializer.Deserialize<{elementType}>(s)"
            };

            return $@"lines[{lineIndex}].Trim('[', ']').Split(',', System.StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).Select({deserializer}).ToArray()";
        }
    }
}
