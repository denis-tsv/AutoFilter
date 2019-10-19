using Tests.Models;
using Xunit;
using AutoFilter;
using System.Linq;
using System;

namespace Tests.EF
{
    public class InvalidCasesTests : TestBase
    {
        [Fact]
        public void NotExistsValue()
        {
            //arrange
            Init();
            var filter = new InvalidCaseFilter { NotExistsValue = "WrongEnumName" };

            //act
            
            //asssert
            Assert.ThrowsAny<Exception>(() => Context.InvalidCaseItems.AutoFilter(filter).ToList());            
        }

        [Fact]
        public void NotExistsProperty()
        {
            //arrange
            Init();
            var filter = new InvalidCaseFilter { NotExistsProperty = "First" };

            //act
            
            //asssert
            Assert.ThrowsAny<Exception>(() => Context.InvalidCaseItems.AutoFilter(filter).ToList());
        }
    }
}
