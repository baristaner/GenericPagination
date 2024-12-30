using System;
using System.Collections.Generic;

namespace GenericPagination.Tests.TestHelpers
{
    public static class TestData
    {
        public static List<TestEntity> GetTestData(int count = 100)
        {
            var testData = new List<TestEntity>();
            
            for (int i = 1; i <= count; i++)
            {
                testData.Add(new TestEntity
                {
                    Id = i,
                    Name = $"Test Entity {i}",
                    CreatedDate = DateTime.Now.AddDays(-i)
                });
            }
            
            return testData;
        }
    }
} 