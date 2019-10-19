using AutoFilter;
using Tests.Models;
using System.Linq;
using Xunit;
using Tests.Data;

namespace Tests.InMemory
{
    public class NavigationPropertyTests
    {
        [Fact]
        public void IntEqual()
        {
            //arrange
            var filter = new NavigationPropertyFilter { Int = 1 };

            //act
            var filtered = NavigationPropertyTestsData.Items.AutoFilter(filter).ToList();

            //assert
            Assert.Equal(1, filtered.Count);
            Assert.Equal(1, filtered[0].NestedItem.Int);
        }

        [Fact]
        public void NullableIntGreatOrEqual()
        {
            //arrange
            var filter = new NavigationPropertyFilter { NullableIntFilter = 1 };

            //act
            var filtered = NavigationPropertyTestsData.Items.AutoFilter(filter)
                .OrderBy(x => x.NestedItem.NullableInt)
                .ToList();

            //assert
            Assert.Equal(2, filtered.Count);
            Assert.Equal(1, filtered[0].NestedItem.NullableInt);
            Assert.Equal(2, filtered[1].NestedItem.NullableInt);
        }

        [Fact]
        public void NestedString()
        {
            //arrange
            var filter = new NavigationPropertyFilter { StringFilter = "Nested" };

            //act
            var filtered = NavigationPropertyTestsData.Items.AutoFilter(filter).ToList();

            //assert
            Assert.Equal(1, filtered.Count);
            Assert.Equal("Nested", filtered[0].NestedItem.String);
        }
    }
}
