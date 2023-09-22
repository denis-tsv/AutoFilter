using AutoFilter;
using Tests.Models;
using System.Linq;
using Xunit;

namespace Tests.EF
{
    public class StringTests
    {
        [Fact]
        public void NoAttributes()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new StringFilter { NoAttribute = "NoAttribute" };
            
            //act
            var filtered = context.StringTestItems
                .AutoFilter(filter)
                .OrderBy(x => x.NoAttribute)
                .ToList();

            //assert
            Assert.Equal(1, filtered.Count); 
            Assert.Equal("NoAttributeOk", filtered[0].NoAttribute);
        }

        [Fact]
        public void ContainsCase()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new StringFilter { ContainsCase = "ContainsCase" };

            //act
            var filtered = context.StringTestItems
                .AutoFilter(filter)
                .OrderBy(x => x.ContainsCase)
                .ToList();

            //assert
            Assert.Equal(1, filtered.Count);
            Assert.Equal("TestContainsCase", filtered[0].ContainsCase);
        }

        [Fact]
        public void ContainsIgnoreCase()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new StringFilter { ContainsIgnoreCase = "ContainsIgnoreCase" };

            //act
            var filtered = context.StringTestItems
                .AutoFilter(filter)
                .OrderBy(x => x.ContainsIgnoreCase)
                .ToList();

            //assert
            Assert.Equal(2, filtered.Count);
            Assert.Equal("testcontainsignorecase", filtered[0].ContainsIgnoreCase);
            Assert.Equal("TestContainsIgnoreCase", filtered[1].ContainsIgnoreCase);
        }

        [Fact]
        public void StartsWithCase()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new StringFilter { StartsWithCase = "StartsWithCase" };

            //act
            var filtered = context.StringTestItems
                .AutoFilter(filter)
                .OrderBy(x => x.StartsWithCase)
                .ToList();

            //assert
            Assert.Equal(1, filtered.Count);

            Assert.Equal("StartsWithCase", filtered[0].StartsWithCase);
        }

        [Fact]
        public void StartsWithIgnoreCase()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new StringFilter { StartsWithIgnoreCase = "StartsWithIgnoreCase" };

            //act
            var filtered = context.StringTestItems
                .AutoFilter(filter)
                .OrderBy(x => x.StartsWithIgnoreCase)
                .ToList();

            //assert
            Assert.Equal(2, filtered.Count);

            Assert.Equal("startswithignorecasetest", filtered[0].StartsWithIgnoreCase);
            Assert.Equal("StartsWithIgnoreCaseTest", filtered[1].StartsWithIgnoreCase);
        }

        [Fact]
        public void EndsWithCase()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new StringFilter { EndsWithCase = "EndsWithCase" };

            //act
            var filtered = context.StringTestItems
                .AutoFilter(filter)
                .OrderBy(x => x.EndsWithCase)
                .ToList();

            //assert
            Assert.Equal(1, filtered.Count);

            Assert.Equal("EndsWithCase", filtered[0].EndsWithCase);
        }

        [Fact]
        public void EndsWithIgnoreCase()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new StringFilter { EndsWithIgnoreCase = "EndsWithIgnoreCase" };

            //act
            var filtered = context.StringTestItems
                .AutoFilter(filter)
                .OrderBy(x => x.EndsWithIgnoreCase)
                .ToList();

            //assert
            Assert.Equal(2, filtered.Count);

            Assert.Equal("EndsWithIgnoreCase", filtered[1].EndsWithIgnoreCase);
            Assert.Equal("endswithignorecase", filtered[0].EndsWithIgnoreCase);
        }

        [Fact]
        public void EqualsCase()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new StringFilter { EqualsCase = "EqualsCase" };

            //act
            var filtered = context.StringTestItems
                .AutoFilter(filter)
                .OrderBy(x => x.EqualsCase)
                .ToList();

            //assert
            Assert.Equal(1, filtered.Count); //MS SQL string comparison not case sensitive, but postgres is case sensitive
            Assert.Equal("EqualsCase", filtered[0].EqualsCase);
        }

        [Fact]
        public void EqualsIgnoreCase()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new StringFilter { EqualsIgnoreCase = "EqualsIgnoreCase" };

            //act
            var filtered = context.StringTestItems
                .AutoFilter(filter)
                .OrderBy(x => x.EqualsIgnoreCase)
                .ToList();

            //assert
            Assert.Equal(2, filtered.Count);
            Assert.Equal("equalsignorecase", filtered[0].EqualsIgnoreCase);
            Assert.Equal("EqualsIgnoreCase", filtered[1].EqualsIgnoreCase);
        }

        [Fact]
        public void PropertyName()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new StringFilter { TargetStringProperty = "PropertyName" };

            //act
            var filtered = context.StringTestItems.AutoFilter(filter).ToList();

            //assert
            Assert.Equal(1, filtered.Count);
            Assert.Equal("PropertyName", filtered[0].PropertyName);            
        }

        [Fact]
        public void TargetStringPropertyContainsIgnoreCase()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new StringFilter { TargetStringPropertyContainsIgnoreCase = "ropertyname" };

            //act
            var filtered = context.StringTestItems.AutoFilter(filter).ToList();

            //assert
            Assert.Equal(1, filtered.Count);
            Assert.Equal("PropertyName", filtered[0].PropertyName);
        }

    }
}
