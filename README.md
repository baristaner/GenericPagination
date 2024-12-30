# Generic Pagination for .NET

A flexible and easy-to-use pagination library for .NET applications, supporting Entity Framework Core and IQueryable collections.

## Features

- 🚀 Easy to use extension methods
- 📦 Works with Entity Framework Core
- 🔄 Supports async operations
- 🔍 Built-in sorting and filtering
- 📏 Configurable page size limits
- 🎯 LINQ and Dynamic LINQ support

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package GenericPagination
```

```powershell
Install-Package GenericPagination
```

## Quick Start

### Basic Usage
```csharp
// Using with Entity Framework Core
var parameters = new PaginationParameters
{
PageIndex = 1,
PageSize = 10,
SortColumn = "Name",
SortOrder = "asc"
};
var pagedResult = await dbContext.Users
.ToPaginatedListAsync(parameters);
```

### With Filtering

```csharp
// Add filtering to your query
Expression<Func<User, bool>> filter = user => user.Age > 18;
var pagedResult = await dbContext.Users
.ToPaginatedListAsync(parameters, filter);
```

### Complex Queries

```csharp
var parameters = new PaginationParameters
{
PageIndex = 1,
PageSize = 10,
SortColumn = "CreatedDate",
SortOrder = "desc",
SearchTerm = "john"
};
var pagedResult = await dbContext.Users
.Where(u => u.IsActive)
.Include(u => u.Orders)
.ToPaginatedListAsync(parameters);
```

## Pagination Parameters
The `PaginationParameters` class provides several options to control pagination:

```csharp
var parameters = new PaginationParameters
{
PageIndex = 1, // Current page number (1-based)
PageSize = 10, // Items per page (max 50 by default)
SortColumn = "Id", // Column to sort by
SortOrder = "asc", // Sort direction ("asc" or "desc")
SearchTerm = "test" // Optional search term
};
```

## Pagination Result
The `PaginatedList<T>` class contains the following properties:
```csharp
public class PaginatedList<T>
{
public List<T> Items { get; } // The page items
public int PageNumber { get; } // Current page number
public int PageSize { get; } // Items per page
public int TotalCount { get; } // Total number of items
public int TotalPages { get; } // Total number of pages
public bool HasPreviousPage { get; } // Whether there's a previous page
public bool HasNextPage { get; } // Whether there's a next page
}
```

## Advanced Usage
### Custom Sorting

```csharp
// Using with custom sorting logic
var result = await dbContext.Products
.OrderBy(x => x.Category.Name)
.ThenBy(x => x.Price)
.ToPaginatedListAsync(parameters);
```

### With Search Term

```csharp
// Using the search term
var query = dbContext.Products.AsQueryable();
if (!string.IsNullOrEmpty(parameters.SearchTerm))
{
query = query.Where(x => x.Name.Contains(parameters.SearchTerm));
}
var result = await query.ToPaginatedListAsync(parameters);
```

### Multiple Filters

```csharp
// Combining multiple filters
Expression<Func<Product, bool>> priceFilter = p => p.Price > 100;
Expression<Func<Product, bool>> stockFilter = p => p.InStock;
var result = await dbContext.Products
.Where(priceFilter)
.Where(stockFilter)
.ToPaginatedListAsync(parameters);
```

## API Response Example

```json
json
{
"items": [
{
"id": 1,
"name": "Product 1",
"price": 99.99
},
// ... more items
],
"pageNumber": 1,
"pageSize": 10,
"totalCount": 100,
"totalPages": 10,
"hasPreviousPage": false,
"hasNextPage": true
}
```

## Best Practices

1. **Page Size Limits**: The default maximum page size is 50. You can modify this in the `PaginationParameters` class.

2. **Performance**: 
   - Use `AsNoTracking()` when you only need to read data
   - Add appropriate indexes for sorted columns
   - Consider using projection (Select) to limit returned data

3. **Validation**: 
   - PageIndex is automatically validated to be >= 1
   - PageSize is automatically capped between 1 and MaxPageSize

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.