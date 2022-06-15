using Instrument.StoreData.Worker.Models.Response;
namespace Instrument.StoreData.Worker.Services.Interfaces
{
    public interface IFinanceApiService
    {
        Task<List<MarketSummaryResponse>> GetMarketSummaries();
        Task<List<StockSummaryResponse>> GetStockSummaries(IEnumerable<string> symbols);
    }
}
