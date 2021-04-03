using InventoryTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;

namespace InventoryTracker.Infrastructure.Persistence
{
    internal interface IRepositoryBase<TDomain> where TDomain : IBaseDomain
    {
        void Add(TDomain entity);

        void Delete(Expression<Func<TDomain, bool>> predicate);

        IEnumerable<TDomain> Find(Expression<Func<TDomain, bool>> predicate);

        IEnumerable<TDomain> Find(Expression<Func<TDomain, bool>> predicate, int offset, 
            int limit, string orderByProperty, bool orderByDesc = false);

        bool HasAny(Expression<Func<TDomain, bool>> predicate);

        void Update(TDomain entity);
    }
}
