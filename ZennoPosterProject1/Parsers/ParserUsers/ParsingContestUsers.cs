using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using Requests;
using ZennoLab.InterfacesLibrary.ProjectModel;
using ZennoPosterProject1.DataBase.Model;
using ZennoPosterProject1.DataBase.Tables;
using HtmlAgilityPack;

namespace Parsers
{
    internal class ParsingContestUsers
    {
        
        readonly IZennoPosterProjectModel project;

        private WebProxy webProxy;
        private CookieContainer cookieContainer;
        private GetRequest getRequest;
        private HtmlDocument document;
        private HtmlNodeCollection htmlNodeCollection;
        private MatchCollection UserContestName;

        public ParsingContestUsers(IZennoPosterProjectModel project)
        {
            this.project = project;
        }
        public void StartContestParsing()
        {

            project.SendInfoToLog("Парсим участников конкурса.", true);
            project.SendInfoToLog("Страница номер: 1", true);

            webProxy = new WebProxy("103.152.17.8:63482", true);
            webProxy.Credentials = new NetworkCredential("6mZJADRWk", "bsK2DkkBg");

            cookieContainer = new CookieContainer();

            getRequest = new GetRequest("https://rt.pornhub.com/contest_hub/viewers_choice",project);
            getRequest.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            getRequest.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36 OPR/94.0.0.0";
            getRequest.Host = "rt.pornhub.com";
            getRequest.Proxy = webProxy;

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


            document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(getRequest.Responce);

            htmlNodeCollection = document.DocumentNode.SelectNodes("//p[contains(@class, 'username')]");


            
            using (PornHubDb dataBaseModelContainer = new PornHubDb())
            {
                dataBaseModelContainer.Database.ExecuteSqlCommand("TRUNCATE TABLE [UsersContest]");
                foreach (var htmlNode in htmlNodeCollection)
                {
                    UserContest userContest = new UserContest();
                    userContest.UserName = htmlNode.InnerText.ToLower().Replace(" ", "-");
                    dataBaseModelContainer.UserContests.Add(userContest);
                }
                dataBaseModelContainer.SaveChanges();
            }

            int Counter = 2;
            while (true)
            {

                project.SendInfoToLog("Страница номер: " + Counter, true);
                try
                {
                    getRequest = new GetRequest(String.Format(
                        "https://rt.pornhub.com/contest_hub/viewers_choice/videos?p={0}&code=2023-01&s=straight&o=mv",
                        Counter),project);

                    getRequest.Accept = @"*/*";
                    getRequest.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36 OPR/94.0.0.0";
                    getRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                    getRequest.Referer = "https://rt.pornhub.com/contest_hub/viewers_choice";
                    getRequest.Headers.Add(@"sec-ch-ua", "\"Chromium\"; v = \"108\", \"Opera\"; v = \"94\", \"Not)A;Brand\"; v = \"99\"");
                    getRequest.Headers.Add("sec-ch-ua-arch", "\"x86\"");
                    getRequest.Headers.Add("sec-ch-ua-full-version", "\"108.0.5359.125\"");
                    getRequest.Headers.Add("sec-ch-ua-mobile", "?0");
                    getRequest.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
                    getRequest.Headers.Add("sec-ch-ua-platform-version", "\"10.0.0\"");
                    getRequest.Headers.Add("Sec-Fetch-Dest", "empty");
                    getRequest.Headers.Add("Sec-Fetch-Mode", "cors");
                    getRequest.Headers.Add("Sec-Fetch-Site", "same-origin");
                    getRequest.Proxy = webProxy;
                    getRequest.Host = "rt.pornhub.com";

                    getRequest.Run(cookieContainer);

                    string UserContestNameRegex = "(?<=class=\\\\\"contestEntry--info\\\\\"><p\\ class=\\\\\"username\\\\\">).*?(?=<\\\\/p><p\\ class=\\\\\"position\\\\\")";
                    UserContestName = Regex.Matches(getRequest.Responce, UserContestNameRegex);

                    using (PornHubDb dataBaseModelContainer = new PornHubDb())
                    {
                        foreach (var userName in UserContestName)
                        {
                            UserContest userContest = new UserContest();
                            userContest.UserName = userName.ToString().ToLower().Replace("<//p>", "").Replace(" ", "-");
                            dataBaseModelContainer.UserContests.Add(userContest);
                        }
                        dataBaseModelContainer.SaveChanges();
                    }

                    Counter++;
                    Thread.Sleep(300);
                    if (UserContestName.Count == 0)
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
        }
    }
}
