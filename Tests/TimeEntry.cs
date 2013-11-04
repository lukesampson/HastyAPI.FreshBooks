using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace HastyAPI.FreshBooks.Tests {
	public class TimeEntry {

		public void CreateTimeEntry() {
			var fb = Shared.NewCaller();

			var res = FreshBooks.CreateRequestXML("time_entry.create", x => x.time_entry = new {
				task_id = 999999,
				hours = 25,
                something = "else"
			});
            Console.Write(res.ToString());

            dynamic time_entry = new ExpandoObject();
            time_entry.task_id = 999998;
            time_entry.hours = 24;
            time_entry.something = "otherwise";

            res = FreshBooks.CreateRequestXML("time_entry.create", x => x.time_entry = time_entry);
            Console.Write(res.ToString());		
		}
	}
}
