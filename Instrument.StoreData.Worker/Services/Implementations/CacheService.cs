using Instrument.StoreData.Worker.Services.Interfaces;
using StackExchange.Redis;

namespace Instrument.StoreData.Worker.Services.Implementations
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _cache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public CacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _cache = _connectionMultiplexer.GetDatabase();
            
        }

        public async Task SaveInstrumentPrice(Dictionary<string,double?> prices)
        {
            const int BatchSize = 100;
            var batch = new List<KeyValuePair<RedisKey, RedisValue>>(BatchSize);
            foreach (var pair in prices)
            {
                batch.Add(new KeyValuePair<RedisKey, RedisValue>(pair.Key, pair.Value));
                if (batch.Count == BatchSize)
                {
                    _cache.StringSetAsync(batch.ToArray());
                    batch.Clear();
                }
            }
            if (batch.Count != 0) // final batch
                _cache.StringSetAsync(batch.ToArray());
        }
    }
}
