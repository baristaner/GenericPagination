namespace GenericPagination.Pagination.Models;

public class PaginatedList<T>
{
    public List<T> Items { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public PaginatedList(List<T> items, int totalCount, int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
            throw new ArgumentException("Page number cannot be negative", nameof(pageNumber));
        
        if (pageSize <= 0)
            throw new ArgumentException("Page size cannot be zero or negative", nameof(pageSize));
        
        if (totalCount < 0)
            throw new ArgumentException("Total count cannot be negative", nameof(totalCount));

        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        
        if (TotalPages == 0)
            TotalPages = 1;
    }
}
