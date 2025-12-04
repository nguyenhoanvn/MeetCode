using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Interfaces.Helpers
{
    public interface ILanguageTemplateGenerator
    {
        string LanguageName { get; }
        string GenerateTemplate(string methodName, string returnType, string[] parameters);
    }
}
