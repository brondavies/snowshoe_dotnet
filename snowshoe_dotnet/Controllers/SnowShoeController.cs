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
        const string sss_action = "POST";
        const string sss_api_url = "http://beta.snowshoestamp.com/api/v2/stamp";

        [HttpPost]
        public ActionResult Stamp(string data)
        {
            string auth = GetShowShoeAuthorization();

            string result = MakeSnowShoeRequest(data, auth);
           
            //returns the plain JSON result from snowshoe API, use as you see fit
            return Content(result, "application/json");
        }
        
        private string GetShowShoeAuthorization()
        {
            //Update these two values in web.debug.config and web.release.config according to your environment
            string app_key = AppSetting("snowshoe:app_key");
            string app_secret = AppSetting("snowshoe:app_secret");
            
            OAuthRequest client = new OAuthRequest
            {
                Method = sss_action,
                RequestUrl = sss_api_url,
                ConsumerKey = app_key,
                ConsumerSecret = app_secret,
                Type = OAuthRequestType.RequestToken,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                SignatureTreatment = OAuthSignatureTreatment.Escaped,
                Version = "1.0"
            };

            return client.GetAuthorizationHeader();
        }

        private string AppSetting(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }

        private string MakeSnowShoeRequest(string data, string authorization)
        {
            string dataString = "data=" + Uri.EscapeDataString(data);
            byte[] dataBytes = Encoding.ASCII.GetBytes(dataString);

            HttpWebRequest sssRequest = (HttpWebRequest)WebRequest.Create(sss_api_url);
            sssRequest.Method = sss_action;
            sssRequest.Headers.Add("Authorization", authorization);
            sssRequest.ContentType = "application/x-www-form-urlencoded";
            sssRequest.ContentLength = dataBytes.Length;

            Stream requestStream = sssRequest.GetRequestStream();
            requestStream.Write(dataBytes, 0, dataBytes.Length);
            requestStream.Close();
            HttpWebResponse sssResponse = (HttpWebResponse)sssRequest.GetResponse();
            string responseFromServer;
            using (Stream dataStream = sssResponse.GetResponseStream())
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