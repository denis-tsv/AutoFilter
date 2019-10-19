using AutoFilter;
using Tests.Models;
using System.Linq;
using Xunit;
using Tests.Data;

namespace Tests.InMemory
{
    public class StringTests
    {
        [Fact]
        public void NoAttributes()
        {
            //arrange
            var filter = new StringFilter { NoAttribute = "NoAttribute" };
            
            //act
            var filtered = StringTestsData.Items.AutoFilter(filter);

            //assert
            Assert.Single(filtered);
            Assert.Equal("NoAttributeOk", filtered.Single().NoAttribute);
        }

        [Fact]
        public void ContainsCase()
        {
            //arrange
            var filter = new StringFilter { ContainsCase = "ContainsCase" };

            //act
            var filtered = StringTestsData.Items.AutoFilter(filter);

            //assert
            Assert.Single(filtered);
            Assert.Equal("TestContainsCase", filtered.Single().ContainsCase);
        }

        [Fact]
        public void ContainsIgnoreCase()
        {
            //arrange
            var filter = new StringFilter { ContainsIgnoreCase = "ContainsIgnoreCase" };

            //act
            var filtered = StringTestsData.Items.AutoFilter(filter)
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
            var filter = new StringFilter { StartsWithCase = "StartsWithCase" };

            //act
            var filtered = StringTestsData.Items.AutoFilter(filter).ToList();

            //assert
            Assert.Equal(1, filtered.Count);
            Assert.Equal("StartsWithCase", filtered[0].StartsWithCase);            
        }

        [Fact]
        public void StartsWithIgnoreCase()
        {
            //arrange
            var filter = new StringFilter { StartsWithIgnoreCase = "StartsWithIgnoreCase" };

            //act
            var filtered = StringTestsData.Items.AutoFilter(filter)
                .OrderBy(x => x.StartsWithIgnoreCase)
                .ToList();

            //assert
            Assert.Equal(2, filtered.Count);
            Assert.Equal("startswithignorecasetest", filtered[0].StartsWithIgnoreCase);
            Assert.Equal("StartsWithIgnoreCaseTest", filtered[1].StartsWithIgnoreCase);
            
        }

        [Fact]
        public void PropertyName()
        {
            //arrange
            var filter = new StringFilter { TargetStringProperty = "PropertyName" };

            //act
            var filtered = StringTestsData.Items.AutoFilter(filter).ToList();

            //assert
            Assert.Equal(1, filtered.Count);
            Assert.Equal("PropertyName", filtered[0].PropertyName);            
        }

        [Fact]
        public void TargetStringPropertyContainsIgnoreCase()
        {
            //arrange
            var filter = new StringFilter { TargetStringPropertyContainsIgnoreCase = "ropertyname" };

            //act
            var filtered = StringTestsData.Items.AutoFilter(filter).ToList();

            //assert
            Assert.Equal(1, filtered.Count);
            Assert.Equal("PropertyName", filtered[0].PropertyName);
        }
    }
}
