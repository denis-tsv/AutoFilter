﻿using Tests.Models;
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
        public void NotExistsProperty()
        {
            //arrange
            var filter = new InvalidCaseFilter { NotExistsProperty = "First" };

            //act
            var result = InvalidCasesTestsData.Items.AutoFilter(filter).ToList();

            //assert
            Assert.Equal(InvalidCasesTestsData.Items.Count, result.Count);
        }
    }
}
