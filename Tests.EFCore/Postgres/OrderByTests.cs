using Xunit;
using AutoFilter;
using System.Linq;

namespace Tests.EF
{
    public class OrderByTests
    {
        [Fact]
        public void OrderBy()
        {
            //arrange
            var context = Shared.GetDbContext();
            var propertyName = "NoAttribute";

            //act
            var orderedAuto = context.StringTestItems
                .OrderBy(propertyName).ToList();

            var orderedManual = context.StringTestItems
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
            var context = Shared.GetDbContext();
            var propertyName = "IntGreaterOrEqual";

            //act
            var orderedAuto = context.FilterConditionItems
                .OrderByDescending(propertyName).ToList();

            var orderedManual = context.FilterConditionItems
                .OrderByDescending(x => x.IntGreaterOrEqual).ToList();

            //assert
            Assert.Equal(orderedAuto.Count, orderedManual.Count);
            for (int i = 0; i < orderedAuto.Count; i++)
                Assert.Equal(orderedAuto[i].IntGreaterOrEqual, orderedManual[i].IntGreaterOrEqual);
        }
    }
}
