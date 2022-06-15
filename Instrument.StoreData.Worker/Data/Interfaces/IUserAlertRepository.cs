using Instrument.StoreData.Worker.Models.Domain;

namespace Instrument.StoreData.Worker.Data.Interfaces
{
    public  interface IUserAlertRepository
    {
        IEnumerable<InstrumentAlertQueueModel> GetUserAlerts(IEnumerable<InstrumentPrice> instrumentSymbols);
    }
}
