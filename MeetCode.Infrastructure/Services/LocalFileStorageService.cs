using MeetCode.Application.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Infrastructure.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _basePath;
        private readonly IConfiguration _configuration;

        public LocalFileStorageService(IWebHostEnvironment env, IConfiguration configuration)
        {
            _basePath = Path.Combine(env.WebRootPath, "uploads");
            _configuration = configuration;
            Directory.CreateDirectory(_basePath);
        }

        public async Task<string> UploadAsync(IFormFile file, string folder, CancellationToken ct)
        {
            var folderPath = Path.Combine(_basePath, folder);
            Directory.CreateDirectory(folderPath);

            var fileName = $"avatar_{Guid.NewGuid}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream, ct);

            return Path.Combine(folder, fileName).Replace("\\", "/");
        }

        public Task<string> GetUrlAsync(string path, CancellationToken ct)
        {
            var baseUrl = _configuration["BaseUrl"] ?? "https://localhost:7254";
            return Task.FromResult($"{baseUrl}/uploads/{path}");
        }

        public Task DeleteAsync(string path, CancellationToken ct)
        {
            var filePath = Path.Combine(_basePath, path);
            if (File.Exists(filePath))
                File.Delete(filePath);
            return Task.CompletedTask;
        }
    }
}
