using NUnit.Framework;
using GenericPagination.Tests.TestHelpers;
using System.Linq;
using System.Linq.Expressions;
using GenericPagination.Pagination.Extensions;
using GenericPagination.Pagination.Models;

namespace GenericPagination.Tests.Unit
{
    public class QueryableExtensionsTests
    {
        private IQueryable<TestEntity> _testData;
        
        [SetUp]
        public void Setup()
        {
            _testData = TestData.GetTestData(100).AsQueryable();
        }

        [Test]
        public void ToPaginatedList_ShouldReturnCorrectPage()
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
            var result = _testData.ToPaginatedList(parameters);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Items.Count, Is.EqualTo(parameters.PageSize));
                Assert.That(result.PageNumber, Is.EqualTo(parameters.PageIndex));
                Assert.That(result.PageSize, Is.EqualTo(parameters.PageSize));
                Assert.That(result.TotalCount, Is.EqualTo(100));
                Assert.That(result.TotalPages, Is.EqualTo(10));
                Assert.That(result.Items.First().Id, Is.EqualTo(11));
                Assert.That(result.Items.Last().Id, Is.EqualTo(20));
            });
        }

        [Test]
        public void ToPaginatedList_WithEmptySource_ShouldReturnEmptyList()
        {
            // Arrange
            var emptyData = Enumerable.Empty<TestEntity>().AsQueryable();
            var parameters = new PaginationParameters
            {
                PageIndex = 1,
                PageSize = 10
            };

            // Act
            var result = emptyData.ToPaginatedList(parameters);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Items, Is.Empty);
                Assert.That(result.TotalCount, Is.EqualTo(0));
                Assert.That(result.TotalPages, Is.EqualTo(1));
                Assert.That(result.HasNextPage, Is.False);
                Assert.That(result.HasPreviousPage, Is.False);
            });
        }

        [Test]
        public void ToPaginatedList_WithPageSizeLargerThanTotalItems_ShouldReturnAllItems()
        {
            // Arrange
            var smallData = TestData.GetTestData(5).AsQueryable();
            var parameters = new PaginationParameters
            {
                PageIndex = 1,
                PageSize = 10
            };

            // Act
            var result = smallData.ToPaginatedList(parameters);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Items.Count, Is.EqualTo(5));
                Assert.That(result.TotalCount, Is.EqualTo(5));
                Assert.That(result.TotalPages, Is.EqualTo(1));
                Assert.That(result.HasNextPage, Is.False);
            });
        }

        [Test]
        public void ToPaginatedList_WithSorting_ShouldReturnSortedPage()
        {
            // Arrange
            var parameters = new PaginationParameters
            {
                PageIndex = 1,
                PageSize = 10,
                SortColumn = "CreatedDate",
                SortOrder = "desc"
            };

            // Act
            var result = _testData.ToPaginatedList(parameters);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Items.Count, Is.EqualTo(parameters.PageSize));
                Assert.That(result.Items, Is.Ordered.By("CreatedDate").Descending);
            });
        }

        [Test]
        public void ToPaginatedList_WithFiltering_ShouldReturnFilteredPage()
        {
            // Arrange
            var parameters = new PaginationParameters
            {
                PageIndex = 1,
                PageSize = 10,
                SortColumn = "Id",
                SortOrder = "asc"
            };

            Expression<Func<TestEntity, bool>> filter = x => x.Id > 50;

            // Act
            var result = _testData
                .Where(filter)
                .ToPaginatedList(parameters, filter);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.TotalCount, Is.EqualTo(50));
                Assert.That(result.Items.Count, Is.EqualTo(parameters.PageSize));
                Assert.That(result.Items.All(x => x.Id > 50), Is.True);
            });
        }

        [Test]
        public void ToPaginatedList_WithMaxPageSize_ShouldLimitPageSize()
        {
            // Arrange
            var parameters = new PaginationParameters
            {
                PageIndex = 1,
                PageSize = 100 // Bu, MaxPageSize'dan büyük olmalı
            };

            // Act
            var result = _testData.ToPaginatedList(parameters);

            // Assert
            Assert.That(result.PageSize, Is.EqualTo(50)); // MaxPageSize değeri
        }

        [Test]
        public void ToPaginatedList_WithNegativePageIndex_ShouldUseFirstPage()
        {
            // Arrange
            var parameters = new PaginationParameters
            {
                PageIndex = -1,
                PageSize = 10
            };

            // Act
            var result = _testData.ToPaginatedList(parameters);

            // Assert
            Assert.That(result.PageNumber, Is.EqualTo(1));
        }

        [Test]
        public void ToPaginatedList_WithZeroPageSize_ShouldUseMinimumPageSize()
        {
            // Arrange
            var parameters = new PaginationParameters
            {
                PageIndex = 1,
                PageSize = 0
            };

            // Act
            var result = _testData.ToPaginatedList(parameters);

            // Assert
            Assert.That(result.PageSize, Is.EqualTo(1));
        }
    }
} 