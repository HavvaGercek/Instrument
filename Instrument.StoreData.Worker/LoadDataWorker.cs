using Instrument.StoreData.Worker.Services.Interfaces;
namespace Instrument.StoreData.Worker
{
    public class LoadDataWorker : BackgroundService
    {
        private readonly ILogger<LoadDataWorker> _logger;
        private readonly IInstrumentService _instrumentService;
        public LoadDataWorker(ILogger<LoadDataWorker> logger, IInstrumentService instrumentService)
        {
            _logger = logger;
            _instrumentService = instrumentService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _instrumentService.LoadDataAsync();
                _logger.LogInformation("LoadDataWorker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
            }
        }
    }
}
