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

            //EndsWithCase
            new StringTestItem
            {
                EndsWithCase = "EndsWithCase"
            },
            new StringTestItem
            {
                EndsWithCase = "endswithcase"
            },
            new StringTestItem
            {
                EndsWithCase = "EndsWithCaseNot"
            },
            new StringTestItem
            {
                EndsWithCase = "endswithcasenot"
            },

            //EndsWithIgnoreCase
            new StringTestItem
            {
                EndsWithIgnoreCase = "EndsWithIgnoreCase"
            },
            new StringTestItem
            {
                EndsWithIgnoreCase = "endswithignorecase"
            },
            new StringTestItem
            {
                EndsWithIgnoreCase = "EndsWithIgnoreCaseNot"
            },
            new StringTestItem
            {
                EndsWithIgnoreCase = "endswithignorecasenot"
            },

            //EqualsCase
            new StringTestItem
            {
                EqualsCase = "EqualsCase"
            },
            new StringTestItem
            {
                EqualsCase = "equalscase"
            },
            new StringTestItem
            {
                EqualsCase = "EqualsCaseTest"
            },
            new StringTestItem
            {
                EqualsCase = "equalscasetest"
            },
            new StringTestItem
            {
                EqualsCase = "TestEqualsCase"
            },
            new StringTestItem
            {
                EqualsCase = "testequalscase"
            },

            //EqualsIgnoreCase
            new StringTestItem
            {
                EqualsIgnoreCase = "EqualsIgnoreCase"
            },
            new StringTestItem
            {
                EqualsIgnoreCase = "equalsignorecase"
            },
            new StringTestItem
            {
                EqualsIgnoreCase = "EqualsIgnoreCaseTest"
            },
            new StringTestItem
            {
                EqualsIgnoreCase = "equalsignorecasetest"
            },
            new StringTestItem
            {
                EqualsIgnoreCase = "TestEqualsIgnoreCase"
            },
            new StringTestItem
            {
                EqualsIgnoreCase = "testequalsignorecase"
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
