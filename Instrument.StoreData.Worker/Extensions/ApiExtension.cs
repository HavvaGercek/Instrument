using Instrument.StoreData.Worker.Services.Implementations;
using Instrument.StoreData.Worker.Services.Interfaces;
using Instrument.StoreData.Worker.Settings;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace Instrument.StoreData.Worker.Extensions
{
    public static class ApiExtension
    {
        public static void AddHttpClientToFinanceApiService(this IServiceCollection serviceCollection, FinanceApiSettings financeApiSettings)
        {
            serviceCollection.AddHttpClient<IFinanceApiService, FinanceApiService>(c =>
            {
                c.BaseAddress = new Uri(financeApiSettings.BaseUrl);
                c.DefaultRequestHeaders.Accept.Clear();
                c.DefaultRequestHeaders.Add("X-RapidAPI-Key", financeApiSettings.ApiKey);
                c.DefaultRequestHeaders.Add("X-RapidAPI-Host", financeApiSettings.ApiHost);
            }).SetHandlerLifetime(TimeSpan.FromMinutes(financeApiSettings.HttpClientLifeTime))
            .ConfigureHttpClient(x => x.Timeout = TimeSpan.FromSeconds(financeApiSettings.Timeout))
            .AddPolicyHandler((service, request) => HttpPolicyExtensions.HandleTransientHttpError().Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(new[]
            {
                TimeSpan.FromMilliseconds(500),
                TimeSpan.FromMilliseconds(500),
                TimeSpan.FromMilliseconds(500)
            }, onRetry: (result, timeSpan, retryAttempt, context) => { OnRetry<FinanceApiService>(service, result, timeSpan, retryAttempt, context); }
            ).WrapAsync(Policy.TimeoutAsync<HttpResponseMessage>(timeout: TimeSpan.FromSeconds(financeApiSettings.Timeout),
            onTimeoutAsync: (context, timeSpan, task) => OnTimeoutAsync<FinanceApiService>(service, context, timeSpan, task))));
        }

        private static void OnRetry<T>(IServiceProvider service, DelegateResult<HttpResponseMessage> result, TimeSpan timeSpan, int retryAttempt, Context context)
        {
            var resultMessage = result.Result != null ? $"Request failed. {result.Result.StatusCode}. Retry Attempt: {retryAttempt}" : $"Request failed. Retry attempt {retryAttempt}";
            var logger = service.GetService<ILogger<T>>();
            logger.LogError(resultMessage);
        }

        private static Task OnTimeoutAsync<T>(IServiceProvider service, Context context, TimeSpan timeSpan, Task task)
        {
            var logger = service.GetService<ILogger<T>>();
            logger.LogError($"Execution timeout. {timeSpan} seconds.");
            return Task.CompletedTask;
        }
    }
}
