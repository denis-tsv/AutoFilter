using AutoFilter;
using Tests.Models;
using System.Linq;
using Xunit;

namespace Tests.EF
{
    public class CompositeKindTests : TestBase
    {
        [Fact]
        public void AndTest()
        {
            //arrange
            Init();
            var filter = new CompositeKindFilter { Int1 = 1, Int2 = 1 };

            //act
            var filtered = Context.CompositeKindItems.AutoFilter(filter, ComposeKind.And).ToList();

            //assert
            Assert.Equal(1, filtered.Count);            
        }

        [Fact]
        public void OrTest()
        {
            //arrange
            Init();
            var filter = new CompositeKindFilter { Int1 = 1, Int2 = 1 };

            //act
            var filtered = Context.CompositeKindItems.AutoFilter(filter, ComposeKind.Or).ToList();

            //assert
            Assert.Equal(3, filtered.Count);
        }
    }
}
