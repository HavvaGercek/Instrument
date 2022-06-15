using Instrument.API.Models.Domain;

namespace Instrument.API.Services.Interfaces
{
    public interface IInstrumentService
    {
        Task<List<InstrumentInfo>> GetInstrumentList();
        Task<InstrumentSummary> GetInstrumentSummary(string symbol);
        Task<string> GetInstrumentCurrentPrice(string symbol);
        Task CreateAlertForInstrument(string instrumentSymbol, string email, double priceOfInstrument);
    }
}
