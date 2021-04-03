using System;

namespace InventoryTracker.Dto
{
    public class Item
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid Version { get; set; }
    }
}
