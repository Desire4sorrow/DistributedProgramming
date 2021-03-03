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
        private readonly RedisKey _textIdentifiersKey = "textIdentifiers";
        private readonly IDatabase _db;
        public RedisStorage(ILogger<RedisStorage> logger)
        {
            _logger = logger;
            _connectionMultiplexer = ConnectionMultiplexer.Connect("localhost, allowAdmin=true");

        }
        public void Store(string key, string value)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            db.StringSet(key, value);
        }

        public void StoreKey(string key)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            db.ListRightPush(_textIdentifiersKey, key);
        }

        public bool Indeed(string prefix, string value)
        {
            var connection = _connectionMultiplexer.GetServer("localhost", 6379);
            var values = connection.Keys(pattern: "*" + prefix + "*").Select(x => Load(x)).ToList();
            bool foundation = values.Exists(x => x == value);

            return foundation;
        }

        public List<string> TextSignes()
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            return db.ListRange(_textIdentifiersKey).Select(x => x.ToString()).ToList();
        }

        public string Load(string key)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            return db.StringGet(key);
        }


    }
}