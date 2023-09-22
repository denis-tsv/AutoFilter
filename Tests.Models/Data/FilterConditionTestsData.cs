using Tests.Models;
using System;
using System.Collections.Generic;

namespace Tests.Data
{
    public class FilterConditionTestsData
    {
        public static List<FilterConditionItem> Items = new List<FilterConditionItem>
        {
            //BoolEqual
            new FilterConditionItem
            {

            },
            new FilterConditionItem
            {
                BoolEqual = true
            },
            new FilterConditionItem
            {
                BoolEqual = false
            },

            //IntGreaterOrEqual
            new FilterConditionItem
            {
                IntGreaterOrEqual = 1
            },
            new FilterConditionItem
            {
                IntGreaterOrEqual = 2
            },
            new FilterConditionItem
            {
                IntGreaterOrEqual = -1
            },

            //DateTimeLessOrEqual
            new FilterConditionItem
            {
                DateTimeLessOrEqual = new DateTime(2010, 10, 23, 14, 56, 54).ToUniversalTime()
            },
            new FilterConditionItem
            {
                DateTimeLessOrEqual = new DateTime(2015, 10, 10).ToUniversalTime()
            },
            new FilterConditionItem
            {
                DateTimeLessOrEqual = new DateTime(2010, 10, 23).ToUniversalTime()
            },

            //DecimalLess
            new FilterConditionItem
            {
                DecimalLess = 1000
            },
            new FilterConditionItem
            {
                DecimalLess = -1000
            },
            new FilterConditionItem
            {
                DecimalLess = -1
            },

            //DoubleGreater
            new FilterConditionItem
            {
                DoubleGreater = 1000
            },
            new FilterConditionItem
            {
                DoubleGreater = -1000
            },
            new FilterConditionItem
            {
                DoubleGreater = 1
            },
        };

        
    }
}
