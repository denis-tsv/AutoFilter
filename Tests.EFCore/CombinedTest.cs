using AutoFilter;
using Tests.Models;
using Tests.EF;
using System.Linq;
using Xunit;
using Tests.Data;

namespace Tests
{
    public class CombinedTest : TestBase
    {
        [Fact]
        public void TestSqlAndInMemoryQuery()
        {
            //arrange
            Init();
            var filter = new StringFilter { NoAttribute = "NoAttribute" };

            //act
            var filteredDatabase = Context.StringTestItems.AutoFilter(filter).ToList();
            var filteredInMemory = StringTestsData.Items.AutoFilter(filter).ToList();


            //assert
            Assert.Equal(2, filteredDatabase.Count);
            Assert.Equal(1, filteredInMemory.Count);
        }
    }
}
