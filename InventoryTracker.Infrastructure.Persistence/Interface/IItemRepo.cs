using InventoryTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;

namespace InventoryTracker.Infrastructure.Persistence
{
    public interface IItemRepo
    {
        void Add(Item entity);

        void Delete(Expression<Func<Item, bool>> predicate);

        IEnumerable<Item> Find(Expression<Func<Item, bool>> predicate, int offset,
            int limit, string orderByProperty, bool orderByDesc = false);

        bool HasAny(Expression<Func<Item, bool>> predicate);
         
        void Update(Item entity);
    }
}
