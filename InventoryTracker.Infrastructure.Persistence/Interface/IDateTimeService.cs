using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Infrastructure.Persistence
{
    public interface IDateTimeService
    {
        DateTime Current { get; }
    }
}
