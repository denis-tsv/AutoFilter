using System.Collections.Generic;
using Tests.Models;

namespace Tests.Data
{
    public class NotAutoFilteredData
    {
        public static List<NotAutoFilteredItem> Items = new List<NotAutoFilteredItem>
        {
            new NotAutoFilteredItem
            {
                NotFiltered = 1,
            },
            new NotAutoFilteredItem
            {
                NotFiltered = 2,
            },
            new NotAutoFilteredItem
            {
                Filtered = 1,
            },
            new NotAutoFilteredItem
            {
                Filtered = 2
            }
        };
    }
}
