using InventoryTracker.Domain.Attributes;
using System;

namespace InventoryTracker.Domain
{
    public interface IBaseDomain
    {
        public string UniqueIdentifier { get; }
        
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Version number used to handle concurrency issue
        /// </summary>
        public Guid Version { get; set; }
    }
}