using AutoFilter.Extensions;
using Tests.Models;
#if EF_CORE
using Microsoft.EntityFrameworkCore;
#elif EF6
using System.Data.Entity;
#endif
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Tests.EF
{
    public class KillerFeatureTests : TestBase
    {
        [Fact]
        public void ScalarCombination()
        {
            //arrange
            Init();
            Expression<Func<NestedItem, bool>> ex = (x) => x.Int > 1;
            
            // act
            var filtered = Context.NavigationPropertyItems.Include(x => x.NestedItem).Where(x => x.NestedItem, ex).ToList();

            //assert
            Assert.Equal(1, filtered.Count);
        }

        [Fact]
        public void CollectionCombination()
        {
            //arrange
            Init();
            Expression<Func<NavigationPropertyItem, bool>> ex = (x) => x.Id > 3;

            // act
            var filtered = Context.NestedItems.Include(x => x.NavigationPropertyItems).WhereAny(x => x.NavigationPropertyItems, ex).ToList();

            //assert
            Assert.Equal(6, filtered.Count);
        }
    }
}
