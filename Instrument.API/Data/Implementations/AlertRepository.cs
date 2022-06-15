using Instrument.API.Data.Interfaces;
using Instrument.API.Models.Domain;
using Instrument.API.Settings;
using MongoDB.Driver;

namespace Instrument.API.Data.Implementations
{
    public class AlertRepository : IAlertRepository
    {
        private IMongoCollection<InstrumentAlert> _instrumentAlertRepository;
        private readonly InstrumentDatabaseSettings _instrumentDatabaseSettings;
        public AlertRepository(InstrumentDatabaseSettings instrumentDatabaseSettings)
        {
            _instrumentDatabaseSettings = instrumentDatabaseSettings;
            var mongoClient = new MongoClient(
              _instrumentDatabaseSettings.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                _instrumentDatabaseSettings.DatabaseName);

            _instrumentAlertRepository = mongoDatabase.GetCollection<InstrumentAlert>(
                _instrumentDatabaseSettings.AlertCollectionName);
        }

        public async Task CreateAlertForInstrument(string instrumentSymbol, string email, double priceOfInstrument)
        {
            var builder = Builders<InstrumentAlert>.Filter;
            var filter = builder.Eq(x => x.Symbol, instrumentSymbol) & builder.Eq(x => x.Email, email) & builder.Eq(x => x.Price, priceOfInstrument);
            var alert = await _instrumentAlertRepository.FindAsync(filter);
            if (alert.FirstOrDefault() == null)
            {
                await _instrumentAlertRepository.InsertOneAsync(new InstrumentAlert { Email = email, Price = priceOfInstrument, Symbol = instrumentSymbol });
            }
        
        }
    }
}
