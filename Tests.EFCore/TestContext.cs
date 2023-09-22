using Tests.Models;
using Tests.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Tests.EF
{
    public class TestContext : DbContext
    {
        public TestContext(DbContextOptions<TestContext> options) : base(options)
        {
        }
        
        public DbSet<FilterConditionItem> FilterConditionItems { get; set; }
        public DbSet<NavigationPropertyItem> NavigationPropertyItems { get; set; }
        public DbSet<NestedItem> NestedItems { get; set; }
        public DbSet<StringTestItem> StringTestItems { get; set; }
        public DbSet<CompositeKindItem> CompositeKindItems { get; set; }
        public DbSet<InvalidCaseItem> InvalidCaseItems { get; set; }
        public DbSet<NotAutoFilteredItem> NotAutoFilteredItems { get; set; }
        public DbSet<RangeFilterItem> RangeFilterItems { get; set; }
        public DbSet<RangeFilterNestedItem> RangeFilterNestedItems { get; set; }
        public DbSet<EnumerableFilterItem> EnumerableFilterItems { get; set; }
    }
}
