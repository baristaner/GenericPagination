namespace GenericPagination.Pagination.Models;

public class PaginationParameters
{
    private const int MaxPageSize = 50;
    private int _pageSize = 10;
    private int _pageIndex = 1;

    public int PageIndex
    {
        get => _pageIndex;
        set => _pageIndex = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value < 1 ? 1 : value;
    }

    public string SortColumn { get; set; } = "Id";
    public string SortOrder { get; set; } = "asc";
    public string? SearchTerm { get; set; }
}
