using Tests.Models;
using Tests.Models.Models;
#if EF_CORE
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
#elif EF6
using System.Data.Entity;
#endif



namespace Tests.EF
{
    public class TestContext : DbContext
    {
#if EF_CORE
        //public static readonly ILoggerFactory loggerFactory = new LoggerFactory(new[] {
        //      new ConsoleLoggerProvider((_, __) => true, true)
        //});

        //public TestContext(DbContextOptions options) : base(options)
        //{
        //}
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
            //.UseLoggerFactory(loggerFactory)  //tie-up DbContext with LoggerFactory object
            .EnableSensitiveDataLogging()
            .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=AutoFilter;Trusted_Connection=True;MultipleActiveResultSets=true");
            //.UseSqlServer(@"Data Source=127.0.0.1,1433;Initial Catalog=AutoFilter;User Id=sa;Password=<YourStrong!Passw0rd>;");

#elif EF6

        public TestContext() : base(@"Data Source=127.0.0.1,1433;Initial Catalog=AutoFilter;User Id=sa;Password=<YourStrong!Passw0rd>;")
        {}
#endif
        public DbSet<ConvertItem> ConvertItems { get; set; }
        public DbSet<FilterConditionItem> FilterConditionItems { get; set; }
        public DbSet<NavigationPropertyItem> NavigationPropertyItems { get; set; }
        public DbSet<NestedItem> NestedItems { get; set; }
        public DbSet<StringTestItem> StringTestItems { get; set; }
        public DbSet<CompositeKindItem> CompositeKindItems { get; set; }
        public DbSet<InvalidCaseItem> InvalidCaseItems { get; set; }
        public DbSet<NotAutoFilteredItem> NotAutoFilteredItems { get; set; }
        public DbSet<RangeFilterItem> RangeFilterItems { get; set; }
        public DbSet<RangeFilterNestedItem> RangeFilterNestedItems { get; set; }
    }
}
