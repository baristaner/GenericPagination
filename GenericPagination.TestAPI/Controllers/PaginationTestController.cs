using Microsoft.AspNetCore.Mvc;
using GenericPagination.Pagination.Extensions;
using GenericPagination.Pagination.Models;
using GenericPagination.Tests.TestHelpers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GenericPagination.TestAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaginationTestController : ControllerBase
{
    private readonly TestDbContext _context;

    public PaginationTestController(TestDbContext context)
    {
        _context = context;
    }

    [HttpGet("basic")]
    public async Task<ActionResult<PaginatedList<TestEntity>>> GetBasicPagination(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var parameters = new PaginationParameters
        {
            PageIndex = pageNumber,
            PageSize = pageSize
        };

        var result = await _context.TestEntities
            .OrderBy(x => x.Id)
            .ToPaginatedListAsync(parameters);

        return Ok(result);
    }

    [HttpGet("filtered")]
    public async Task<ActionResult<PaginatedList<TestEntity>>> GetFilteredPagination(
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var parameters = new PaginationParameters
        {
            PageIndex = pageNumber,
            PageSize = pageSize
        };

        Expression<Func<TestEntity, bool>>? filter = null;
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            filter = x => x.Name.Contains(searchTerm);
        }

        var result = await _context.TestEntities
            .OrderBy(x => x.Name)
            .ToPaginatedListAsync(parameters, filter);

        return Ok(result);
    }

    [HttpGet("sorted")]
    public async Task<ActionResult<PaginatedList<TestEntity>>> GetSortedPagination(
        [FromQuery] string sortBy = "Id",
        [FromQuery] bool descending = false,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var parameters = new PaginationParameters
        {
            PageIndex = pageNumber,
            PageSize = pageSize
        };

        var query = _context.TestEntities.AsQueryable();

        query = sortBy.ToLower() switch
        {
            "name" => descending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
            "createddate" => descending ? query.OrderByDescending(x => x.CreatedDate) : query.OrderBy(x => x.CreatedDate),
            _ => descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id)
        };

        var result = await query.ToPaginatedListAsync(parameters);

        return Ok(result);
    }

    // Seed test data
    [HttpPost("seed")]
    public async Task<IActionResult> SeedTestData([FromQuery] int count = 100)
    {
        await _context.TestEntities.AddRangeAsync(TestData.GetTestData(count));
        await _context.SaveChangesAsync();
        return Ok($"{count} test records created");
    }
}

// Expression birleştirme için extension method


// DTO for complex pagination example
public class TestEntityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public string FormattedDate { get; set; } = string.Empty;
}