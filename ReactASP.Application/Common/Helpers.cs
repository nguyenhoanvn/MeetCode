using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReactASP.Application.Common
{
    public static class Helpers
    {
        public static string GenerateSlug(string title)
        {
            return Regex.Replace(title.ToLowerInvariant().Trim(), @"\s+", "-");
        }
    }
}
