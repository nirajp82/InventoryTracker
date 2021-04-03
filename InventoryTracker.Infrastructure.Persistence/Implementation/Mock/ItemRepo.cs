using InventoryTracker.Domain;

namespace InventoryTracker.Infrastructure.Persistence.Mock
{
    internal class ItemRepo : RepositoryBase<Item>, IItemRepo, IMockRepo
    {
    }
}
