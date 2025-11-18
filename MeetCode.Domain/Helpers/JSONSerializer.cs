using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MeetCode.Domain.Helpers
{
    public static class JSONSerializer
    {
        public static string ConvertStringToJson(string inputText)
        {
            var dict = inputText
                .Split(',')
                .Select(p => p.Split('='))
                .ToDictionary(kvp => kvp[0].Trim(), kvp => kvp[1].Trim());

            var json = JsonSerializer.Serialize(dict);

            return json;
        }
    }
}
