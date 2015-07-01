using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace JoyHoodApp
{
    public class VKAPI
    {
        private readonly string _accessToken;

        public VKAPI(string accessToken)
        {
            _accessToken = accessToken;
        }

        public string ExecuteCommand(string name, NameValueCollection qs)
        {
            string jsonString = String.Empty;
            string url = string.Empty;
            if (qs != null)
            {
                url = String.Format(Constants.REQUEST_URL_PARAMS, name, _accessToken,
                    String.Join("&", from item in qs.AllKeys select item + "=" + qs[item]));
            }
            else
            {
                url = String.Format(Constants.REQUEST_URL, name, _accessToken);
            }
            var request = WebRequest.Create(url);
            var response = request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                jsonString = reader.ReadToEnd().Trim();
            }

            return jsonString;
        }
    }
}
