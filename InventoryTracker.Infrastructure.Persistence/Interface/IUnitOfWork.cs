using System.Threading;

namespace InventoryTracker.Infrastructure.Persistence
{
    public interface IUnitOfWork
    {
        IItemRepo ItemRepo { get; }

        void Commit(CancellationToken cancellationToken = default);
    }
}
