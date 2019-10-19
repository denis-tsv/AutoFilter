using Tests.Models;
using Xunit;
using AutoFilter;
using System.Linq;
using System;
using Tests.Data;

namespace Tests.InMemory
{
    public class InvalidCasesTests
    {
        [Fact]
        public void WithTargetType()
        {
            //arrange
            var filter = new InvalidCaseFilter { EnumTargetType = "Default" };

            //act
            var filtered = InvalidCasesTestsData.Items.AutoFilter(filter).ToList();

            //asssert
            Assert.Equal(1, filtered.Count);
            Assert.Equal(TargetEnum.Default, filtered[0].TargetEnum);
        }

        [Fact]
        public void NotExistsValue()
        {
            //arrange
            var filter = new InvalidCaseFilter { NotExistsValue = "WrongEnumName" };

            //act
            
            //asssert
            Assert.ThrowsAny<Exception>(() => InvalidCasesTestsData.Items.AutoFilter(filter));
        }

        [Fact]
        public void NotExistsProperty()
        {
            //arrange
            var filter = new InvalidCaseFilter { NotExistsProperty = "First" };

            //act
            
            //asssert
            Assert.ThrowsAny<Exception>(() => InvalidCasesTestsData.Items.AutoFilter(filter));
        }
    }
}
