using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HastyAPI.FreshBooks.Tests {
    public class ExpenseTests {
        public void ListAllPage5() {
            var res = Shared.NewCaller().Call("expense.list", (x) => { x.page = 5; });
            //Console.WriteLine(res);
        }

        public void ListAllVendor() {
            var res = Shared.NewCaller().Call("expense.list", (x) => { x.vendor = "Luke Sampson"; x.page = 2; });
            //Console.WriteLine(res);
        }

        public void TestContractor() {
            var res = Shared.NewCaller().Call("contractor.list");
        }
    }
}
