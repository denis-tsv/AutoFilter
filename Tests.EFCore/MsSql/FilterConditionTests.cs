using AutoFilter;
using Tests.Models;
using System;
using System.Linq;
using Xunit;

namespace Tests.EF
{
    public class FilterConditionTests : TestBase
    {
        [Fact]
        public void BoolEqual()
        {
            //arrange
            Init();
            var filter = new FilterConditionFilter { BoolEqual = true };

            //act
            var filtered = Context.FilterConditionItems.AutoFilter(filter).ToList();

            //assert
            Assert.Equal(1, filtered.Count);
            Assert.Equal(true, filtered[0].BoolEqual);
        }

        [Fact] 
        public void IntGreaterOrEqual()
        {
            //arrange
            Init();
            var filter = new FilterConditionFilter { IntGreaterOrEqual = 1 };

            //act
            var filtered = Context.FilterConditionItems
                .AutoFilter(filter)
                .OrderBy(x => x.IntGreaterOrEqual)
                .ToList();

            //assert
            Assert.Equal(2, filtered.Count);
            Assert.Equal(1, filtered[0].IntGreaterOrEqual);
            Assert.Equal(2, filtered[1].IntGreaterOrEqual);
        }

        [Fact] 
        public void DateTimeLessOrEqual()
        {
            //arrange
            Init();
            var filter = new FilterConditionFilter { DateTimeLessOrEqual = new DateTime(2010, 10, 23, 14, 56, 54) };

            //act
            var filtered = Context.FilterConditionItems
                .AutoFilter(filter)
                .OrderBy(x => x.DateTimeLessOrEqual)
                .ToList();

            //assert
            Assert.Equal(2, filtered.Count);
            Assert.Equal(new DateTime(2010, 10, 23), filtered[0].DateTimeLessOrEqual);
            Assert.Equal(new DateTime(2010, 10, 23, 14, 56, 54), filtered[1].DateTimeLessOrEqual);
        }

        [Fact]
        public void DecimalLess()
        {
            //arrange
            Init();
            var filter = new FilterConditionFilter { DecimalLess = -1 };

            //act
            var filtered = Context.FilterConditionItems.AutoFilter(filter).ToList();

            //assert
            Assert.Equal(1, filtered.Count);
            Assert.Equal(-1000, filtered[0].DecimalLess);            
        }
        

        [Fact]
        public void DoubleGreater()
        {
            //arrange
            Init();
            var filter = new FilterConditionFilter { DoubleGreater = 1 };

            //act
            var filtered = Context.FilterConditionItems.AutoFilter(filter).ToList();

            //assert
            Assert.Equal(1, filtered.Count);
            Assert.Equal(1000, filtered[0].DoubleGreater);
        }
    }
}
