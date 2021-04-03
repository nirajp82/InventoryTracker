using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application
{
    public interface IMapperHelper
    {
        dest Map<src, dest>(src entity);

        IEnumerable<dest> MapList<src, dest>(IEnumerable<src> list);
    }
}
