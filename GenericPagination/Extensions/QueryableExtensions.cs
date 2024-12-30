using System.Linq.Dynamic.Core;

namespace GenericPagination.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplySearch<T>(this IQueryable<T> query, string searchTerm, params string[] searchColumns)
    {
        if (string.IsNullOrWhiteSpace(searchTerm) || searchColumns.Length == 0) return query;

        var searchExpression = string.Join(" OR ", searchColumns.Select(c => $"{c}.Contains(@0)"));
        return query.Where(searchExpression, searchTerm);
    }
}
