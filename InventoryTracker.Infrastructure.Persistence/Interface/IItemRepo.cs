using InventoryTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InventoryTracker.Infrastructure.Persistence
{
    public interface IItemRepo
    {
        void Delete(string name);

        int Count(Expression<Func<Item, bool>> predicate);

        Item Find(string name);

        IEnumerable<Item> Find(Expression<Func<Item, bool>> predicate, int? offset = default,
            int? limit = default, string orderByProperty = default, bool orderByDesc = false);

        IEnumerable<Item> GetAll();

        bool HasAny(Expression<Func<Item, bool>> predicate);

        void Save(Item item);

        void Save(IEnumerable<Item> list);
    }
}
