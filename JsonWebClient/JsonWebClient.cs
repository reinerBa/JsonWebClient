using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonWebClient
{
    public class JsonWebClient : WebClient
    {
        public JsonWebClient() : base() {
           base.Encoding = Encoding.UTF8;
           base.Headers.Add("Content-Type", "application/json; charset=utf-8");
           base.Headers.Add("Accept", "application/json");
        }
        public bool responseNotEmpty = false;

        public string UploadObject(string url, object myObject, string method = "POST")
        {
            var vClass = myObject.GetType().ToString().Split('.');
            string vType = vClass[vClass.Length - 1];
            var variableName = vType.ToLower()[0] + vType.Substring(1, vType.Length - 1);

            return UploadObject(url, new Dictionary<string, object>() { { variableName, myObject } }, method);
        }

        public string UploadObject(string url, Dictionary<string, object> dic, string method = "POST")
        {
            JObject jObjects = new JObject();

            var jsonString = string.Empty;
            foreach(var obj  in dic)
            {
                jObjects.Add(obj.Key, JObject.FromObject(obj.Value));
            }

            string rValue = base.UploadString(url, method, jObjects.ToString(Formatting.None));
            responseNotEmpty = !string.IsNullOrEmpty(rValue);
            return rValue;
        }

        public T DownloadObject<T>(string url, object myObject, string method = "POST")
        {
            var resultString = UploadObject(url, myObject, method);
            return JsonConvert.DeserializeObject<T>(resultString);
        }

        public T DownloadObject<T>(string url, Dictionary<string, object> dic, string method = "POST")
        {
            var resultString = UploadObject(url, dic, method);
            return JsonConvert.DeserializeObject<T>(resultString);
        }

        public static string UploadObject(object objectToSend, string url, string method = "POST", Dictionary<string, string> headers = null, IWebProxy proxy = null)
        {
            using(JsonWebClient client = new JsonWebClient())
            {
                if (proxy != null)
                    client.Proxy = proxy;
                if (headers != null)
                    foreach (var h in headers)
                        client.Headers.Add(h.Key, h.Value);
                
                return client.UploadObject(url, objectToSend, method);
            }
        }

        public static T DownloadObject<T>(object objectToSend, string url, string method = "POST", Dictionary<string, string> headers = null, IWebProxy proxy = null)
        {
            using (JsonWebClient client = new JsonWebClient())
            {
                if (proxy != null)
                    client.Proxy = proxy;
                if (headers != null)
                    foreach (var h in headers)
                        client.Headers.Add(h.Key, h.Value);

                return client.DownloadObject<T>(url, objectToSend, method);
            }
        }
    }
}