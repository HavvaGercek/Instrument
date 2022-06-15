using Instrument.StoreData.Worker.Data.Interfaces;
using Instrument.StoreData.Worker.Models.Domain;
using Instrument.StoreData.Worker.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instrument.StoreData.Worker.Data.Implementations
{
    public class UserAlertRepository : IUserAlertRepository
    {
        private IMongoCollection<InstrumentAlert> _instrumentAlertRepository;
        private readonly InstrumentDatabaseSettings _instrumentDatabaseSettings;
        public UserAlertRepository(InstrumentDatabaseSettings instrumentDatabaseSettings)
        {
            _instrumentDatabaseSettings = instrumentDatabaseSettings;
            var mongoClient = new MongoClient(
              _instrumentDatabaseSettings.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                _instrumentDatabaseSettings.DatabaseName);

            _instrumentAlertRepository = mongoDatabase.GetCollection<InstrumentAlert>(
                _instrumentDatabaseSettings.AlertCollectionName);
        }
        public IEnumerable<InstrumentAlertQueueModel> GetUserAlerts(IEnumerable<InstrumentPrice> instrumentSymbols)
        {
            try
            {
                if (instrumentSymbols == null)
                    return new List<InstrumentAlertQueueModel>();

                var builder = Builders<InstrumentAlert>.Filter;
                var filter = FilterDefinition<InstrumentAlert>.Empty;
                foreach (var item in instrumentSymbols)
                {
                    if(filter == FilterDefinition<InstrumentAlert>.Empty)
                    {
                        filter = (builder.Eq(x => x.Price, item.Price) & builder.Eq(x => x.Symbol, item.Symbol));
                    }
                    else
                    {
                        filter = filter | (builder.Eq(x => x.Price, item.Price) & builder.Eq(x => x.Symbol, item.Symbol));

                    }
                }
                var result = _instrumentAlertRepository.Find(filter);
                if(!result.Any())
                    return new List<InstrumentAlertQueueModel>();
                return result.ToList().Select(x => new InstrumentAlertQueueModel { Email = x.Email, Price = x.Price, Symbol = x.Symbol});
            }
            catch (Exception ex)
            {

                throw;
            }
       
        }
    }
}
