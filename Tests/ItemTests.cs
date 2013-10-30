using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HastyAPI.FreshBooks.Tests {

    public class ItemTests {

        public void CreateItem() {
            var fb = Shared.NewCaller();
            var res = fb.Call("item.create", x => x.item = new {
                name = "Fuzzy Slippers",
                description = "Extra soft",
                unit_cost = "59.99",
                quantity = 1,
                inventory = 10
            });

            Console.Write(res.ToString());
        }
    }
}
