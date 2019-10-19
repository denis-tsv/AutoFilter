using Tests.Models;
using System.Collections.Generic;

namespace Tests.Data
{
    public class InvalidCasesTestsData
    {
        public static List<InvalidCaseItem> Items = new List<InvalidCaseItem>
        {
            new InvalidCaseItem
            {
                TargetEnum = TargetEnum.Default
            },
            new InvalidCaseItem
            {
                TargetEnum = TargetEnum.First
            },
        };

       
    }
}
