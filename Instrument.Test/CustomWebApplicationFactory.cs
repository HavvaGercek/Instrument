using Instrument.API.Data.Implementations;
using Instrument.API.Data.Interfaces;
using Instrument.API.Services.Implementations;
using Instrument.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Instrument.Test
{
   
    internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly string _environment;

        public CustomWebApplicationFactory(string environment = "Development")
        {
            _environment = environment;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment(_environment);

            // Add mock/test services to the builder here
            builder.ConfigureServices(services =>
            {
                services.AddTransient<IInstrumentService, InstrumentService>();
                services.AddTransient<IAlertRepository, AlertRepository>();
                services.AddTransient<IInstrumentRepository, InstrumentRepository>();
            });

            return base.CreateHost(builder);
        }
    }
}
