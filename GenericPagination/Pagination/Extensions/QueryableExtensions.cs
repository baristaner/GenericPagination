using Microsoft.EntityFrameworkCore;
using GenericPagination.Pagination.Models;
using System.Linq.Expressions;
using GenericPagination.Pagination.Services;

namespace GenericPagination.Pagination.Extensions;

public static class QueryableExtensions
{
    public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(
            this IQueryable<T> query,
            PaginationParameters parameters,
            Expression<Func<T, bool>>? filter = null) where T : class
    {
        // Apply filtering if provided
        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync();

        // Apply pagination and sorting
        var items = await query
            .ApplyPagination(parameters)
            .ToListAsync();

        return new PaginatedList<T>(items, totalCount, parameters.PageIndex, parameters.PageSize);
    }

    public static PaginatedList<T> ToPaginatedList<T>(
            this IQueryable<T> query,
            PaginationParameters parameters,
            Expression<Func<T, bool>>? filter = null) where T : class
    {
        // Apply filtering if provided
        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Get total count before pagination
        var totalCount = query.Count();

        // Apply pagination and sorting
        var items = query
            .ApplyPagination(parameters)
            .ToList();

        return new PaginatedList<T>(items, totalCount, parameters.PageIndex, parameters.PageSize);
    }
}
