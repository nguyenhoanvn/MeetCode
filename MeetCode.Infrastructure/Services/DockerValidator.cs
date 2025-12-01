using MeetCode.Application.DTOs.Other;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MeetCode.Infrastructure.Services
{
    public class DockerValidator : IDockerValidator
    {
        private string AUTHURL = "https://auth.docker.io/token?service=registry.docker.io&scope=repository:{repo}:pull";
        private string MANIFESTURL = "https://registry-1.docker.io/v2/{repo}/manifests/{tag}";
        private string FULLREGISTRYURL = "https://{registry}/v2/{repo}/manifests/{tag}";
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<DockerValidator> _logger;
        public DockerValidator(
            IHttpClientFactory httpClient,
            ILogger<DockerValidator> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<bool> ImageExistsAsync(string image, CancellationToken ct)
        {
            var (registry, repo, tag) = ParseImage(image);
            if (registry == "docker.io")
            {
                return await CheckDockerHubAsync(repo, tag, ct);
            }
            else
            {
                return await CheckGenericRegistryAsync(registry, repo, tag, ct);
            }
        }

        public async Task<TestResult?> RunCodeAsync(string code, Language language, Problem problem, TestCase testCase, CancellationToken ct)
        {
            var tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempFolder);

            var codeFile = Path.Combine(tempFolder, language.Name);
            await File.WriteAllTextAsync(codeFile, code, ct);

            var inputFile = Path.Combine(tempFolder, "testCases.txt");
            await File.WriteAllTextAsync(inputFile, testCase.InputText, ct);

            var outputFile = Path.Combine(tempFolder, "output.json");

            var containerName = $"runner_{Guid.NewGuid():N}";

            try
            {
                var args = $"run --rm --name {containerName} -v {tempFolder}:/sandbox ${language.RuntimeImage} " +
                    $"/bin/sh -c \"{language.CompileCommand ?? ""} ?? {language.RunCommand}\"";

                var processStart = new ProcessStartInfo
                {
                    FileName = "docker",
                    Arguments = args,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(processStart);
                if (process == null)
                {
                    return null;
                }

                if (!process.WaitForExit(problem.TimeLimitMs))
                {
                    process.Kill();
                    return new TestResult(testCase, "", "Time Limit Exceeded", false, problem.TimeLimitMs);
                }

                string dockerStdout = await process.StandardOutput.ReadToEndAsync(ct);
                string dockerStderr = await process.StandardError.ReadToEndAsync(ct);

                _logger.LogInformation($"Docker stdout: {dockerStdout}");
                _logger.LogInformation($"Docker stderr: {dockerStderr}");

                if (!File.Exists(outputFile))
                {
                    throw new FileNotFoundException("No test result found in the container");
                }

                var json = await File.ReadAllTextAsync(outputFile, ct);
                var containerResult = JsonSerializer.Deserialize<TestResult>(json);

                return containerResult;
            } finally
            {
                Directory.Delete(tempFolder);
            }
        }

        private async Task<bool> CheckDockerHubAsync(string repo, string tag, CancellationToken ct)
        {
            var client = _httpClient.CreateClient();
            try
            {
                var authUrl = AUTHURL.Replace("{repo}", repo);
                var authResponse = await client.GetFromJsonAsync<DockerAuthResponse>(authUrl, ct);
                if (authResponse?.Token == null)
                {
                    return false;
                }

                client.DefaultRequestHeaders.Authorization = new("Bearer", authResponse.Token);
                var manifestUrl = MANIFESTURL.Replace("{repo}", repo).Replace("{tag}", tag);
                var response = await client.GetAsync(manifestUrl, ct);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to validate Docker image: {Image}", $"{repo}:{tag}");
                return false;
            }
        }

        private async Task<bool> CheckGenericRegistryAsync(string registry, string repo, string tag, CancellationToken ct)
        {
            var client = _httpClient.CreateClient();
            try
            {
                var fullManifestUrl = FULLREGISTRYURL.Replace("{registry}", registry).Replace("{repo}", repo).Replace("{tag}", tag);
                var response = await client.GetAsync(fullManifestUrl, ct);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Failed to check registry {registry} {repo}:{tag}");
                return false;
            }
        }

        private (string registry, string repo, string tag) ParseImage(string image)
        {
            // openjdk:17 -> docker.io/library/openjdk, 17

            var parts = image.Split(new[] { '/' }, 2);
            string registry = "docker.io";
            string repoAndTag = parts[0];

            if (parts.Length == 2 && parts[0].Contains('.'))
            {
                registry = parts[0];
                repoAndTag = parts[1];
            }

            var repoParts = repoAndTag.Split(new[] { ':' }, 2);
            var repo = repoParts[0];
            var tag = repoParts.Length > 1 ? repoParts[1] : "lastest";

            if (registry == "docker.io" && !repo.Contains('/'))
            {
                repo = $"library/{repo}";
            }
            return (registry, repo, tag);
        }

        public record DockerAuthResponse(string Token);
    }
}
