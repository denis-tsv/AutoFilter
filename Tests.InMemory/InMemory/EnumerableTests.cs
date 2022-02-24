using System.Collections.Generic;
using System.Linq;
using AutoFilter;
using Tests.Data;
using Tests.Models;
using Xunit;

namespace Tests.InMemory
{
    public class EnumerableTests
    {
        [Fact]
        public void Enum()
        {
            //arrange
            var filter = new EnumerableFilter { Enum = new List<TargetEnum> {
                TargetEnum.First
            }};

            //act
            var filtered = EnumerableTestsData.Items.AutoFilter(filter).ToList();

            //assert
            Assert.Equal(1, filtered.Count);
            Assert.Equal(TargetEnum.First, filtered[0].Enum);
        }

        [Fact]
        public void NullableEnum()
        {
            //arrange
            var filter = new EnumerableFilter
            {
                NullableEnum = new List<TargetEnum> {
                    TargetEnum.First
                }
            };

            //act
            var filtered = EnumerableTestsData.Items.AutoFilter(filter).ToList();
            
            //assert
            Assert.Equal(1, filtered.Count);
            Assert.Equal(TargetEnum.First, filtered[0].NullableEnum);
        }

        [Fact]
        public void Int()
        {
            //arrange
            var filter = new EnumerableFilter
            {
                Int = new int[]{ 1 }
            };

            //act
            var filtered = EnumerableTestsData.Items.AutoFilter(filter).ToList();

            //assert
            Assert.Equal(1, filtered.Count);
            Assert.Equal(1, filtered[0].Int);
        }

        [Fact]
        public void NullableInt()
        {
            //arrange
            var filter = new EnumerableFilter
            {
                NullableInt = new []{ 1 }
            };

            //act
            var filtered = EnumerableTestsData.Items.AutoFilter(filter).ToList();

            //assert
            Assert.Equal(1, filtered.Count);
            Assert.Equal(1, filtered[0].NullableInt);
        }

        [Fact]
        public void String()
        {
            //arrange
            var filter = new EnumerableFilter
            {
                String = new List<string> { "S1", "S2" }
            };

            //act
            var filtered = EnumerableTestsData.Items.AutoFilter(filter).ToList();

            //assert
            Assert.Equal(2, filtered.Count);
        }
    }
}
