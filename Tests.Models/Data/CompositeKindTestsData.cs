using Tests.Models;
using System.Collections.Generic;

namespace Tests.Data
{
    public class CompositeKindTestsData
    {
        public static List<CompositeKindItem> Items = new List<CompositeKindItem>
        {
            new CompositeKindItem
            {
                Int1 = 1,
            },
            new CompositeKindItem
            {
                Int2 = 1,
            },
            new CompositeKindItem
            {
                Int1 = 1,
                Int2 = 1,
            },
        };
        
    }
}
