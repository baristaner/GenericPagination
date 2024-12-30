using GenericPagination.Pagination.Models;
using System.Linq.Expressions;

namespace GenericPagination.Pagination.Interfaces;

public interface IPaginationService
{
    Task<PaginatedList<T>> GetPaginatedListAsync<T>(
        IQueryable<T> query,
        PaginationParameters paginationParameters,
        Expression<Func<T, bool>> filter = null,
        bool asNoTracking = true) where T : class;
}
