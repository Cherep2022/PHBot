using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using Requests;
using Global.Zennolab.HtmlAgilityPack;
using ZennoLab.InterfacesLibrary.ProjectModel;
using ZennoPosterProject1.Parsers.ParserUsers;

namespace Parsers
{
    internal class ParsingContestUsers
    {
        
        readonly IZennoPosterProjectModel project;

        public ParsingContestUsers(IZennoPosterProjectModel project)
        {
            this.project = project;
        }
        public List<string> StartContestParsing()
        {

            project.SendInfoToLog("Парсим участников конкурса.", true);
            project.SendInfoToLog("Страница номер: 1", true);

            List<string> UserList = new List<string>();
            var Proxy = new WebProxy("103.152.17.8:63482", true);
            Proxy.Credentials = new NetworkCredential("6mZJADRWk", "bsK2DkkBg");
            var cookieContainer = new CookieContainer();

            var getRequest = new GetRequest("https://rt.pornhub.com/contest_hub/viewers_choice",project);
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
                document.DocumentNode.SelectNodes("//p[contains(@class, 'username')]");

            foreach (var htmlNode in htmlNodeCollection)
            {
                UserList.Add(htmlNode.InnerText.ToLower().Replace(" ", "-"));
            }

            int Counter = 2;
            while (true)
            {

                project.SendInfoToLog("Страница номер: " + Counter, true);
                try
                {
                    var getRequest2 = new GetRequest(String.Format(
                        "https://rt.pornhub.com/contest_hub/viewers_choice/videos?p={0}&code=2023-01&s=straight&o=mv",
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

                    var regex2 = "(?<=class=\\\\\"contestEntry--info\\\\\"><p\\ class=\\\\\"username\\\\\">).*?(?=<\\\\/p><p\\ class=\\\\\"position\\\\\")";
                    MatchCollection matches3 = Regex.Matches(getRequest2.Responce, regex2);

                    foreach (var VARIABLE in matches3)
                    {
                        UserList.Add(VARIABLE.ToString().ToLower().Replace("<//p>", "").Replace(" ", "-"));
                    }

                    Counter++;
                    Thread.Sleep(300);
                    if (matches3.Count == 0)
                    {
                        break;
                    }
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
            project.SendInfoToLog("Закончили парсить участников конкурса.", true);
            return UserList;

        }

    }
}
