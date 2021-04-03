using System.Collections.Generic;

namespace InventoryTracker.Dto
{
    public class ResponseEnvelope<T>
    {
        public IEnumerable<T> List { get; set; }
        public int Count { get; set; }
    }
}
