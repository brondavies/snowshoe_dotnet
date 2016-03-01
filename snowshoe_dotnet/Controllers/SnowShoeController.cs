using OAuth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace snowshoe_dotnet.Controllers
{
    public class SnowShoeController : Controller
    {
        [HttpPost]
        public ActionResult Stamp(string data)
        {
            //Update these two values in web.debug.config and web.release.config according to your environment
            string app_key = AppSetting("snowshoe:application_key");
            string app_secret = AppSetting("snowshoe:app_secret");

            string action = "POST";
            string url = "http://beta.snowshoestamp.com/api/v2/stamp";

            string dataString = "data=" + Uri.EscapeDataString(data);

            OAuthRequest client = OAuthRequest.ForRequestToken(app_key, app_secret);
            client.Method = action;
            client.RequestUrl = url;

            string auth = client.GetAuthorizationHeader();

            string result = SnowShoeRequest(action, url, dataString, auth);

            //returns the plain JSON result from snowshoe API, use as you see fit
            return Content(result, "application/json");
        }

        private string AppSetting(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }

        private string SnowShoeRequest(string action, string url, string dataString, string authorization)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(dataString);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.AllowAutoRedirect = true;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = dataBytes.Length;
            request.Headers.Add("Authorization", authorization);
            request.Method = action;

            var stream = request.GetRequestStream();
            stream.Write(dataBytes, 0, dataBytes.Length);
            stream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseFromServer;
            using (Stream dataStream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    responseFromServer = reader.ReadToEnd();
                }
            }
            return responseFromServer;
        }
    }
}