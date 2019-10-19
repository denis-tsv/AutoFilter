using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFrameworkConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var st = new Tests.EF.StringTests();
            st.NoAttributes();
            st.ContainsCase();
            st.ContainsIgnoreCase();
            st.StartsWithCase();
            st.StartsWithIgnoreCase();
        }
    }
}

