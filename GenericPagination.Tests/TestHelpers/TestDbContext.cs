using Microsoft.EntityFrameworkCore;

namespace GenericPagination.Tests.TestHelpers
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) 
            : base(options)
        {
        }

        public DbSet<TestEntity> TestEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure test entity
        }
    }
} 