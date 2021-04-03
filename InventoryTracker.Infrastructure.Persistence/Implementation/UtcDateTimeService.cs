using System;
using System.Collections.Generic;
using System.Text;

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
