using Instrument.API.Data.Interfaces;
using Instrument.API.Models.Domain;
using Instrument.API.Services.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Instrument.API.Services.Implementations
{
    public class InstrumentService : IInstrumentService
    {
        private readonly IInstrumentRepository _instrumentRepository;
        private readonly IAlertRepository _alertRepository;
        private readonly IDatabase _cache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;


        public InstrumentService(IInstrumentRepository instrumentRepository, IAlertRepository alertRepository, IConnectionMultiplexer connectionMultiplexer)
        {
            _instrumentRepository = instrumentRepository;
            _alertRepository = alertRepository;
            _connectionMultiplexer = connectionMultiplexer;
            _connectionMultiplexer = connectionMultiplexer;
            _cache = _connectionMultiplexer.GetDatabase();
            
        }
        public async Task CreateAlertForInstrument(string instrumentSymbol, string email, double priceOfInstrument)
        {
           await _alertRepository.CreateAlertForInstrument(instrumentSymbol, email, priceOfInstrument);
        }

        public async Task<string> GetInstrumentCurrentPrice(string symbol)
        {
            var result = await _cache.StringGetAsync(symbol);
     
            return result;
        }

        public async Task<List<InstrumentInfo>> GetInstrumentList()
        {
            return await _instrumentRepository.GetInstrumentList();
        }

        public async Task<InstrumentSummary> GetInstrumentSummary(string symbol)
        {
            return await _instrumentRepository.GetInstrumentSummary(symbol);
        }
    }
}
