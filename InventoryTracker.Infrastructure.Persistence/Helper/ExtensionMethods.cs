using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InventoryTracker.Infrastructure.Persistence
{
    static class ExtensionMethods
    {
        public static IEnumerable<TEntity> OrderBy<TEntity>(this IEnumerable<TEntity> source, string orderByProperty,
                          bool orderByDesc)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            IQueryable qrySource = source.AsQueryable();

            string command = orderByDesc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                          qrySource.Expression, Expression.Quote(orderByExpression));
            return qrySource.Provider.CreateQuery<TEntity>(resultExpression);
        }
    }
}
