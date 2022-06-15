using Instrument.StoreData.Worker.Services.Interfaces;
namespace Instrument.StoreData.Worker
{
    public class CurrentPriceWorker : BackgroundService
    {
        private readonly ILogger<CurrentPriceWorker> _logger;
        private readonly IInstrumentService _instrumentService;
        public CurrentPriceWorker(ILogger<CurrentPriceWorker> logger, IInstrumentService instrumentService)
        {
            _logger = logger;
            _instrumentService = instrumentService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _instrumentService.SaveInstrumentsCurrentPrices();
                _logger.LogInformation("CurrentPriceWorker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
