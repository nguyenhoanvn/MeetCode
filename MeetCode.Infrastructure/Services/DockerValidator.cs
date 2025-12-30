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
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

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

        public async Task<TestResult?> RunCodeAsync(string code, Language language, ProblemTemplate problemTemplate, TestCase testCase, CancellationToken ct)
        {
            code = problemTemplate.RunnerCode.Replace("// USER_CODE", code);

            var tempFolder = await InitializeWorkingFolderAsync(code, testCase, language, ct);

            var containerName = $"runner_{Guid.NewGuid():N}";        

            try
            {
                await CreateRunningContainerAsync(containerName, tempFolder, language, ct);

                var codeFile = Path.Combine(tempFolder, $"Program.{language.FileExtension}");
                File.WriteAllText(codeFile, code);

                await CompileFileAsync(containerName, language, ct);
                if (language.FileExtension == "cs")
                {
                    File.WriteAllText(codeFile, code);
                }

                var sw = new Stopwatch();
                sw.Start();

                await RunFileAsync(containerName, language, ct);

                sw.Stop();

                var outputFile = Path.Combine(tempFolder, "output.json");
                if (!File.Exists(outputFile))
                {
                    throw new FileNotFoundException("No test result found in the container");
                }

                var json = await File.ReadAllTextAsync(outputFile, ct);
                var containerResult = JsonSerializer.Deserialize<object>(json);

                var testResult = new TestResult
                (
                    TestCase: testCase,
                    Result: containerResult.ToString(),
                    IsSuccessful: testCase.ExpectedOutputText == containerResult.ToString() ? true : false,
                    ExecTimeMs: sw.ElapsedMilliseconds
                );

                return testResult;
            } catch (Exception ex)
            {
                var testResult = new TestResult
                    (
                        TestCase: testCase,
                        Result: ex.Message,
                        IsSuccessful: false,
                        ExecTimeMs: 0
                    );
                return testResult;
            }
            finally
            {
                var processCleanup = new ProcessStartInfo
                {
                    FileName = "docker",
                    Arguments = $"rm -f {containerName}"
                };
                Process.Start(processCleanup)?.WaitForExit();
                Directory.Delete(tempFolder, recursive: true);
            }
        }

        private async Task<string> InitializeWorkingFolderAsync(string code, TestCase testCase, Language language, CancellationToken ct)
        {
            var tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempFolder);

            var codeFile = Path.Combine(tempFolder, $"Program.{language.FileExtension}");
            await File.WriteAllTextAsync(codeFile, code, ct);

            var inputFile = Path.Combine(tempFolder, "testCase.txt");
            var testCaseLine = testCase.InputText.Split(',');
            using (var writer = File.AppendText(inputFile))
            {
                foreach (var line in testCaseLine)
                {
                    writer.WriteLine(line.Substring(line.IndexOf('=') + 1).Trim());
                }
            }

            //Return the tempFolder destination
            return tempFolder;
        }

        private async Task CreateRunningContainerAsync(string containerName, string tempFolder, Language language, CancellationToken ct)
        {
            var process = new ProcessStartInfo
            {
                FileName = "docker",
                Arguments = $"run -d --name {containerName} " +
                $"--memory=256m --network=none " +
                $"-v \"{tempFolder}:/app\" -w /app {language.CompileImage} " +
                $"sleep infinity",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            await Process.Start(process)!.WaitForExitAsync(ct);
        }

        private async Task CompileFileAsync(string containerName, Language language, CancellationToken ct)
        {
            var process = new ProcessStartInfo
            {
                FileName = "docker",
                Arguments = $"exec {containerName} /bin/bash -c \"{language.CompileCommand}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            await Process.Start(process)!.WaitForExitAsync(ct);
        }

        private async Task RunFileAsync(string containerName, Language language, CancellationToken ct)
        {
            var processThirdStart = new ProcessStartInfo
            {
                FileName = "docker",
                Arguments = $"exec {containerName} /bin/bash -c \"{language.RunCommand} > /app/output.json\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            await Process.Start(processThirdStart)!.WaitForExitAsync(ct);
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
