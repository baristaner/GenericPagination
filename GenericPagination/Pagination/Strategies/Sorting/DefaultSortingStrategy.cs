using GenericPagination.Pagination.Interfaces;
using System.Linq.Dynamic.Core;

namespace GenericPagination.Pagination.Strategies.Sorting;

public class DefaultSortingStrategy : ISortingStrategy
{
    public IQueryable<T> ApplySorting<T>(IQueryable<T> query, string sortColumn, string sortOrder)
    {
        // Null kontrolü
        if (query == null) throw new ArgumentNullException(nameof(query));

        // Geçerli sıralama düzeni kontrolü
        if (sortOrder != "asc" && sortOrder != "desc")
        {
            sortOrder = "asc"; // Varsayılan sıralama düzeni
        }

        // Geçersiz sütun adı kontrolü (örnek: burada basit bir kontrol yapıyoruz)
        if (string.IsNullOrWhiteSpace(sortColumn))
        {
            throw new ArgumentException("Sort column cannot be null or empty.", nameof(sortColumn));
        }

        // Dinamik sıralama
        string orderBy = $"{sortColumn} {sortOrder}";
        return query.OrderBy(orderBy);
    }
}
