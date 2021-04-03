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

        void Delete(Expression<Func<Item, bool>> predicate, CancellationToken cancellationToken = default);

        IEnumerable<Item> Find(Expression<Func<Item, bool>> predicate);

        bool HasAny(Expression<Func<Item, bool>> predicate,
            CancellationToken cancellationToken = default);
         
        void Update(Item entity);
    }
}
