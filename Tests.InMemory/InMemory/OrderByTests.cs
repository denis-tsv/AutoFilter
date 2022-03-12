using Tests.Data;
using Xunit;
using AutoFilter;
using System.Linq;
using Tests.Models;

namespace Tests.InMemory
{
    public class OrderByTests
    {
        [Fact]
        public void OrderBy()
        {
            //arrange
            var propertyName = nameof(StringTestItem.NoAttribute).ToLower();

            //act
            var orderedAuto = StringTestsData.Items
                .OrderBy(propertyName).ToList();

            var orderedManual = StringTestsData.Items
                .OrderBy(x => x.NoAttribute).ToList();

            //assert
            Assert.Equal(orderedAuto.Count, orderedManual.Count);
            for (int i = 0; i < orderedAuto.Count; i++)
                Assert.Equal(orderedAuto[i].NoAttribute, orderedManual[i].NoAttribute);
        }

        [Fact]
        public void OrderByDescending()
        {
            //arrange
            var propertyName = nameof(FilterConditionItem.IntGreaterOrEqual);

            //act
            var orderedAuto = FilterConditionTestsData.Items
                .OrderByDescending(propertyName).ToList();

            var orderedManual = FilterConditionTestsData.Items
                .OrderByDescending(x => x.IntGreaterOrEqual).ToList();

            //assert
            Assert.Equal(orderedAuto.Count, orderedManual.Count);
            for (int i = 0; i < orderedAuto.Count; i++)
                Assert.Equal(orderedAuto[i].IntGreaterOrEqual, orderedManual[i].IntGreaterOrEqual);
        }
    }
}
