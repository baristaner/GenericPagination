using GenericPagination.Pagination.Interfaces;
using System.Linq.Dynamic.Core;

namespace GenericPagination.Pagination.Strategies.Searching;

public class CustomSearchingStrategy : ISearchingStrategy
{
    public IQueryable<T> ApplySearch<T>(IQueryable<T> query, string searchTerm, params string[] searchColumns)
    {
        if (string.IsNullOrWhiteSpace(searchTerm) || searchColumns == null || !searchColumns.Any())
            return query;

        // Özel bir mantıkla sorgu oluşturabiliriz
        // Örnek: yalnızca belirli uzunlukta terimleri arayın
        var filteredColumns = searchColumns.Where(c => c.Length > 3); // Özel filtreleme
        var searchExpression = string.Join(" OR ", filteredColumns.Select(c => $"{c}.Contains(@0)"));
        return query.Where(searchExpression, searchTerm);
    }
}
