﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Domain.Entities;
using MeetCode.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace MeetCode.Infrastructure.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly ILogger<LanguageService> _logger;
        public LanguageService(
            ILanguageRepository languageRepository,
            ILogger<LanguageService> logger
            )
        {
            _languageRepository = languageRepository;
            _logger = logger;
        }

        public async Task<Language> UpdateLanguageAsync(string name, string version, string compileCommand, string runCommand, CancellationToken ct)
        {
            var language = await FindLanguageByNameAsync(name, ct);
            if (language == null)
            {
                _logger.LogWarning($"Language {name} cannot be found.");
                throw new EntityNotFoundException<Language>(nameof(name), name);
            }

            language.Version = version;
            language.CompileCommand = compileCommand;
            language.RunCommand = runCommand;

        }

        public async Task<Language?> FindLanguageByNameAsync(string name, CancellationToken ct)
        {
            return await _languageRepository.GetByNameAsync(name, ct);
        }

        private sealed record LanguageTemplate(
            string ImagePattern,
            string? DefaultCompile,
            string? DefaultRun);

        private Dictionary<string, LanguageTemplate> Rules = new(StringComparer.OrdinalIgnoreCase)
        {
            ["C#"] = new(
                "mcr.microsoft.com/dotnet/sdk:{version}",
                "csc /out:program.exe {file}.cs",
                "dotnet program.dll"
                ),
            ["Java"] = new(
                "openjdk:{version}-jdk-slim",
                "javac {file}.java",
                "java {file}"
                )
        };
    }
}
