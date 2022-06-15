using EasyNetQ;
using Instrument.StoreData.Worker;
using Instrument.StoreData.Worker.Data.Implementations;
using Instrument.StoreData.Worker.Data.Interfaces;
using Instrument.StoreData.Worker.Extensions;
using Instrument.StoreData.Worker.Services.Implementations;
using Instrument.StoreData.Worker.Services.Interfaces;
using Instrument.StoreData.Worker.Settings;
using StackExchange.Redis;

var configuration = new ConfigurationBuilder()
     .SetBasePath(Directory.GetCurrentDirectory())
     .AddJsonFile($"appsettings.json");

var config = configuration.Build();
var financeApiSettings = config.GetSection("FinanceApiSettings").Get<FinanceApiSettings>();
var instrumentDatabaseSettings = config.GetSection("InstrumentDatabaseSettings").Get<InstrumentDatabaseSettings>();
var emailQueueSettings = config.GetSection("EmailQueueSettings").Get<EmailQueueSettings>();
var redisConnection = config.GetConnectionString("Redis");
var multiplexer = ConnectionMultiplexer.Connect(redisConnection);


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton(financeApiSettings);
        services.AddSingleton(instrumentDatabaseSettings);
        services.AddSingleton(emailQueueSettings);
        services.AddSingleton<IFinanceApiService, FinanceApiService>();
        services.AddSingleton<ICacheService, CacheService>();
        services.AddSingleton<IInstrumentService, InstrumentService>();
        services.AddSingleton<IInstrumentRepository, InstrumentRepository>();
        services.AddSingleton<IQueueRepository, QueueRepository>();
        services.AddSingleton<IUserAlertRepository, UserAlertRepository>();
        services.AddHttpClientToFinanceApiService(financeApiSettings);
        services.AddSingleton<IConnectionMultiplexer>(multiplexer);
        services.AddHostedService<LoadDataWorker>();
        services.AddHostedService<CurrentPriceWorker>();
    })
    .Build();

await host.RunAsync();
