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

        void Delete(Expression<Func<TDomain, bool>> predicate, CancellationToken cancellationToken = default);

        IEnumerable<TDomain> Find(Expression<Func<TDomain, bool>> predicate);

        bool HasAny(Expression<Func<TDomain, bool>> predicate, CancellationToken cancellationToken = default);

        void Update(TDomain entity);
    }
}
