using Tests.Models;
using Xunit;
using AutoFilter;
using System.Linq;
using Tests.Data;

namespace Tests.InMemory
{
    public class NotAutoFilteredTests
    {
        [Fact]
        public void NotAutoFiltered()
        {
            //arrange
            var filter = new NotAutoFilteredFilter { NotFiltered = 1 };

            //act
            var filtered = NotAutoFilteredData.Items.AutoFilter(filter).ToList();

            //assert
            Assert.Equal(NotAutoFilteredData.Items.Count, filtered.Count);
        }
    }
}
