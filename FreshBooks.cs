using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Dynamic;

namespace HastyAPI.FreshBooks {
	public class FreshBooks {
		string Username { get; set; }
		string Token { get; set; }

		public FreshBooks(string username, string token) {
			Username = username;
			Token = token;
		}

		public dynamic Call(string method, Action<dynamic> getinputs = null) {
			var req = CreateRequestXml(method, getinputs);

			System.Diagnostics.Debug.WriteLine("Sending request:\r\n" + req.ToString());

			var text = new APIRequest("https://" + Username + ".freshbooks.com/api/2.1/xml-in")
				.WithBasicCredentials(Token, null)
				.WithData(req.ToString())
				.Post()
				.EnsureStatus(200)
				.Text;

			var resp = XDocument.Parse(text);
			System.Diagnostics.Debug.WriteLine("Received response:\r\n" + resp.ToString());

			return resp.ToDynamic();
		}

		private static XDocument CreateRequestXml(string method, Action<dynamic> getinputs) {
			var xml = new XDocument();

			var request = new XElement("request");
			request.SetAttributeValue("method", method);
			xml.Add(request);

			if(getinputs != null) {
				dynamic inputs = new ExpandoObject();
				getinputs(inputs);

			    CreateObject(request, inputs);				
			}

			return xml;
		}

        private static void CreateObject(XElement request, object objectValue)
	    {
            if (objectValue == null)
	            return;

            var potentialDynamic = (objectValue as IDictionary<string, object>);
            if (potentialDynamic == null)
                return;

            foreach (var p in potentialDynamic)
            {
                var name = p.Key;
                var value = p.Value;
                var complexType = false;

                if (value is DateTime)
                    value = ((DateTime)value).ToString("yyyy-MM-dd hh:mm:ss");
                else if (value is bool)
                    value = (bool)value ? "1" : "0";
                else if (value is ExpandoObject)
                    complexType = true;

                if (!complexType)
                    request.Add(new XElement(name, value));
                else
                {
                    var newElement = new XElement(name);

                    CreateObject(newElement, value);

                    request.Add(newElement);
                }
            }
	    }
	}
}
