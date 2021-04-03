using System;

namespace InventoryTracker.Domain
{
    public class Item : IBaseDomain
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid Version { get; set; }
        public string UniqueIdentifier { get => Name; }

        public override string ToString()
        {
            return $"Name:{Name} Quantity: {Quantity}";
        }
    }
}
