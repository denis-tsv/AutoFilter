using AutoFilter;
using Tests.Models;
using System.Linq;
using Xunit;

namespace Tests.EF
{
    public class StringTests : TestBase
    {
        [Fact]
        public void NoAttributes()
        {
            //arrange
            Init();
            var filter = new StringFilter { NoAttribute = "NoAttribute" };
            
            //act
            var filtered = Context.StringTestItems
                .AutoFilter(filter)
                .OrderBy(x => x.NoAttribute)
                .ToList();

            //assert
            Assert.Equal(2, filtered.Count); 
            Assert.Equal("noattribute", filtered[0].NoAttribute); 
            Assert.Equal("NoAttributeOk", filtered[1].NoAttribute);

            /* EF Core
             SELECT [Id]
      ,[NoAttribute]
	  ,LEFT(s.[NoAttribute], (LEN(N'NoAttribute')))  as L
  FROM [dbo].[StringTestItems] s
  where [NoAttribute] LIKE 'NoAttribute' + '%' AND (LEFT(s.[NoAttribute], (LEN(N'NoAttribute'))) = N'NoAttribute')

             */

            /* EF 6
             WHERE NoAttribute LIKE N'NoAttribute%'
             */
        }

        [Fact]
        public void ContainsCase()
        {
            //arrange
            Init();
            var filter = new StringFilter { ContainsCase = "ContainsCase" };

            //act
            var filtered = Context.StringTestItems
                .AutoFilter(filter)
                .OrderBy(x => x.ContainsCase)
                .ToList();

            //assert
            Assert.Equal(2, filtered.Count);
            Assert.Equal("containscase", filtered[0].ContainsCase);
            Assert.Equal("TestContainsCase", filtered[1].ContainsCase);

            /* EF Core
             SELECT [Id], ContainsCase	  
              FROM [dbo].[StringTestItems] s
              where charindex('ContainsCase', s.ContainsCase) > 0
             */

            /* EF 6
             WHERE ContainsCase LIKE '%ContainsCase%'
             */
        }

        [Fact]
        public void ContainsIgnoreCase()
        {
            //arrange
            Init();
            var filter = new StringFilter { ContainsIgnoreCase = "ContainsIgnoreCase" };

            //act
            var filtered = Context.StringTestItems
                .AutoFilter(filter)
                .OrderBy(x => x.ContainsIgnoreCase)
                .ToList();

            //assert
            Assert.Equal(2, filtered.Count);
            Assert.Equal("TestContainsIgnoreCase", filtered[0].ContainsIgnoreCase);
            Assert.Equal("testcontainsignorecase", filtered[1].ContainsIgnoreCase);

            /* EF 6
               WHERE ( CAST(CHARINDEX(LOWER(N'ContainsIgnoreCase'), LOWER([Extent1].[ContainsIgnoreCase])) AS int)) > 0
             */
        }

        [Fact]
        public void StartsWithCase()
        {
            //arrange
            Init();
            var filter = new StringFilter { StartsWithCase = "StartsWithCase" };

            //act
            var filtered = Context.StringTestItems
                .AutoFilter(filter)
                .OrderBy(x => x.StartsWithCase)
                .ToList();

            //assert
            Assert.Equal(2, filtered.Count);

            Assert.Equal("StartsWithCase", filtered[0].StartsWithCase);
            Assert.Equal("startswithcase", filtered[1].StartsWithCase);
        }

        [Fact]
        public void StartsWithIgnoreCase()
        {
            //arrange
            Init();
            var filter = new StringFilter { StartsWithIgnoreCase = "StartsWithIgnoreCase" };

            //act
            var filtered = Context.StringTestItems
                .AutoFilter(filter)
                .OrderBy(x => x.StartsWithIgnoreCase)
                .ToList();

            //assert
            Assert.Equal(2, filtered.Count);

            Assert.Equal("StartsWithIgnoreCaseTest", filtered[0].StartsWithIgnoreCase);
            Assert.Equal("startswithignorecasetest", filtered[1].StartsWithIgnoreCase);

            /* EF 6
            WHERE ( CAST(CHARINDEX(LOWER(N'StartsWithIgnoreCase'), LOWER([Extent1].[StartsWithIgnoreCase])) AS int)) = 1
            */

        }

        [Fact]
        public void EqualsCase()
        {
            //arrange
            Init();
            var filter = new StringFilter { EqualsCase = "EqualsCase" };

            //act
            var filtered = Context.StringTestItems
                .AutoFilter(filter)
                .OrderBy(x => x.EqualsCase)
                .ToList();

            //assert
            Assert.Equal(2, filtered.Count);
            Assert.Equal("EqualsCase", filtered[0].EqualsCase);
            Assert.Equal("equalscase", filtered[1].EqualsCase); //MS SQL string comparison not case sensitive, but postgres is case sensitive
        }

        [Fact]
        public void EqualsIgnoreCase()
        {
            //arrange
            Init();
            var filter = new StringFilter { EqualsIgnoreCase = "EqualsIgnoreCase" };

            //act
            var filtered = Context.StringTestItems
                .AutoFilter(filter)
                .OrderBy(x => x.EqualsIgnoreCase)
                .ToList();

            //assert
            Assert.Equal(2, filtered.Count);
            Assert.Equal("EqualsIgnoreCase", filtered[0].EqualsIgnoreCase);
            Assert.Equal("equalsignorecase", filtered[1].EqualsIgnoreCase);
        }

        [Fact]
        public void PropertyName()
        {
            //arrange
            Init();
            var filter = new StringFilter { TargetStringProperty = "PropertyName" };

            //act
            var filtered = Context.StringTestItems.AutoFilter(filter).ToList();

            //assert
            Assert.Equal(1, filtered.Count);
            Assert.Equal("PropertyName", filtered[0].PropertyName);            
        }

        [Fact]
        public void TargetStringPropertyContainsIgnoreCase()
        {
            //arrange
            Init();
            var filter = new StringFilter { TargetStringPropertyContainsIgnoreCase = "ropertyname" };

            //act
            var filtered = Context.StringTestItems.AutoFilter(filter).ToList();

            //assert
            Assert.Equal(1, filtered.Count);
            Assert.Equal("PropertyName", filtered[0].PropertyName);
        }

    }
}
