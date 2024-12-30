using System.Linq.Expressions;
using GenericPagination.Pagination.Models;
using System.Linq.Dynamic.Core;

namespace GenericPagination.Pagination.Services;

public static class PaginationService
{
    public static IQueryable<T> ApplyPagination<T>(
        this IQueryable<T> query,
        PaginationParameters parameters) where T : class
    {
        // Sıralama uygula
        if (!string.IsNullOrWhiteSpace(parameters.SortColumn))
        {
            var sortOrder = parameters.SortOrder?.ToLower() == "desc" ? "descending" : "ascending";
            query = query.OrderBy($"{parameters.SortColumn} {sortOrder}");
        }

        // Sayfalama uygula
        return query
            .Skip((parameters.PageIndex - 1) * parameters.PageSize)
            .Take(parameters.PageSize);
    }
}
