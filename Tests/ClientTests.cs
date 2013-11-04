using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace HastyAPI.FreshBooks.Tests {
	public class ClientTests {

		public void ListAll() {
			var res = Shared.NewCaller().Call("client.list");
			Console.Write(res.ToString());
		}

        public void Clients_Get_Request() {
            var req = FreshBooks.CreateRequestXML("client.get", x => x.client_id = 12);
            Console.Write(req.ToString());
        }

		public void Clients_Get() {
			var res = Shared.NewCaller().Call("client.get", x => x.client_id = 12);
			Console.Write(res.ToString());

			Console.Write(res.response);
		}
	}
}
