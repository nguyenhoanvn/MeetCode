using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MeetCode.Application.Interfaces.Services;
using StackExchange.Redis;

namespace MeetCode.Infrastructure.Services
{
    public class RedisService :ICacheService
    {
        private readonly IDatabase _cacheDatabase;

        public RedisService(IConnectionMultiplexer connection)
        {
            _cacheDatabase = connection.GetDatabase();
        }

        public async Task SetValueAsync<T>(string key, T value, TimeSpan expiry)
        {
            string data;

            if (value is string strValue)
            {
                data = strValue;
            }
            else if (value is ValueType)
            {
                data = value.ToString() ?? string.Empty;
            }
            else
            {
                data = JsonSerializer.Serialize(value);
            }

            await _cacheDatabase.StringSetAsync(key, data, expiry);
        }
        public async Task<T?> GetValueAsync<T>(string key)
        {
            var data = await _cacheDatabase.StringGetAsync(key);
            if (!data.HasValue)
                return default;

            if (typeof(T) == typeof(string))
            {
                return (T)(object)data.ToString();
            }

            return JsonSerializer.Deserialize<T>(data!);
        }

        public async Task RemoveValueAsync(string key)
        {
            await _cacheDatabase.KeyDeleteAsync(key);
        }
        public async Task<bool> ExistsAsync(string key)
        {
            return await _cacheDatabase.KeyExistsAsync(key);
        }
        public async Task RemoveRangeAsync(params string[] keys)
        {
            var redisKey = keys.Select(k => (RedisKey)k).ToArray();
            await _cacheDatabase.KeyDeleteAsync(redisKey);
        }
    }
}
