using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace MeetCode.Application.Commands.CommandValidators.Language
{
    public interface IDockerValidator
    {
        Task<bool> ImageExistsAsync(string image, CancellationToken ct);
    }
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
            } else
            {
                return await CheckGenericRegistryAsync(registry, repo, tag, ct);
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
            } catch (Exception ex)
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
            } catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Failed to check registry {registry} {repo}:{tag}");
                return false;
            }
        }

        private (string registry, string repo, string tag) ParseImage(string image)
        {
            // openjdk:17 -> docker.io/library/openjdk, 17

            var parts = image.Split(new[] {'/'}, 2);
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
