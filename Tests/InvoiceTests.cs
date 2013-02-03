using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HastyAPI.FreshBooks.Tests {
    public class InvoiceTests {
        public void ListAll() {
            var res = Shared.NewCaller().Call("invoice.list");
            Console.Write(res.ToString());
        }
    }
}
