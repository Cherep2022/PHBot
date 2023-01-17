using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using ZennoLab.InterfacesLibrary.ProjectModel;

namespace Requests
{
    internal class PostRequest
    {
        readonly IZennoPosterProjectModel project;
        public string Responce { get; set; }
        public string Accept { get; set; }
        public string Host { get; set; }
        public string Referer { get; set; }
        public string UserAgent { get; set; }
        public string Data { get; set; }
        public string ContentType { get; set; }
        public WebProxy Proxy { get; set; }
        private string Adress;
        public Dictionary<string, string> Headers { get; set; }
        HttpWebRequest Request;
        public PostRequest(string Adress, IZennoPosterProjectModel project)
        {
            this.project = project;
            this.Adress = Adress;
            Headers = new Dictionary<string, string>();
        }


        public void Run(System.Net.CookieContainer cookieContainer)
        {
            Request = (HttpWebRequest)WebRequest.Create(Adress);
            Request.Method = "POST";
            Request.CookieContainer = cookieContainer;
            Request.Proxy = Proxy;
            Request.Accept = Accept;
            Request.Host = Host;
            Request.ContentType = ContentType;
            Request.Referer = Referer;
            Request.UserAgent = UserAgent;

            byte[] sendData = Encoding.UTF8.GetBytes(Data);
            Request.ContentLength = sendData.Length;
            Stream sendStream = Request.GetRequestStream();
            sendStream.Write(sendData, 0, sendData.Length);
            sendStream.Close();

            foreach (var Pair in Headers)
            {
                Request.Headers.Add(Pair.Key, Pair.Value);
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