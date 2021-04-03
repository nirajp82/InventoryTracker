using System;

namespace InventoryTracker.Infrastructure.Persistence
{
    public interface IDateTimeService
    {
        DateTime Current { get; }
    }
}
