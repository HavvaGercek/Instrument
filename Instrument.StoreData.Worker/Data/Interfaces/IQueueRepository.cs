using Instrument.StoreData.Worker.Models.Domain;

namespace Instrument.StoreData.Worker.Data.Interfaces
{
    public interface IQueueRepository
    {
        Task AddAsync(QueueAddModel model);
    }
}
