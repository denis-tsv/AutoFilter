
using System;
using System.Linq;
using AutoFilter;
using Tests.Data;
using Tests.Models;
using Xunit;

namespace Tests.InMemory
{
    public class RangeTests
    {
        [Fact]
        public void IntFrom()
        {
            //arrange
            var filter = new RangeFilter{ IntValue = new Range<int> { From = 15}};

            //act
            var filtered = RangeData.Items.AutoFilter(filter);

            //assert
             Assert.Equal(2, filtered.Count());
        }

        [Fact]
        public void IntTo()
        {
            //arrange
            var filter = new RangeFilter { IntValue = new Range<int> { To = 15 } };

            //act
            var filtered = RangeData.Items.AutoFilter(filter);

            //assert
            Assert.Equal(1, filtered.Count());
        }

        [Fact]
        public void IntFromTo()
        {
            //arrange
            var filter = new RangeFilter { IntValue = new Range<int> { From = 10, To = 15 } };

            //act
            var filtered = RangeData.Items.AutoFilter(filter);

            //assert
            Assert.Equal(1, filtered.Count());
        }

        [Fact]
        public void DateTimeNestedFrom()
        {
            //arrange
            var filter = new RangeFilter { DateTimeValue = new Range<DateTime> { From = new DateTime(2021, 01, 01) } };

            //act
            var filtered = RangeData.Items.AutoFilter(filter);

            //assert
            Assert.Equal(2, filtered.Count());
        }

        [Fact]
        public void DateTimeNestedTo()
        {
            //arrange
            var filter = new RangeFilter { DateTimeValue = new Range<DateTime> { To = new DateTime(2021, 01, 01) } };

            //act
            var filtered = RangeData.Items.AutoFilter(filter);

            //assert
            Assert.Equal(2, filtered.Count());
        }

        [Fact]
        public void DateTimeNestedFromTo()
        {
            //arrange
            var filter = new RangeFilter { DateTimeValue = new Range<DateTime>
            {
                From = new DateTime(2021, 02, 01),
                To = new DateTime(2022, 01, 01)
            } };

            //act
            var filtered = RangeData.Items.AutoFilter(filter);

            //assert
            Assert.Equal(1, filtered.Count());
        }
    }
}
