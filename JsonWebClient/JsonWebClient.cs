using System.Collections.Generic;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bamberger.rocks
{
    /// <summary>
    /// Serializes and Deserializes objects with the use of Newtonsoft.Json that can used as Controller method inputs
    /// with the require the object classes have a default constructor and the properties have {get; set;}. 
    /// Without that the .Net Serializer can't do it's duty.
    /// 
    /// Be carefull with the methods that takes a List parameter, the class types will be used as parameter names (with first Letter lowercase)
    /// </summary>
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

        /// <summary>
        /// Indicates if a http response body wasn't empty or null
        /// </summary>
        public bool responseNotEmpty = false;

        /// <summary>
        /// Uploads a object 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="myObject">The object to send. The class name, with first character lowercase, will be used as key. 
        /// Instanceof a DateTime -> Controller.Method(DateTime dateTime)
        /// </param>
        /// <param name="method">POST, GET, etc.</param>
        /// <returns></returns>
        public string UploadObject(string url, object myObject, string method = "POST")
        {
            if(myObject == null)
                return UploadObject(url, new Dictionary<string, object>(), method);

            var vClass = myObject.GetType().ToString().Split('.');
            string vType = vClass[vClass.Length - 1];
            var variableName = vType.ToLower()[0] + vType.Substring(1, vType.Length - 1);

            return UploadObject(url, new Dictionary<string, object>() { { variableName, myObject } }, method);
        }

        public string UploadObject(string url, List<object> objectsList, string method = "POST")
        {
            string inputJson;
            if (objectsList.Count == 0)
            {
                inputJson = string.Empty;
            }
            else
            {
                JObject jObjects = new JObject();

                foreach (var obj in objectsList)
                {
                    if (obj == null)
                        continue;

                    var vClass = obj.GetType().ToString().Split('.');
                    string vType = vClass[vClass.Length - 1];
                    var variableName = vType.ToLower()[0] + vType.Substring(1, vType.Length - 1);
                    jObjects.Add(variableName, JObject.FromObject(obj));
                }
                inputJson = jObjects.ToString(Formatting.None);
            }

            string rValue = base.UploadString(url, method, inputJson);

            responseNotEmpty = !string.IsNullOrEmpty(rValue);
            return rValue;
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

                foreach (KeyValuePair<string, object> pair in objectsDictionary)
                {
                    var value = JObject.FromObject(pair.Value);
                    jObjects.Add(pair.Key, value);
                }
                inputJson = jObjects.ToString(Formatting.None);
            }

            string rValue = base.UploadString(url, method, inputJson);

            responseNotEmpty = !string.IsNullOrEmpty(rValue);
            return rValue;
        }

        // would ambigous with the static method
        //public T DownloadObject<T>(string url, string method = "POST")
        //{
        //    return UpDownloadObject<T>(url, new Dictionary<string, object>(), method);
        //}

        public T UpDownloadObject<T>(string url, object myObject, string method = "POST")
        {
            var resultString = UploadObject(url, myObject, method);
            return JsonConvert.DeserializeObject<T>(resultString);
        }

        public T UpDownloadObject<T>(string url, List<object> list, string method = "POST")
        {
            var resultString = UploadObject(url, list, method);
            return JsonConvert.DeserializeObject<T>(resultString);
        }

        public T UpDownloadObject<T>(string url, Dictionary<string, object> dic, string method = "POST")
        {
            var resultString = UploadObject(url, dic, method);
            return JsonConvert.DeserializeObject<T>(resultString);
        }

        #region static methods
        public static string UploadObject(object objectToSend, string url, string method = "POST", Dictionary<string, string> headers = null, IWebProxy proxy = null)
        {
            using (JsonWebClient client = new JsonWebClient(proxy, headers))
                return client.UploadObject(url, objectToSend, method);
        }

        public static string UploadObject(List<object> list, string url, string method = "POST", Dictionary<string, string> headers = null, IWebProxy proxy = null)
        {
            using (JsonWebClient client = new JsonWebClient(proxy, headers))
                return client.UploadObject(url, list, method);
        }

        public static string UploadObject(Dictionary<string, object> objectsDictionary, string url, string method = "POST", Dictionary<string, string> headers = null, IWebProxy proxy = null)
        {
            using (JsonWebClient client = new JsonWebClient(proxy, headers))
                return client.UploadObject(url, objectsDictionary, method);
        }

        public static T UpDownloadObject<T>(object objectToSend, string url, string method = "POST", Dictionary<string, string> headers = null, IWebProxy proxy = null)
        {
            using (JsonWebClient client = new JsonWebClient(proxy, headers))
                return client.UpDownloadObject<T>(url, objectToSend, method);
        }
        public static T UpDownloadObject<T>(List<object> list, string url, string method = "POST", Dictionary<string, string> headers = null, IWebProxy proxy = null)
        {
            using (JsonWebClient client = new JsonWebClient(proxy, headers))
                return client.UpDownloadObject<T>(url, list, method);
        }
        public static T UpDownloadObject<T>(Dictionary<string, object> objectsDictionary, string url, string method = "POST", Dictionary<string, string> headers = null, IWebProxy proxy = null)
        {
            using (JsonWebClient client = new JsonWebClient(proxy, headers))
                return client.UpDownloadObject<T>(url, objectsDictionary, method);
        }

        public static T DownloadObject<T>(string url, string method = "POST", Dictionary<string, string> headers = null, IWebProxy proxy = null)
        {
            using (JsonWebClient client = new JsonWebClient(proxy, headers))
                return client.UpDownloadObject<T>(url, new Dictionary<string, object>(), method);

        }
        #endregion
    }
}
