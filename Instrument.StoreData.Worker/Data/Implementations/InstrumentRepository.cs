using Instrument.API.Models.Domain;
using Instrument.StoreData.Worker.Data.Interfaces;
using Instrument.StoreData.Worker.Settings;
using MongoDB.Driver;

namespace Instrument.StoreData.Worker.Data.Implementations
{
    public class InstrumentRepository : IInstrumentRepository
    {
        private IMongoCollection<InstrumentInfo> _marketSummaryCollection;
        private IMongoCollection<InstrumentSummary> _stockSummaryCollection;
        private readonly InstrumentDatabaseSettings _instrumentDatabaseSettings;
        public InstrumentRepository(InstrumentDatabaseSettings instrumentDatabaseSettings)
        {
            _instrumentDatabaseSettings = instrumentDatabaseSettings;

            var mongoClient = new MongoClient(
                _instrumentDatabaseSettings.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                _instrumentDatabaseSettings.DatabaseName);

            _marketSummaryCollection = mongoDatabase.GetCollection<InstrumentInfo>(
                _instrumentDatabaseSettings.MarketSummaryCollectionName);

            _stockSummaryCollection = mongoDatabase.GetCollection<InstrumentSummary>(
                _instrumentDatabaseSettings.StockSummaryCollectionName);
        }
        public async Task SaveMarketSummaries(List<InstrumentInfo> marketSummaryList)
        {
            var bulkUpdatModel = new List<UpdateOneModel<InstrumentInfo>>();

            foreach (var record in marketSummaryList)
            {
                var update = Builders<InstrumentInfo>.Update.Set(x=> x.QuoteType, record.QuoteType).Set(x => x.ShortName, record.ShortName).Set(x => x.Symbol, record.Symbol);
                var filter = Builders<InstrumentInfo>.Filter.Eq(x=> x.Symbol, record.Symbol);

                var upsert = new UpdateOneModel<InstrumentInfo>(filter, update) { IsUpsert = true };

                bulkUpdatModel.Add(upsert);
            }

            await _marketSummaryCollection.BulkWriteAsync(bulkUpdatModel);
            bulkUpdatModel.Clear();
        }

        public async Task SaveStockSummaries(List<InstrumentSummary> stockSummaryList)
        {
            var bulkUpdatModel = new List<UpdateOneModel<InstrumentSummary>>();

            foreach (var record in stockSummaryList)
            {
                var update = Builders<InstrumentSummary>.Update.Set(x => x.Currency, record.Currency)
                    .Set(x => x.CurrencySymbol, record.CurrencySymbol)
                    .Set(x => x.Price, record.Price)
                    .Set(x => x.ExchangeName, record.ExchangeName)
                    .Set(x => x.LongName, record.LongName)
                    .Set(x => x.ShortName, record.ShortName)
                    .Set(x => x.Summary, record.Summary)
                    .Set(x => x.Recommendation, record.Recommendation)
                    .Set(x => x.Symbol, record.Symbol);
                var filter = Builders<InstrumentSummary>.Filter.Eq(x => x.Symbol, record.Symbol);

                var upsert = new UpdateOneModel<InstrumentSummary>(filter, update) { IsUpsert = true };

                bulkUpdatModel.Add(upsert);
            }

            await _stockSummaryCollection.BulkWriteAsync(bulkUpdatModel);
            bulkUpdatModel.Clear();
        }

        public List<string> GetMarketSummarySymbolList()
        {
            return _marketSummaryCollection.AsQueryable().Select(x=> x.Symbol).ToList();
        }
    }
}
