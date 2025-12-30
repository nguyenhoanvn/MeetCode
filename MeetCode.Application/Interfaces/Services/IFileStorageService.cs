using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace MeetCode.Application.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task<string> UploadAsync(IFormFile file, string folder, CancellationToken ct);
        Task<string> GetUrlAsync(string path, CancellationToken ct);
        Task DeleteAsync(string path, CancellationToken ct);

    }
}
