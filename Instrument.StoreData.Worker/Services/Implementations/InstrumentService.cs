using Instrument.API.Models.Domain;
using Instrument.StoreData.Worker.Data.Interfaces;
using Instrument.StoreData.Worker.Models.Domain;
using Instrument.StoreData.Worker.Services.Interfaces;
using Instrument.StoreData.Worker.Settings;
using StackExchange.Redis;
using System.Text.Json;

namespace Instrument.StoreData.Worker.Services.Implementations
{
    public class InstrumentService : IInstrumentService
    {
        private readonly IInstrumentRepository _instrumentRepository;
        private readonly IFinanceApiService _financeApiService;
        private readonly ICacheService _cacheService;
        private readonly IQueueRepository _queueRepository;
        private readonly IUserAlertRepository _userAlertRepository;
        private readonly EmailQueueSettings _emailQueueSettings;
        public InstrumentService(IInstrumentRepository instrumentRepository, IFinanceApiService financeApiService, ICacheService cacheService, IQueueRepository queueRepository, EmailQueueSettings emailQueueSettings, IUserAlertRepository userAlertRepository)
        {

            _instrumentRepository = instrumentRepository;
            _financeApiService = financeApiService;
            _cacheService = cacheService;
            _queueRepository = queueRepository;
            _emailQueueSettings = emailQueueSettings;
            _userAlertRepository = userAlertRepository;
        }
       
        public async Task LoadDataAsync()
        {
            await SaveInstruments();
            await SaveInstrumentSummaries();
        }
        private async Task SaveInstruments()
        {
           var marketSummaries = await _financeApiService.GetMarketSummaries();
           if (marketSummaries == null)
                return;

           await _instrumentRepository.SaveMarketSummaries(marketSummaries.Select(x => new InstrumentInfo
            {
                Symbol = x.Symbol,
                QuoteType = x.QuoteType,
                ShortName = x.ShortName
            }).ToList());

        }

        private async Task SaveInstrumentSummaries(IEnumerable<string> symbols)
        {
            var stockSummaries = await _financeApiService.GetStockSummaries(symbols);
            if (stockSummaries == null)
                return;

            await _instrumentRepository.SaveStockSummaries(stockSummaries.Where(x=>x.Symbol != null).Select(x => new InstrumentSummary
            {
                Symbol = x.Symbol,
                Currency = x.Price?.Currency,
                CurrencySymbol = x.Price?.CurrencySymbol,
                ExchangeName = x.Price?.ExchangeName,
                Price = x.Price?.RegularMarketPrice?.Raw,
                LongName = x.Price?.LongName,
                ShortName = x.Price?.ShortName,
                Recommendation = x.FinancialData?.RecommendationKey,
                Summary = x.SummaryProfile?.LongBusinessSummary
            }).ToList());
        }

        private async Task SaveInstrumentSummaries()
        {
            var taskList = new List<Task>();
            var symbols = _instrumentRepository.GetMarketSummarySymbolList();
            var batchSize = 100;
            int numberOfBatches = (int)Math.Ceiling((double)symbols.Count() / batchSize);

            for (int i = 0; i < numberOfBatches; i++)
            {
                var current = symbols.Skip(i * batchSize).Take(batchSize);
                var task = SaveInstrumentSummaries(current);
                taskList.Add(task);
            }
            await Task.WhenAll(taskList);
        }

        public async Task<int> SaveInstrumentsCurrentPrices()
        {
            var taskList = new List<Task>();
            var symbols = _instrumentRepository.GetMarketSummarySymbolList();
            var batchSize = 1;
            var count = symbols.Count();
            int numberOfBatches = 1;//(int)Math.Ceiling((double)count / batchSize);

            for (int i = 0; i < numberOfBatches; i++)
            {
                var current = symbols.Skip(i * batchSize).Take(batchSize);
                var task = SaveInstrumentsCurrentPrices(current);
                taskList.Add(task);
            }
            await Task.WhenAll(taskList);
            return count;
        }

        private async Task SaveInstrumentsCurrentPrices(IEnumerable<string> symbols)
        {
            var stockSummaries = await _financeApiService.GetStockSummaries(symbols);
            if (stockSummaries == null)
                return;

            stockSummaries = stockSummaries.Where(x => x.Symbol != null && x.Price != null).ToList();
            var dic = stockSummaries.ToDictionary(x => x.Symbol, x => x.Price?.RegularMarketPrice?.Raw);
            await _cacheService.SaveInstrumentPrice(dic);
            await SendPriceAlertMailToUsers(stockSummaries.Select(x=> new InstrumentPrice { Price = x.Price?.RegularMarketPrice?.Raw, Symbol = x.Symbol }));
        }

        private async Task SendPriceAlertMailToUsers(IEnumerable<InstrumentPrice> prices)
        {
            var alerts = _userAlertRepository.GetUserAlerts(prices);
            if (alerts.Any())
            {
                await _queueRepository.AddAsync(new QueueAddModel { Body = JsonSerializer.Serialize(alerts) });
            }
        }
    }
}
