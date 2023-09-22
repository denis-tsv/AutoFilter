using AutoFilter;
using Tests.Models;
using Tests.EF;
using System.Linq;
using Xunit;
using Tests.Data;

namespace Tests
{
    public class CombinedTest
    {
        [Fact]
        public void TestSqlAndInMemoryQuery()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new StringFilter { NoAttribute = "NoAttribute" };

            //act
            var filteredDatabase = context.StringTestItems.AutoFilter(filter).ToList();
            var filteredInMemory = StringTestsData.Items.AutoFilter(filter).ToList();


            //assert
            Assert.Equal(1, filteredDatabase.Count);
            Assert.Equal(1, filteredInMemory.Count);
        }
    }
}
