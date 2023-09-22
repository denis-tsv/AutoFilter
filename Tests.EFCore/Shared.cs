using Tests.Data;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Tests.EF
{
    public static class Shared
    {
        public static TestContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<TestContext>()
                .UseNpgsql(_configuration.GetConnectionString("Postgres"))
                .Options;
            return new TestContext(options);
        }

        private static IConfiguration _configuration;

        static Shared() 
        {
            _configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json")
                .Build();

            var options = new DbContextOptionsBuilder<TestContext>()
                .UseNpgsql(_configuration.GetConnectionString("Postgres"))
                .Options;

            var context = new TestContext(options);
            var created = context.Database.EnsureCreated();

            if (created)
            {
                context.FilterConditionItems.AddRange(FilterConditionTestsData.Items);
                context.StringTestItems.AddRange(StringTestsData.Items);
                context.NestedItems.AddRange(NavigationPropertyTestsData.Items.Where(x => x.NestedItem != null).Select(x => x.NestedItem));
                context.NavigationPropertyItems.AddRange(NavigationPropertyTestsData.Items);
                context.CompositeKindItems.AddRange(CompositeKindTestsData.Items);
                context.InvalidCaseItems.AddRange(InvalidCasesTestsData.Items);
                context.NotAutoFilteredItems.AddRange(NotAutoFilteredData.Items);
                context.RangeFilterItems.AddRange(RangeData.Items);
                context.EnumerableFilterItems.AddRange(EnumerableTestsData.Items);
                context.SaveChanges();
            }
        }
    }
}

