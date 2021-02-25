using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;

namespace Valuator
{
    public class RedisStorage: IStorage
    {
        private readonly ILogger<RedisStorage> _logger;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _db;
        public RedisStorage(ILogger<RedisStorage> logger)
        {
            _logger = logger;
            _connectionMultiplexer = ConnectionMultiplexer.Connect("localhost, allowAdmin=true");

        }
        public string Load(string key)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            return db.StringGet(key);
        }

        public void Store(string key, string value)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            db.StringSet(key, value);
        }

        public bool Indeed(string prefix, string value)
        {
            var connection = _connectionMultiplexer.GetServer("localhost", 6379);
            var values = connection.Keys(pattern: "*" + prefix + "*").Select(x => Load(x)).ToList();
            bool foundation = values.Exists(x => x == value);

            return foundation;
        }

    }
}