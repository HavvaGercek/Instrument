using Instrument.StoreData.Worker.Helpers;
using Instrument.StoreData.Worker.Models.Response;
using Instrument.StoreData.Worker.Services.Interfaces;
using Instrument.StoreData.Worker.Settings;
using System.Text.Json;

namespace Instrument.StoreData.Worker.Services.Implementations
{
    public class FinanceApiService : IFinanceApiService
    {
        private readonly FinanceApiSettings _financeApiSettings;
        private readonly HttpClient _httpClient;
  
        public FinanceApiService(FinanceApiSettings financeApiSettings,
            HttpClient httpClient)
        {
            _financeApiSettings = financeApiSettings;
            _httpClient = httpClient;
        }

        public async Task<List<MarketSummaryResponse>> GetMarketSummaries()
        {
            var result = new List<MarketSummaryResponse>();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{_financeApiSettings.MarketSummaryUrl}");

            var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                var response = await JsonSerializer.DeserializeAsync
                    <MarketSummaryAndSparkRootResponse>(contentStream, JsonSerializerHelper.JsonSerializerOptions);

                result.AddRange(response.MarketSummaryAndSparkResponse.Result);
            }
            return result;
        }

        public async Task<StockSummaryResponse> GetStockSummary(string symbol)
        {
            var result = new StockSummaryResponse();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, GetApiUrl(_financeApiSettings.StockSummaryUrl, symbol));

            var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                result = await JsonSerializer.DeserializeAsync
                    <StockSummaryResponse>(contentStream, JsonSerializerHelper.JsonSerializerOptions);
            }
            return result;
        }

        public async Task<List<StockSummaryResponse>> GetStockSummaries(IEnumerable<string> symbols)
        {
            var stockSummaryResponses = new List<StockSummaryResponse>();
            var batchSize = 3;//_financeApiSettings.BatchSize
            int numberOfBatches = (int)Math.Ceiling((double)symbols.Count() / batchSize);

            for (int i = 0; i < numberOfBatches; i++)
            {
                var current = symbols.Skip(i * batchSize).Take(batchSize);
                var tasks = current.Select(x => GetStockSummary(x));
                stockSummaryResponses.AddRange(await Task.WhenAll(tasks));
            }

            return stockSummaryResponses;
        }

        private static string GetApiUrl(string url, string symbol)
        {
            return $"{url}?symbol={symbol}&region=US";
        }
    }
}

//exception handling