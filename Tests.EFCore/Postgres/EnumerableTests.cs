using System.Collections.Generic;
using System.Linq;
using AutoFilter;
using Tests.Models;
using Xunit;

namespace Tests.EF
{
    public class EnumerableTests 
    {
        [Fact]
        public void Enum()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new EnumerableFilter
            {
                Enum = new List<TargetEnum> {
                    TargetEnum.First,
                    TargetEnum.Default
                }
            };

            //act
            var filtered = context.EnumerableFilterItems.AutoFilter(filter).ToList();

            //assert
            Assert.Equal(filtered.Count, context.EnumerableFilterItems.Count());
        }

        [Fact]
        public void NullableEnum()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new EnumerableFilter
            {
                NullableEnum = new List<TargetEnum> {
                    TargetEnum.First,
                    TargetEnum.Default
                }
            };

            //act
            var filtered = context.EnumerableFilterItems.AutoFilter(filter).ToList();

            //assert
            Assert.Equal(2, filtered.Count);
        }

        [Fact]
        public void Int()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new EnumerableFilter
            {
                Int = new int[] { 1, 2 }
            };

            //act
            var filtered = context.EnumerableFilterItems.AutoFilter(filter).ToList();

            //assert
            Assert.Equal(1, filtered.Count);
            Assert.Equal(1, filtered[0].Int);
        }

        [Fact]
        public void NullableInt()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new EnumerableFilter
            {
                NullableInt = new[] { 1, 2 }
            };

            //act
            var filtered = context.EnumerableFilterItems.AutoFilter(filter).ToList();

            //assert
            Assert.Equal(1, filtered.Count);
            Assert.Equal(1, filtered[0].NullableInt);

            //if single item in collection WHERE NullableInt = 1
            //if many items in collection WHERE NullableInt in (1, 2)
        }

        [Fact]
        public void String()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new EnumerableFilter
            {
                String = new List<string>{ "S1", "S2" }
            };

            //act
            var filtered = context.EnumerableFilterItems.AutoFilter(filter).ToList();

            //assert
            Assert.Equal(2, filtered.Count);
        }
    }
}
