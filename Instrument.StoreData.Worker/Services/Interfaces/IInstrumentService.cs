namespace Instrument.StoreData.Worker.Services.Interfaces
{
    public interface IInstrumentService
    {
        Task LoadDataAsync();
        Task<int> SaveInstrumentsCurrentPrices();
    }
}
