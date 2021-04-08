using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Infrastructure.Storage
{
    public class RedisStorage : IStorage
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        private IConnectionMultiplexer _connection;
        private IConnectionMultiplexer _connectionRus;
        private IConnectionMultiplexer _connectionEu;
        private IConnectionMultiplexer _connectionOther;

        private readonly string _setTextKeys = Constants.SetTextKeys;
        public RedisStorage()
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect("localhost, allowAdmin=true");
            _connectionRus = ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable(Constants.RusDB, EnvironmentVariableTarget.User));
            _connectionEu = ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable(Constants.EUDB, EnvironmentVariableTarget.User));
            _connectionOther = ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable(Constants.OtherDB, EnvironmentVariableTarget.User));

        }
        public void Store(string key, string value)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            db.StringSet(key, value);
        }

        public bool TextSignes(string prefix, string text)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            var _server = _connectionMultiplexer.GetServer("localhost", 6379);
            var keys = _server.Keys(pattern: "*" + prefix + "*");
            return keys.Select(x => Load(x)).Where(x => x == text).Any();
        }

        public string Load(string sKey, string key)
        {
            IDatabase db = sKey.GetDatabase();
            return db.StringGet(key);
        }

        public bool CheckingKey(string key)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            return db.KeyExists(key);
        }

    }
}
