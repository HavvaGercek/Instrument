using Instrument.API.Data.Implementations;
using Instrument.API.Data.Interfaces;
using Instrument.API.Filters;
using Instrument.API.Services.Implementations;
using Instrument.API.Services.Interfaces;
using Instrument.API.Settings;
using StackExchange.Redis;

var configuration = new ConfigurationBuilder()
     .SetBasePath(Directory.GetCurrentDirectory())
     .AddJsonFile($"appsettings.json");

var config = configuration.Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var redisConnection = config.GetConnectionString("Redis");
var multiplexer = ConnectionMultiplexer.Connect(redisConnection);

builder.Services.AddControllers().AddMvcOptions(options => options.Filters.Add<GlobalExceptionFilter>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IInstrumentService, InstrumentService>();
builder.Services.AddSingleton<IAlertRepository, AlertRepository>();
builder.Services.AddSingleton<IInstrumentRepository, InstrumentRepository>();
var instrumentDatabaseSettings = config.GetSection("InstrumentDatabaseSettings").Get<InstrumentDatabaseSettings>();
builder.Services.AddSingleton(instrumentDatabaseSettings);
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
