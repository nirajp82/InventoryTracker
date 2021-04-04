using System;

namespace InventoryTracker.Dto
{
    public abstract class BaseItem
    {
        public virtual string Name { get; set; }
        public int Quantity { get; set; }
    } 

    public class Item : BaseItem
    {
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Version number used to handle concurrency issue
        /// </summary>
        public Guid Version { get; set; }
    }
}
