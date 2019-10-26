using Xunit;
using AutoFilter;
using System.Linq;

namespace Tests.EF
{
    public class OrderByTests : TestBase
    {
        [Fact]
        public void OrderBy()
        {
            //arrange
            Init();
            var propertyName = "NoAttribute";

            //act
            var orderedAuto = Context.StringTestItems
                .OrderBy(propertyName).ToList();

            var orderedManual = Context.StringTestItems
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
            Init();
            var propertyName = "IntGreaterOrEqual";

            //act
            var orderedAuto = Context.FilterConditionItems
                .OrderByDescending(propertyName).ToList();

            var orderedManual = Context.FilterConditionItems
                .OrderByDescending(x => x.IntGreaterOrEqual).ToList();

            //assert
            Assert.Equal(orderedAuto.Count, orderedManual.Count);
            for (int i = 0; i < orderedAuto.Count; i++)
                Assert.Equal(orderedAuto[i].IntGreaterOrEqual, orderedManual[i].IntGreaterOrEqual);
        }
    }
}
