using InventoryTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InventoryTracker.Infrastructure.Persistence.Mock
{
    internal class ItemRepo : RepositoryBase<Item>, IItemRepo, IMockRepo
    {
    }
}
