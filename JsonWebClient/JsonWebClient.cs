using System.Collections.Generic;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bamberger.rocks
{
    public class JsonWebClient : WebClient
    {
        #region ctor
        public JsonWebClient() : base()
        {
            base.Encoding = Encoding.UTF8;
            base.Headers.Add("Content-Type", "application/json; charset=utf-8");
            base.Headers.Add("Accept", "application/json");
        }
        public JsonWebClient(IWebProxy proxy, Dictionary<string, string> headers) : base()
        {
            base.Encoding = Encoding.UTF8;
            base.Headers.Add("Content-Type", "application/json; charset=utf-8");
            base.Headers.Add("Accept", "application/json");

            if (proxy != null)
                this.Proxy = proxy;
            if (headers != null)
                foreach (var h in headers)
                    this.Headers.Add(h.Key, h.Value);
        }
        #endregion

        public bool responseNotEmpty = false;

        public string UploadObject(string url, object myObject, string method = "POST")
        {
            if(myObject == null)
                return UploadObject(url, new Dictionary<string, object>(), method);

            var vClass = myObject.GetType().ToString().Split('.');
            string vType = vClass[vClass.Length - 1];
            var variableName = vType.ToLower()[0] + vType.Substring(1, vType.Length - 1);

            return UploadObject(url, new Dictionary<string, object>() { { variableName, myObject } }, method);
        }

        public string UploadObject(string url, Dictionary<string, object> objectsDictionary, string method = "POST")
        {
            string inputJson;
            if (objectsDictionary.Count == 0)
            {
                inputJson = string.Empty;
            }
            else
            {
                JObject jObjects = new JObject();

                foreach (var obj in objectsDictionary)
                {
                    jObjects.Add(obj.Key, JObject.FromObject(obj.Value));
                }
                inputJson = jObjects.ToString(Formatting.None);
            }

            string rValue = base.UploadString(url, method, inputJson);

            responseNotEmpty = !string.IsNullOrEmpty(rValue);
            return rValue;
        }

        public T DownloadObject<T>(string url, string method = "POST")
        {
            return DownloadObject<T>(url, new Dictionary<string, object>(), method);
        }

        public T UpDownloadObject<T>(string url, object myObject, string method = "POST")
        {
            var resultString = UploadObject(url, myObject, method);
            return JsonConvert.DeserializeObject<T>(resultString);
        }

        public T DownloadObject<T>(string url, Dictionary<string, object> dic, string method = "POST")
        {
            var resultString = UploadObject(url, dic, method);
            return JsonConvert.DeserializeObject<T>(resultString);
        }

        #region static methods
        public static string UploadObject(object objectToSend, string url, string method = "POST", Dictionary<string, string> headers = null, IWebProxy proxy = null)
        {
            using (JsonWebClient client = new JsonWebClient(proxy, headers))
            {
                return client.UploadObject(url, objectToSend, method);
            }
        }

        public static T DownloadObject<T>(string url, string method = "POST", Dictionary<string, string> headers = null, IWebProxy proxy = null)
        {
            using (JsonWebClient client = new JsonWebClient(proxy, headers))
            {
                return client.DownloadObject<T>(url, method);
            }
        }

        public static T UpDownloadObject<T>(string url, object objectToSend, string method = "POST", Dictionary<string, string> headers = null, IWebProxy proxy = null)
        {
            using (JsonWebClient client = new JsonWebClient(proxy, headers))
            {
                return client.UpDownloadObject<T>(url, objectToSend, method);
            }
        }
        #endregion
    }
}
