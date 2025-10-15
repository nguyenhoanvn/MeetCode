using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactASP.Application.Interfaces.Services
{
    public interface ICacheService
    {
        // Individual value
        Task SetValueAsync<T>(string key, T value, TimeSpan expiry);
        Task<T?> GetValueAsync<T>(string key);
        Task RemoveValueAsync(string key);
        Task<bool> ExistsAsync(string key);
        Task RemoveRangeAsync(params string[] keys);
    }
}
