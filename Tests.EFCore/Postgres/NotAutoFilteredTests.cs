using Tests.Models;
using Xunit;
using AutoFilter;
using System.Linq;

namespace Tests.EF
{
    public class NotAutoFilteredTests 
    {
        [Fact]
        public void NotAutoFiltered()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new NotAutoFilteredFilter { NotFiltered = 1 };

            //act
            var filtered = context.NotAutoFilteredItems.AutoFilter(filter).ToList();

            //assert
            Assert.Equal(context.NotAutoFilteredItems.Count(), filtered.Count);
        }
    }
}
