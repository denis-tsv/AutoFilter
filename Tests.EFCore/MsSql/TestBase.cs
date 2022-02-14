using Tests.Data;
using System.Linq;
#if EF6
using System;
#endif

namespace Tests.EF
{
    public class TestBase
    {
        

        //private DbContextOptions<TestContext> options = new DbContextOptionsBuilder<TestContext>()
        //        .UseInMemoryDatabase(databaseName: "Test")
        //        .Options;



        protected TestContext Context { get; set; }
        private static bool initialized;
        protected void Init()
        {
            Context = new TestContext();

#if EF6
            Context.Database.Log = (s) => 
            {
                //C:\Users\DenisT\source\repos\ContestantRegister\AutoFilterTests.EF6\bin\Debug\ef6log.txt
                //File.AppendAllText("ef6log.txt", s);
                Console.Write(s);
            };
#endif
//            if (!initialized)
//            {
//#if EF_CORE
//                Context.Database.EnsureDeleted();
//                Context.Database.EnsureCreated();
//#endif
//                Context.ConvertItems.AddRange(ConverterTestsData.Items);
//                Context.FilterConditionItems.AddRange(FilterConditionTestsData.Items);
//                Context.StringTestItems.AddRange(StringTestsData.Items);
//                Context.NestedItems.AddRange(
//                    NavigationPropertyTestsData.Items.Where(x => x.NestedItem != null).Select(x => x.NestedItem));
//                Context.NavigationPropertyItems.AddRange(NavigationPropertyTestsData.Items);
//                Context.CompositeKindItems.AddRange(CompositeKindTestsData.Items);
//                Context.InvalidCaseItems.AddRange(InvalidCasesTestsData.Items);
//                Context.NotAutoFilteredItems.AddRange(NotAutoFilteredData.Items);
//                Context.RangeFilterItems.AddRange(RangeData.Items);
//                Context.SaveChanges();

//                initialized = true;
//            }
        }
    }
}

