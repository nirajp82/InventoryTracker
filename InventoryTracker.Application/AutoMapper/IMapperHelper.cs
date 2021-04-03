using System.Collections.Generic;

namespace InventoryTracker.Application
{
    public interface IMapperHelper
    {
        dest Map<src, dest>(src entity);

        IEnumerable<dest> MapList<src, dest>(IEnumerable<src> list);
    }
}
