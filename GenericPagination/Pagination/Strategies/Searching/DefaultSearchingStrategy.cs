using GenericPagination.Pagination.Interfaces;
using System.Linq.Dynamic.Core;

namespace GenericPagination.Pagination.Strategies.Searching;

public class DefaultSearchingStrategy : ISearchingStrategy
{
    public IQueryable<T> ApplySearch<T>(IQueryable<T> query, string searchTerm, params string[] searchColumns)
    {
        if (string.IsNullOrWhiteSpace(searchTerm) || searchColumns == null || !searchColumns.Any())
            return query;

        // Dinamik arama sorgusu oluşturma
        var searchExpression = string.Join(" OR ", searchColumns.Select(c => $"{c}.Contains(@0)"));
        return query.Where(searchExpression, searchTerm); // Dynamic LINQ kullanımı
    }
}
