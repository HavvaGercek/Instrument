using Instrument.API.Models.Domain;

namespace Instrument.StoreData.Worker.Data.Interfaces
{
    public interface IInstrumentRepository
    {
        Task SaveMarketSummaries(List<InstrumentInfo> marketSummaryList);
        Task SaveStockSummaries(List<InstrumentSummary> stockSummaryList);
        List<string> GetMarketSummarySymbolList();
        
    }
}
