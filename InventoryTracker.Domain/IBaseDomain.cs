using System;

namespace InventoryTracker.Domain
{
    public interface IBaseDomain : IUniqueIdentifier
    {
        public DateTime CreatedOn { get; set; }
        public Guid Version { get; set; }
    }
}