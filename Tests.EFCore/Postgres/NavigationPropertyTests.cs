using AutoFilter;
using Tests.Models;
#if EF_CORE
using Microsoft.EntityFrameworkCore;
#elif EF6
using System.Data.Entity;
#endif
using System.Linq;
using Xunit;

namespace Tests.EF
{
    public class NavigationPropertyTests 
    {
        [Fact]
        public void IntEqual()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new NavigationPropertyFilter { Int = 1 };

            //act
            var filtered = context.NavigationPropertyItems.Include(x => x.NestedItem).AutoFilter(filter).ToList();

            //assert
            Assert.Equal(1, filtered.Count);
            Assert.Equal(1, filtered[0].NestedItem.Int);
        }

        [Fact]
        public void NullableIntGreatOrEqual()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new NavigationPropertyFilter { NullableIntFilter = 1 };

            //act
            var filtered = context.NavigationPropertyItems
                .Include(x => x.NestedItem)
                .AutoFilter(filter)
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
            var context = Shared.GetDbContext();
            var filter = new NavigationPropertyFilter { StringFilter = "Nested" };

            //act
            var filtered = context.NavigationPropertyItems.Include(x => x.NestedItem).AutoFilter(filter).ToList();

            //assert
            Assert.Equal(1, filtered.Count);
            Assert.Equal("Nested", filtered[0].NestedItem.String);
        }
    }
}
