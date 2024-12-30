using NUnit.Framework;
using System;
using System.Collections.Generic;
using GenericPagination.Pagination.Models;

namespace GenericPagination.Tests.Unit
{
    public class PaginationTests
    {
        [Test]
        public void Constructor_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var items = new List<string> { "item1", "item2", "item3" };
            const int pageNumber = 2;
            const int pageSize = 10;
            const int totalCount = 30;

            // Act
            var pagination = new PaginatedList<string>(items, totalCount, pageNumber, pageSize);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(pagination.PageNumber, Is.EqualTo(pageNumber));
                Assert.That(pagination.PageSize, Is.EqualTo(pageSize));
                Assert.That(pagination.TotalCount, Is.EqualTo(totalCount));
                Assert.That(pagination.Items, Is.EqualTo(items));
                Assert.That(pagination.TotalPages, Is.EqualTo(3)); // 30 items / 10 per page = 3 pages
            });
        }

        [Test]
        public void Constructor_WithInvalidParameters_ShouldThrowException()
        {
            // Arrange
            var items = new List<string>();

            // Act & Assert
            Assert.Multiple(() =>
            {
                // Test negative page number
                var ex1 = Assert.Throws<ArgumentException>(() => 
                    new PaginatedList<string>(items, 0, -1, 10));
                Assert.That(ex1.Message, Does.Contain("Page number cannot be negative"));

                // Test zero page size
                var ex2 = Assert.Throws<ArgumentException>(() => 
                    new PaginatedList<string>(items, 0, 1, 0));
                Assert.That(ex2.Message, Does.Contain("Page size cannot be zero or negative"));

                // Test negative total count
                var ex3 = Assert.Throws<ArgumentException>(() => 
                    new PaginatedList<string>(items, -1, 1, 10));
                Assert.That(ex3.Message, Does.Contain("Total count cannot be negative"));
            });
        }

        [Test]
        public void CalculateTotalPages_ShouldReturnCorrectValue()
        {
            // Arrange
            var items = new List<string>();
            
            // Act & Assert
            var testCases = new[]
            {
                (totalCount: 100, pageSize: 10, expectedPages: 10),
                (totalCount: 101, pageSize: 10, expectedPages: 11),
                (totalCount: 5, pageSize: 10, expectedPages: 1),
                (totalCount: 0, pageSize: 10, expectedPages: 1)
            };

            foreach (var (totalCount, pageSize, expectedPages) in testCases)
            {
                var pagination = new PaginatedList<string>(items, totalCount, 1, pageSize);
                Assert.That(pagination.TotalPages, Is.EqualTo(expectedPages));
            }
        }

        [Test]
        public void HasPrevious_ShouldReturnCorrectValue()
        {
            // Arrange
            var items = new List<string>();
            
            // Act & Assert
            var testCases = new[]
            {
                (pageNumber: 1, expected: false),
                (pageNumber: 2, expected: true),
                (pageNumber: 10, expected: true)
            };

            foreach (var (pageNumber, expected) in testCases)
            {
                var pagination = new PaginatedList<string>(items, 100, pageNumber, 10);
                Assert.That(pagination.HasPreviousPage, Is.EqualTo(expected));
            }
        }

        [Test]
        public void HasNext_ShouldReturnCorrectValue()
        {
            // Arrange
            var items = new List<string>();
            const int pageSize = 10;
            
            // Act & Assert
            var testCases = new[]
            {
                (totalCount: 100, pageNumber: 10, expected: false),  // Last page
                (totalCount: 100, pageNumber: 9, expected: true),    // Has next
                (totalCount: 5, pageNumber: 1, expected: false)      // Single page
            };

            foreach (var (totalCount, pageNumber, expected) in testCases)
            {
                var pagination = new PaginatedList<string>(items, totalCount, pageNumber, pageSize);
                Assert.That(pagination.HasNextPage, Is.EqualTo(expected));
            }
        }
    }
} 