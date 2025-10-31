using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetCode.Domain.Entities;

namespace MeetCode.Application.Interfaces.Services
{
    public interface ILanguageService
    {
        Task<Language> UpdateLanguageAsync(string name, string version, string compileCommand, string runCommand, CancellationToken ct);
        Task<Language?> FindLanguageByNameAsync(string name, CancellationToken ct);

    }
}
