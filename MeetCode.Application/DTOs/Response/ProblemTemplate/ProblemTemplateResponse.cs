using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.DTOs.Response.ProblemTemplate
{
    public sealed record ProblemTemplateResponse(
        string LanguageName,
        string TemplateCode);
}
