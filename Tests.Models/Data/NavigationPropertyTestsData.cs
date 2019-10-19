using Tests.Models;
using System.Collections.Generic;

namespace Tests.Data
{
    public class NavigationPropertyTestsData
    {
        public static List<NavigationPropertyItem> Items = new List<NavigationPropertyItem>
        {
            new NavigationPropertyItem
            {

            },
            new NavigationPropertyItem
            {
                NestedItem = new NestedItem
                {
                }
            },
            
            //Int
            new NavigationPropertyItem
            {
                NestedItem = new NestedItem
                {
                    Int = 1
                }
            },
            new NavigationPropertyItem
            {
                NestedItem = new NestedItem
                {
                    Int = 2
                }
            },

            //NullableInt
            new NavigationPropertyItem
            {
                NestedItem = new NestedItem
                {
                    NullableInt = 1
                }
            },
            new NavigationPropertyItem
            {
                NestedItem = new NestedItem
                {
                    NullableInt = 2
                }
            },
            new NavigationPropertyItem
            {
                NestedItem = new NestedItem
                {
                    NullableInt = -1
                }
            },

            //string
             new NavigationPropertyItem
            {
                NestedItem = new NestedItem
                {
                    String = "Nested"
                }
            },
            new NavigationPropertyItem
            {
                NestedItem = new NestedItem
                {
                    String = "NotInResult"
                }
            },           
            
        };


        
    }
}
