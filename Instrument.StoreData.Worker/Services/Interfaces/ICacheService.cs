namespace Instrument.StoreData.Worker.Services.Interfaces
{
    public interface ICacheService
    {
        Task SaveInstrumentPrice(Dictionary<string, double?> prices);
    }
}
