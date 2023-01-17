using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Global.Zennolab.HtmlAgilityPack;
using Requests;
using ZennoLab.InterfacesLibrary.ProjectModel;
using ZennoPosterProject1.Parsers.ParserUsers;

namespace Parsers
{
    internal class ParsingFriends
    {
        
        readonly IZennoPosterProjectModel project;


        public ParsingFriends(IZennoPosterProjectModel project)
        {
            this.project = project;
        }
        public List<string> StartFriendParsing()
        {
            project.SendInfoToLog("Парсим друзей.",true);
            project.SendInfoToLog("Страница номер: 1",true);
            //127.0.0.1:8888
            var UserList = new List<string>();
            var Proxy = new WebProxy("103.152.17.8:63482", true);
            Proxy.Credentials = new NetworkCredential("6mZJADRWk", "bsK2DkkBg");

            var cookieContainer = new CookieContainer();

            var getRequest = new GetRequest("https://rt.pornhub.com/model/goddessthaleia/friends",project);
            getRequest.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            getRequest.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36 OPR/94.0.0.0";
            getRequest.Host = "rt.pornhub.com";
            getRequest.Proxy = Proxy;

            getRequest.Headers.Add(@"sec-ch-ua", "\"Chromium\"; v = \"108\", \"Opera\"; v = \"94\", \"Not)A;Brand\"; v = \"99\"");
            getRequest.Headers.Add("sec-ch-ua-arch", "\"x86\"");
            getRequest.Headers.Add("sec-ch-ua-full-version", "\"108.0.5359.125\"");
            getRequest.Headers.Add("sec-ch-ua-mobile", "?0");
            getRequest.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
            getRequest.Headers.Add("sec-ch-ua-platform-version", "\"10.0.0\"");
            getRequest.Headers.Add("Sec-Fetch-Dest", "document");
            getRequest.Headers.Add("Sec-Fetch-Mode", "navigate");
            getRequest.Headers.Add("Sec-Fetch-Site", "none");
            getRequest.Headers.Add("Sec-Fetch-User", "?1");
            getRequest.Headers.Add("Upgrade-Insecure-Requests", "1");

            getRequest.Run(cookieContainer);


            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(getRequest.Responce);

            HtmlNodeCollection htmlNodeCollection =
                document.DocumentNode.SelectNodes("//a[contains(@class, 'usernameLink')] ");

            foreach (var htmlNode in htmlNodeCollection)
            {
                UserList.Add(
                    htmlNode.InnerText.ToLower().Replace(" ", "-"));
            }


            int Counter = 2;
            while (true)
            {

                project.SendInfoToLog("Страница номер: " + Counter, true);
                try
                {
                    var getRequest2 = new GetRequest(String.Format(
                        "https://rt.pornhub.com/model/goddessthaleia/friends/ajax?o=recent_friendships&page={0}",
                        Counter),project);
                    getRequest2.Accept = @"*/*";
                    getRequest2.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36 OPR/94.0.0.0";
                    getRequest2.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                    getRequest2.Referer = "https://rt.pornhub.com/contest_hub/viewers_choice";
                    getRequest2.Headers.Add(@"sec-ch-ua", "\"Chromium\"; v = \"108\", \"Opera\"; v = \"94\", \"Not)A;Brand\"; v = \"99\"");
                    getRequest2.Headers.Add("sec-ch-ua-arch", "\"x86\"");
                    getRequest2.Headers.Add("sec-ch-ua-full-version", "\"108.0.5359.125\"");
                    getRequest2.Headers.Add("sec-ch-ua-mobile", "?0");
                    getRequest2.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
                    getRequest2.Headers.Add("sec-ch-ua-platform-version", "\"10.0.0\"");
                    getRequest2.Headers.Add("Sec-Fetch-Dest", "empty");
                    getRequest2.Headers.Add("Sec-Fetch-Mode", "cors");
                    getRequest2.Headers.Add("Sec-Fetch-Site", "same-origin");
                    getRequest2.Proxy = Proxy;
                    getRequest2.Host = "rt.pornhub.com";

                    getRequest2.Run(cookieContainer);
                    document.LoadHtml(getRequest2.Responce);

                    htmlNodeCollection =
                        document.DocumentNode.SelectNodes("//a[contains(@class, 'usernameLink')] ");
                    if (htmlNodeCollection == null)
                    {
                        break;
                    }
                    foreach (var htmlNode in htmlNodeCollection)
                    {
                        UserList.Add(htmlNode.InnerText.ToLower().Replace(" ", "-"));
                    }

                    Counter++;
                    Thread.Sleep(300);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    break;
                }

            }
            List<string> ListTemp = UserList.Distinct().ToList(); // создаем временный список
            UserList.Clear(); // очищаем исходный список
            UserList.AddRange(ListTemp); // из временного списка переносим в исходный список
            project.SendInfoToLog("Закончили парсить друзей.", true);
            return UserList;
        }

    }
}
