using Instrument.API.Data.Interfaces;
using Instrument.API.Models.Domain;
using Instrument.API.Settings;
using MongoDB.Driver;

namespace Instrument.API.Data.Implementations
{
    public class InstrumentRepository : IInstrumentRepository
    {
        private IMongoCollection<InstrumentInfo> _instrumentCollection;
        private IMongoCollection<InstrumentSummary> _instrumentSummaryCollection;
        private readonly InstrumentDatabaseSettings _instrumentDatabaseSettings;
        public InstrumentRepository(InstrumentDatabaseSettings instrumentDatabaseSettings)
        {
            _instrumentDatabaseSettings = instrumentDatabaseSettings;

            var mongoClient = new MongoClient(
                _instrumentDatabaseSettings.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                _instrumentDatabaseSettings.DatabaseName);

            _instrumentCollection = mongoDatabase.GetCollection<InstrumentInfo>(
                _instrumentDatabaseSettings.MarketSummaryCollectionName);

            _instrumentSummaryCollection = mongoDatabase.GetCollection<InstrumentSummary>(
                _instrumentDatabaseSettings.StockSummaryCollectionName);
        }

        public async Task<List<InstrumentInfo>> GetInstrumentList()
        {
            return await _instrumentCollection.Find(x=> true).ToListAsync();
        }

        public async Task<InstrumentSummary> GetInstrumentSummary(string symbol)
        {
            return await _instrumentSummaryCollection.Find(x => x.Symbol == symbol).FirstOrDefaultAsync();
        }
    }
}
