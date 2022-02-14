using System;
using System.Linq;
using AutoFilter;
using Tests.Models;
using Xunit;

namespace Tests.EF
{
    public class RangeTests : TestBase
    {
        [Fact]
        public void IntFrom()
        {
            //arrange
            Init();
            var filter = new RangeFilter { IntValue = new Range<int> { From = 15 } };

            //act
            var filtered = Context.RangeFilterItems.AutoFilter(filter);

            //assert
            Assert.Equal(2, filtered.Count());
        }

        [Fact]
        public void IntTo()
        {
            //arrange
            Init();
            var filter = new RangeFilter { IntValue = new Range<int> { To = 15 } };

            //act
            var filtered = Context.RangeFilterItems.AutoFilter(filter);

            //assert
            Assert.Equal(1, filtered.Count());
        }

        [Fact]
        public void IntFromTo()
        {
            //arrange
            Init();
            var filter = new RangeFilter { IntValue = new Range<int> { From = 10, To = 15 } };

            //act
            var filtered = Context.RangeFilterItems.AutoFilter(filter);

            //assert
            Assert.Equal(1, filtered.Count());
        }

        [Fact]
        public void DateTimeNestedFrom()
        {
            //arrange
            Init();
            var filter = new RangeFilter { DateTimeValue = new Range<DateTime> { From = new DateTime(2021, 01, 01) } };

            //act
            var filtered = Context.RangeFilterItems.AutoFilter(filter);

            //assert
            Assert.Equal(2, filtered.Count());
        }

        [Fact]
        public void DateTimeNestedTo()
        {
            //arrange
            Init();
            var filter = new RangeFilter { DateTimeValue = new Range<DateTime> { To = new DateTime(2021, 01, 01) } };

            //act
            var filtered = Context.RangeFilterItems.AutoFilter(filter);

            //assert
            Assert.Equal(2, filtered.Count());
        }

        [Fact]
        public void DateTimeNestedFromTo()
        {
            //arrange
            Init();
            var filter = new RangeFilter
            {
                DateTimeValue = new Range<DateTime>
                {
                    From = new DateTime(2021, 02, 01),
                    To = new DateTime(2022, 01, 01)
                }
            };

            //act
            var filtered = Context.RangeFilterItems.AutoFilter(filter);

            //assert
            Assert.Equal(1, filtered.Count());
        }
    }
}
