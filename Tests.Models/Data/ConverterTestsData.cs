using Tests.Models;
using System.Collections.Generic;

namespace Tests.Data
{
    public class ConverterTestsData
    {
        public static List<ConvertItem> Items = new List<ConvertItem>
        {
            new ConvertItem
            {

            },

            new ConvertItem
            {
                WithConverter = true,
            },
            new ConvertItem
            {
                WithConverter = false,
            },

            new ConvertItem
            {
                WithoutConverter = true,
            },
            new ConvertItem
            {
                WithoutConverter = false,
            },
        };        

    }
}
