using GenericPagination.Pagination.Interfaces;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace GenericPagination.Pagination.Strategies.Sorting;

public class CustomSortingStrategy : ISortingStrategy
{
    public IQueryable<T> ApplySorting<T>(IQueryable<T> query, string sortColumn, string sortOrder)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, sortColumn);
        var lambda = Expression.Lambda(property, parameter);

        return sortOrder.ToLower() == "asc"
            ? query.Provider.CreateQuery<T>(
                Expression.Call(typeof(Queryable), "OrderBy", new[] { typeof(T), property.Type },
                    query.Expression, Expression.Quote(lambda)))
            : query.Provider.CreateQuery<T>(
                Expression.Call(typeof(Queryable), "OrderByDescending", new[] { typeof(T), property.Type },
                    query.Expression, Expression.Quote(lambda)));
    }
}