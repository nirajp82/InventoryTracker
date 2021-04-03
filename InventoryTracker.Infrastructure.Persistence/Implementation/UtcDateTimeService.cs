using System;

namespace InventoryTracker.Infrastructure.Persistence
{
    internal class UtcDateTimeService : IDateTimeService
    {
        public DateTime Current
        {
            get
            {
                return DateTime.UtcNow;
            }
        }
    }
}
