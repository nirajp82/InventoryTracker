using InventoryTracker.Domain.Attributes;
using System;

namespace InventoryTracker.Domain
{
    public interface IBaseDomain
    {
        public string UniqueIdentifier { get; }
        
        public DateTime CreatedOn { get; set; }
        
        public Guid Version { get; set; }
    }
}