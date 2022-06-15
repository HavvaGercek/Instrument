using Instrument.API.Models.Domain;

namespace Instrument.API.Data.Interfaces
{
    public interface IInstrumentRepository
    {
        Task<List<InstrumentInfo>> GetInstrumentList();
        Task<InstrumentSummary> GetInstrumentSummary(string symbol);
    }
}
