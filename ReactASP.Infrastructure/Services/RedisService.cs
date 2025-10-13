using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactASP.Application.Interfaces.Services;
using StackExchange.Redis;

namespace ReactASP.Infrastructure.Services
{
    public class RedisService :ICacheService
    {
        private readonly IDatabase _cacheDatabase;

        public RedisService(IConnectionMultiplexer connection)
        {
            _cacheDatabase = connection.GetDatabase();
        }

        public async Task SetValueAsync(string key, string value, TimeSpan expiry)
        {
            await _cacheDatabase.StringSetAsync(key, value, expiry);
        }
        public async Task<string?> GetValueAsync(string key)
        {
            var value = await _cacheDatabase.StringGetAsync(key);
            return value.HasValue ? value.ToString() : null;
        }

        public async Task RemoveValueAsync(string key)
        {
            await _cacheDatabase.KeyDeleteAsync(key);
        }
    }
}
