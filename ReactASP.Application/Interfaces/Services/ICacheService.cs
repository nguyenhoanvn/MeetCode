using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactASP.Application.Interfaces.Services
{
    public interface ICacheService
    {
        Task SetValueAsync(string key, string value, TimeSpan expiry);
        Task<string?> GetValueAsync(string key);
        Task RemoveValueAsync(string key);
    }
}
