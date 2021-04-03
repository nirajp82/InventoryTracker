using InventoryTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InventoryTracker.Infrastructure.Persistence
{
    internal interface IRepositoryBase<TDomain> where TDomain : IBaseDomain
    {
        void Add(TDomain entity);

        int Count(Expression<Func<TDomain, bool>> predicate);

        void Delete(Expression<Func<TDomain, bool>> predicate);

        TDomain FindFirst(Expression<Func<TDomain, bool>> predicate);

        IEnumerable<TDomain> Find(Expression<Func<TDomain, bool>> predicate);

        IEnumerable<TDomain> Find(Expression<Func<TDomain, bool>> predicate, int? offset = default, 
            int? limit = default, string orderByProperty = default, bool orderByDesc = false);

        bool HasAny(Expression<Func<TDomain, bool>> predicate);

        void Update(TDomain entity);
    }
}
