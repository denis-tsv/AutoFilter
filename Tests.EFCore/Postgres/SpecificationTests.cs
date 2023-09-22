using AutoFilter;
using Tests.Models;
using System.Linq;
using Xunit;

namespace Tests.EF
{
    public class SpecificationTests
    {
        private Spec<FilterConditionItem> _intGreatOrEqual1 = new Spec<FilterConditionItem>(x => x.IntGreaterOrEqual >= 1);
        
        [Fact]
        public void AndTest()
        {
            //arrange
            var boolIsEmpty = new Spec<FilterConditionItem>(x => !x.BoolEqual.HasValue);
            var context = Shared.GetDbContext();
            
            //act
            var filtered = context.FilterConditionItems.Where(_intGreatOrEqual1 && boolIsEmpty).ToList();

            //assert
            Assert.Equal(2, filtered.Count);
        }

        [Fact]
        public void OrTest()
        {
            //arrange 
            var _decimalless0 = new Spec<FilterConditionItem>(y => y.DecimalLess < 0);
            var context = Shared.GetDbContext();

            //act
            var filtered = context.FilterConditionItems.Where(_intGreatOrEqual1 || _decimalless0).ToList();

            //assert
            Assert.Equal(4, filtered.Count);
        }

        [Fact]
        public void NotTest()
        {
            //arrange 
            var context = Shared.GetDbContext();
            var _decimalless1 = new Spec<FilterConditionItem>(x => x.DecimalLess < 1);

            //act
            var filtered = context.FilterConditionItems.Where(!_decimalless1).ToList();

            //assert
            Assert.Equal(1, filtered.Count);
        }
    }
}
