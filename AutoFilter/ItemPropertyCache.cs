using System.Collections.Generic;
using System.Linq;

namespace AutoFilter
{
    public static class ItemPropertyCache<TItem>
    {
        public static readonly HashSet<string> PropertyNames;

        static ItemPropertyCache()
        {
            PropertyNames = new HashSet<string>(typeof(TItem).GetProperties().Select(x => x.Name));
        }
    }
}
