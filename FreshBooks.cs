using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Dynamic;

namespace HastyAPI.FreshBooks
{
    public class FreshBooks
    {
        string Username { get; set; }
        string Token { get; set; }
        bool Verbose { get; set; }

        public FreshBooks(string username, string token, bool verbose = true)
        {
            Username = username;
            Token = token;
            Verbose = verbose;
        }

        public dynamic Call(string method, Action<dynamic> getinputs = null)
        {
            var req = CreateRequestXML(method, getinputs);

            if (Verbose)
                System.Diagnostics.Debug.WriteLine("Sending request:\r\n" + req.ToString());

            var text = new APIRequest("https://" + Username + ".freshbooks.com/api/2.1/xml-in")
                .WithBasicCredentials(Token, null)
                .WithData(req.ToString())
                .Post()
                .EnsureStatus(200)
                .Text;

            var resp = XDocument.Parse(text);

            if (Verbose)
                System.Diagnostics.Debug.WriteLine("Received response:\r\n" + resp.ToString());

            return resp.ToDynamic();
        }

        private static XDocument CreateRequestXML(string method, Action<dynamic> getinputs)
        {
            var xml = new XDocument();

            var request = new XElement("request");
            request.SetAttributeValue("method", method);
            xml.Add(request);

            if (getinputs != null)
            {
                dynamic inputs = new ExpandoObject();
                getinputs(inputs);

                foreach (var p in (inputs as IDictionary<string, object>))
                    request.Add(XMLElement(p.Key, p.Value));
            }

            return xml;
        }

        private static XElement XMLElement(string name, object value)
        {
            if (value == null) return null;

            if (value is DateTime) value = ((DateTime)value).ToString("yyyy-MM-dd hh:mm:ss");
            else if (value is bool) value = (bool)value ? "1" : "0";

            if (value.GetType().IsValueType || value is string)
            {
                return new XElement(name, value);
            }

            var potentialDynamic = (value as IDictionary<string, object>);
            if (potentialDynamic == null)
            {
                // If the object is not a dynamic one, go the reflection way
                var request = new XElement(name);

                foreach (var prop in value.GetType().GetProperties())
                {
                    var child = prop.GetValue(value, null);
                    return new XElement(name, XMLElement(prop.Name, child));
                }

                return request;
            }
            else
            {
                // If the object is dynamic, process accordingly
                var request = new XElement(name);

                foreach (var p in potentialDynamic)
                {
                    var newElement = XMLElement(p.Key, p.Value);
                    request.Add(newElement);
                }

                return request;
            }
        }
    }
}
