using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Resources;
using System.Text;
using System.Threading;
using ZennoLab.CommandCenter;
using ZennoLab.Emulation;
using ZennoLab.InterfacesLibrary.ProjectModel;
using ZennoLab.InterfacesLibrary.ProjectModel.Enums;
using ZennoPosterEmulation;

namespace Requests
{
    internal class GetRequest
    {
        public string Responce { get; set; }
        public string Accept { get; set; }
        public string Host { get; set; }
        public string Referer { get; set; }
        public string UserAgent { get; set; }
        public string ContentType { get; set; }
        public WebProxy Proxy { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        private string Adress;
        HttpWebRequest Request;
        readonly IZennoPosterProjectModel project;

        public GetRequest(string Adress,IZennoPosterProjectModel project)
        {
            this.project = project;
            this.Adress = Adress;
            Headers = new Dictionary<string, string>();
        }
        public void Run(System.Net.CookieContainer cookieContainer)
        {
            Request = (HttpWebRequest)WebRequest.Create(Adress);
            Request.Method = "GET";
            Request.CookieContainer = cookieContainer;
            Request.Proxy = Proxy;
            Request.Accept = Accept;
            Request.Host = Host;
            Request.Referer = Referer;
            Request.UserAgent = UserAgent;
            Request.ContentType = ContentType;
            foreach (var pair in Headers)
            {
                Request.Headers.Add(pair.Key, pair.Value);
            }

            try
            {
                HttpWebResponse response = (HttpWebResponse)Request.GetResponse();
                var stream = response.GetResponseStream();
                if (stream != null) Responce = new StreamReader(stream).ReadToEnd();
            }
            catch (Exception e)
            {
                project.SendErrorToLog(e.Message,true);
            }
        }

    }
}