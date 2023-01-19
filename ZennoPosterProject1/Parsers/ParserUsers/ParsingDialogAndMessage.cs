﻿using Global.Zennolab.HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZennoLab.CommandCenter;
using ZennoLab.InterfacesLibrary.ProjectModel;
using static System.Net.Mime.MediaTypeNames;

namespace ZennoPosterProject1.Parsers.ParserUsers
{
    internal class ParsingDialogAndMessage
    {
        readonly IZennoPosterProjectModel project;


        public ParsingDialogAndMessage(IZennoPosterProjectModel project)
        {
            this.project = project;
        }
        public List<string> StartDialogAndMessageParsing()
        {
            var UserList = new List<string>();
            project.SendInfoToLog("Парсим диалоги.", true);
            project.SendInfoToLog("Страница номер: 1", true);


            string[] nums = new string[12];

            nums[0] = "Accept: text / html,application / xhtml + xml,application / xml; q = 0.9,image / avif,image / webp,image / apng,*/*; q = 0.8,application / signed - exchange; v = b3; q = 0.9";
            nums[1] = "Accept-Language: ru-RU,ru;q=0.7,en-US;q=0.5,en;q=0.2";
            nums[2] =
                "Cookie: bs=3qu09xwiz4lqympnoaui4d4khmzqmeen; ss=309682650025913921; fg_0d2ec4cbd943df07ec161982a603817e=8721.100000; hasVisited=1; __s=63BAFF4B-42FE722901BBD98D-4BB4A0CA; __l=63BAFF4B-42FE722901BBD98D-4BB4A0CA; atatusScript=hide; d_fs=1; d_uidb=f8d6f3d6-4a18-a0d9-0a00-a8f34dc08d4f; local_storage=1; il=v1a14LhYPrvL7RNOa4S-sNy4wiWWq5uona2HYrP1O0A2gxNjgwOTc1NTgwVzlzX0JOUUJrcmlxYW5RdlVHUGtHNGpSY0VrVmJXRFktQThmZUFlNg..; expiredEnterModalShown=1; platform=mobile; age_verified=v1KaMXojGHCj5s3FLLUvV58sZUx6O6QLOe0cAtdHn7n6sxNjc0NjY1NTQwdmsxLmEuOUdxTGxmTEFGSGNvVHllLU1ielFScC1KOXR0MnF3OVViWmlPNzhfWG9xa2JLdEstN3M3Wm90R0pObEF6aWRMZHRqRjdlZkoxQ3o4Nl9RYlI5RUpjaEp4RUE4UHpjVzZCVThoMDlKM0ZBcWxWMUo1RTkyWnQzMHpya0E0dTJpMDhDYkM2eVZHU25BclctTDRGWUZfV2FpN19LUmQ2cVR6MW9IS1duTm5ZNm5NdURuZmxmbnE5eDNWLXhMY3lWdUJ2; entryOrigin=VidPg-premVid; tj_UUID=3ba5ccd9e9fa4cc2beb31183ebb9c5d0; tj_UUID_v2=3ba5ccd9-e9fa-4cc2-beb3-1183ebb9c5d0; ua=3cb58e121a2f27d6bb20540d2d97f17b; _gid=GA1.2.1248263834.1674155725; _ga=GA1.1.1591339880.1673199438; _gat=1; _ga_B39RFFWGYY=GS1.1.1674155724.5.1.1674157995.0.0.0\r\n";
            nums[3] = "X -Requested-With: XMLHttpRequest";
            nums[4] = "sec -ch-ua: \"Google Chrome\";v=\"107\", \"Chromium\";v=\"107\", \"=Not?A_Brand\";v=\"24\"";
            nums[5] = "sec -ch-ua-arch: ";
            nums[6] = "sec -ch-ua-full-version: \"107.0.0.0\"";
            nums[7] = "sec -ch-ua-mobile: ?1";
            nums[8] = "sec -ch-ua-platform: \"Android\"";
            nums[9] = "sec -ch-ua-platform-version: \"11.0.0\"";
            nums[10] = "Referer:https://rt.pornhub.com";
            nums[11] = "Connection: keep-alive";
            string url =
                "https://rt.pornhub.com/chat/index";
            var resultHttpGet = ZennoPoster.HttpGet(
                url,
                "",
                "UTF-8",
                ZennoLab.InterfacesLibrary.Enums.Http.ResponceType.HeaderAndBody,
                5000,
                "",
                project.Profile.UserAgent,
                true,
                5,
                nums,
                "",
                true);



            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(resultHttpGet);
            HtmlNodeCollection htmlNodeCollection =
                document.DocumentNode.SelectNodes("//*[@id=\"threadList\"]/li[1]/div[3]/div[2]");
            
            foreach (var htmlNode in htmlNodeCollection)
            {
                string name = htmlNode.InnerText.Split('\\')[0].ToLower().Replace(" ", "-");
                UserList.Add(name);
            }
            int Counter = 2;
            while (true)
            {

                project.SendInfoToLog("Страница номер: " + Counter, true);
                try
                {
                    url = String.Format("https://rt.pornhub.com/user/friend_requests_ajax?page={0}&sent=1", Counter);
                    var resultHttpGet2 = ZennoPoster.HttpGet(
                        url,
                        "",
                        "UTF-8",
                        ZennoLab.InterfacesLibrary.Enums.Http.ResponceType.BodyOnly,
                        5000,
                        "",
                        project.Profile.UserAgent,
                        true,
                        5,
                        nums,
                        "",
                        true);


                    document = new HtmlDocument();
                    document.LoadHtml(resultHttpGet2);
                    HtmlNodeCollection htmlNodeCollection2 =
                        document.DocumentNode.SelectNodes("//span[contains(@class, 'usernameBadgesWrapper')] ");


                    if (htmlNodeCollection2 == null)
                    {
                        break;
                    }

                    foreach (var htmlNode in htmlNodeCollection2)
                    {
                        string name = htmlNode.InnerText.Split('\\')[0].ToLower().Replace(" ", "-");
                        UserList.Add(name);
                    }

                    Counter++;
                    Thread.Sleep(500);
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
            project.SendInfoToLog("Закончили парсить отправленные заявки в друзья.", true);
            return UserList;
        }
    }
}
