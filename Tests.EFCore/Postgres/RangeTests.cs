using System;
using System.Linq;
using AutoFilter;
using Tests.Models;
using Xunit;

namespace Tests.EF
{
    public class RangeTests 
    {
        [Fact]
        public void IntFrom()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new RangeFilter { IntValue = new Range<int> { From = 15 } };

            //act
            var filtered = context.RangeFilterItems.AutoFilter(filter);

            //assert
            Assert.Equal(2, filtered.Count());
        }

        [Fact]
        public void IntTo()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new RangeFilter { IntValue = new Range<int> { To = 15 } };

            //act
            var filtered = context.RangeFilterItems.AutoFilter(filter);

            //assert
            Assert.Equal(1, filtered.Count());
        }

        [Fact]
        public void IntFromTo()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new RangeFilter { IntValue = new Range<int> { From = 10, To = 15 } };

            //act
            var filtered = context.RangeFilterItems.AutoFilter(filter);

            //assert
            Assert.Equal(1, filtered.Count());
        }

        [Fact]
        public void DateTimeNestedFrom()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new RangeFilter { DateTimeValue = new Range<DateTime> { From = new DateTime(2021, 01, 01).ToUniversalTime() } };

            //act
            var filtered = context.RangeFilterItems.AutoFilter(filter);

            //assert
            Assert.Equal(2, filtered.Count());
        }

        [Fact]
        public void DateTimeNestedTo()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new RangeFilter { DateTimeValue = new Range<DateTime> { To = new DateTime(2021, 01, 01).ToUniversalTime() } };

            //act
            var filtered = context.RangeFilterItems.AutoFilter(filter);

            //assert
            Assert.Equal(2, filtered.Count());
        }

        [Fact]
        public void DateTimeNestedFromTo()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new RangeFilter
            {
                DateTimeValue = new Range<DateTime>
                {
                    From = new DateTime(2021, 02, 01).ToUniversalTime(),
                    To = new DateTime(2022, 01, 01).ToUniversalTime()
                }
            };

            //act
            var filtered = context.RangeFilterItems.AutoFilter(filter);

            //assert
            Assert.Equal(1, filtered.Count());
        }
    }
}
