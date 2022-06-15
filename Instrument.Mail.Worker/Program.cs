using Instrument.Mail.Worker;
using Instrument.Mail.Worker.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IMailService, MailService>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
