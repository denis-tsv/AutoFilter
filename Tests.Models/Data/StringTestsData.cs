using Tests.Models;
using System.Collections.Generic;

namespace Tests.Data
{
    public class StringTestsData
    {
        public static List<StringTestItem> Items = new List<StringTestItem>
        {
            //NoAttributes
            new StringTestItem
            {
                NoAttribute = "NoAttributeOk"
            },
            new StringTestItem
            {
                NoAttribute = "NoNoAttribute"
            },
            new StringTestItem
            {
                NoAttribute = "noattribute"
            },
            new StringTestItem
            {
                NoAttribute = "nonoattribute"
            },

            //contains case
            new StringTestItem
            {
                ContainsCase = "TestContainsCase"
            },
            new StringTestItem
            {
                ContainsCase = "containscase"
            },       
            
            //ContainsIgnoreCase
            new StringTestItem
            {
                ContainsIgnoreCase = "TestContainsIgnoreCase"
            },
            new StringTestItem
            {
                ContainsIgnoreCase = "testcontainsignorecase"
            },
            new StringTestItem
            {
                ContainsIgnoreCase = "Not contains ignore case"
            },
            
            //StartsWithCase
            new StringTestItem
            {
                StartsWithCase = "StartsWithCase"
            },
            new StringTestItem
            {
                StartsWithCase = "startswithcase"
            },
            new StringTestItem
            {
                StartsWithCase = "NotStartsWithCase"
            },
            new StringTestItem
            {
                StartsWithCase = "notntartswithcase"
            },

            //StartsWithIgnoreCase
             new StringTestItem
            {
                StartsWithIgnoreCase = "StartsWithIgnoreCaseTest"
            },
            new StringTestItem
            {
                StartsWithIgnoreCase = "startswithignorecasetest"
            },
            new StringTestItem
            {
                StartsWithIgnoreCase = "NotStartsWithIgnoreCase"
            },
            new StringTestItem
            {
                StartsWithIgnoreCase = "notstartswithIgnoreCase"
            },

            //PropertyName
            new StringTestItem
            {
                PropertyName = "PropertyName"
            },
            new StringTestItem
            {
                TargetStringProperty = "PropertyName"
            },

            
            new StringTestItem
            {
                
            },

        };

        

    }
}
