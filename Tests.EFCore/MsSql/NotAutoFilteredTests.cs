using Tests.Models;
using Xunit;
using AutoFilter;
using System.Linq;

namespace Tests.EF
{
    public class NotAutoFilteredTests : TestBase
    {
        [Fact]
        public void NotAutoFiltered()
        {
            //arrange
            Init();
            var filter = new NotAutoFilteredFilter { NotFiltered = 1 };

            //act
            var filtered = Context.NotAutoFilteredItems.AutoFilter(filter).ToList();

            //assert
            Assert.Equal(Context.NotAutoFilteredItems.Count(), filtered.Count);
        }
    }
}
