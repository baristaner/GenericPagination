namespace GenericPagination.Pagination.Interfaces;

public interface ISearchingStrategy
{
    IQueryable<T> ApplySearch<T>(IQueryable<T> query, string searchTerm, params string[] searchColumns);
}
