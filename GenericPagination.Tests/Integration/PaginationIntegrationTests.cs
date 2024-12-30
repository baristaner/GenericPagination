using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using GenericPagination.Pagination.Extensions;
using GenericPagination.Tests.TestHelpers;
using System.Threading.Tasks;
using System.Linq;
using GenericPagination.Pagination.Models;
using System.Linq.Expressions;

namespace GenericPagination.Tests.Integration
{
    public class PaginationIntegrationTests
    {
        private TestDbContext _context;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            _context = new TestDbContext(options);

            // Seed test data
            await _context.TestEntities.AddRangeAsync(TestData.GetTestData());
            await _context.SaveChangesAsync();
        }

        [TearDown]
        public async Task Cleanup()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }

        [Test]
        public async Task PaginatedList_WithDatabaseQuery_ShouldReturnCorrectResults()
        {
            // Arrange
            var parameters = new PaginationParameters
            {
                PageIndex = 2,
                PageSize = 10,
                SortColumn = "Id",
                SortOrder = "asc"
            };

            // Act
            var result = await _context.TestEntities
                .OrderBy(x => x.Id)
                .ToPaginatedListAsync(parameters);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Items.Count, Is.EqualTo(parameters.PageSize));
                Assert.That(result.PageNumber, Is.EqualTo(parameters.PageIndex));
                Assert.That(result.TotalCount, Is.EqualTo(100));
                Assert.That(result.Items.First().Id, Is.EqualTo(11));
                Assert.That(result.Items.Last().Id, Is.EqualTo(20));
            });
        }

        [Test]
        public async Task PaginatedList_WithComplexQuery_ShouldReturnCorrectResults()
        {
            // Arrange
            var parameters = new PaginationParameters
            {
                PageIndex = 1,
                PageSize = 5,
                SortColumn = "CreatedDate",
                SortOrder = "desc"
            };

            Expression<Func<TestEntity, bool>> filter = x => x.Id > 50;

            // Act
            var result = await _context.TestEntities
                .Where(filter)
                .ToPaginatedListAsync(parameters, filter);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Items.Count, Is.EqualTo(parameters.PageSize));
                Assert.That(result.TotalCount, Is.EqualTo(50));
                Assert.That(result.Items.All(x => x.Id > 50), Is.True);
                Assert.That(result.Items, Is.Ordered.By("CreatedDate").Descending);
            });
        }

        [Test]
        public async Task PaginatedList_WithSearchTerm_ShouldReturnFilteredResults()
        {
            // Arrange
            var parameters = new PaginationParameters
            {
                PageIndex = 1,
                PageSize = 10,
                SortColumn = "Name",
                SortOrder = "asc",
                SearchTerm = "Test Entity 1" // Sadece "Test Entity 1" ile başlayan kayıtları arar
            };

            // Act
            var result = await _context.TestEntities
                .Where(x => x.Name.Contains(parameters.SearchTerm))
                .ToPaginatedListAsync(parameters);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Items.All(x => x.Name.Contains(parameters.SearchTerm)), Is.True);
                Assert.That(result.Items, Is.Ordered.By("Name"));
            });
        }

        [Test]
        public async Task PaginatedList_WithMaxPageSize_ShouldLimitResults()
        {
            // Arrange
            var parameters = new PaginationParameters
            {
                PageIndex = 1,
                PageSize = 100, // MaxPageSize'dan büyük
                SortColumn = "Id",
                SortOrder = "asc"
            };

            // Act
            var result = await _context.TestEntities.ToPaginatedListAsync(parameters);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.PageSize, Is.EqualTo(50)); // MaxPageSize
                Assert.That(result.Items.Count, Is.EqualTo(50));
            });
        }
    }
} 