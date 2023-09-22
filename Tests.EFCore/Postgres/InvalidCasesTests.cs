using Tests.Models;
using Xunit;
using AutoFilter;
using System.Linq;
using System;

namespace Tests.EF
{
    public class InvalidCasesTests 
    {
        [Fact]
        public void NotExistsProperty()
        {
            //arrange
            var context = Shared.GetDbContext();
            var filter = new InvalidCaseFilter { NotExistsProperty = "First" };

            //act
            var result = context.InvalidCaseItems.AutoFilter(filter).ToList();

            //asssert
            Assert.Equal(result.Count, context.InvalidCaseItems.Count());
        }
    }
}
