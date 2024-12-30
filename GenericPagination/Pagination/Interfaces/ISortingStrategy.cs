namespace GenericPagination.Pagination.Interfaces;

public interface ISortingStrategy
{
    IQueryable<T> ApplySorting<T>(IQueryable<T> query, string sortColumn, string sortOrder);
}
