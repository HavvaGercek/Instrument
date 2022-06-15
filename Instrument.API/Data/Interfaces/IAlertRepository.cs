namespace Instrument.API.Data.Interfaces
{
    public interface IAlertRepository
    {
        Task CreateAlertForInstrument(string instrumentSymbol, string email, double priceOfInstrument);
    }
}
