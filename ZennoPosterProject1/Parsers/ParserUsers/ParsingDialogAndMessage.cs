using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ZennoLab.CommandCenter;
using ZennoLab.InterfacesLibrary.ProjectModel;
using System.Text.RegularExpressions;
using ZennoPosterProject1.DataBase.Model;
using ZennoPosterProject1.DataBase.Tables;

namespace ZennoPosterProject1.Parsers.ParserUsers
{
    internal class ParsingDialogAndMessage
    {
        readonly IZennoPosterProjectModel project;
        private readonly Instance instance;
        private List<string> UserList;
        private string[] Nums;
        private string Url;
        private string ResultHttpGet;

        public ParsingDialogAndMessage(IZennoPosterProjectModel project,Instance instance)
        {
            this.project = project;
            this.instance = instance;
            this.UserList = new List<string>();
            this.Nums = new string[12];
        }
        public void StartDialogAndMessageParsing()
        {
            project.SendInfoToLog("Парсим диалоги.", true);
            project.SendInfoToLog("Страница номер: 1", true);

            Nums[0] = "Accept: text / html,application / xhtml + xml,application / xml; q = 0.9,image / avif,image / webp,image / apng,*/*; q = 0.8,application / signed - exchange; v = b3; q = 0.9";
            Nums[1] = "Accept-Language: ru-RU,ru;q=0.7,en-US;q=0.5,en;q=0.2";
            Nums[2] = "Cookie: bs=3qu09xwiz4lqympnoaui4d4khmzqmeen; ss=309682650025913921; fg_0d2ec4cbd943df07ec161982a603817e=8721.100000; hasVisited=1; __s=63BAFF4B-42FE722901BBD98D-4BB4A0CA; __l=63BAFF4B-42FE722901BBD98D-4BB4A0CA; atatusScript=hide; d_fs=1; d_uidb=f8d6f3d6-4a18-a0d9-0a00-a8f34dc08d4f; local_storage=1; il=v1a14LhYPrvL7RNOa4S-sNy4wiWWq5uona2HYrP1O0A2gxNjgwOTc1NTgwVzlzX0JOUUJrcmlxYW5RdlVHUGtHNGpSY0VrVmJXRFktQThmZUFlNg..; expiredEnterModalShown=1; platform=mobile; age_verified=v1KaMXojGHCj5s3FLLUvV58sZUx6O6QLOe0cAtdHn7n6sxNjc0NjY1NTQwdmsxLmEuOUdxTGxmTEFGSGNvVHllLU1ielFScC1KOXR0MnF3OVViWmlPNzhfWG9xa2JLdEstN3M3Wm90R0pObEF6aWRMZHRqRjdlZkoxQ3o4Nl9RYlI5RUpjaEp4RUE4UHpjVzZCVThoMDlKM0ZBcWxWMUo1RTkyWnQzMHpya0E0dTJpMDhDYkM2eVZHU25BclctTDRGWUZfV2FpN19LUmQ2cVR6MW9IS1duTm5ZNm5NdURuZmxmbnE5eDNWLXhMY3lWdUJ2; entryOrigin=VidPg-premVid; tj_UUID=3ba5ccd9e9fa4cc2beb31183ebb9c5d0; tj_UUID_v2=3ba5ccd9-e9fa-4cc2-beb3-1183ebb9c5d0; ua=3cb58e121a2f27d6bb20540d2d97f17b; _gid=GA1.2.1248263834.1674155725; _ga=GA1.1.1591339880.1673199438; _gat=1; _ga_B39RFFWGYY=GS1.1.1674155724.5.1.1674157995.0.0.0\r\n";
            Nums[3] = "X -Requested-With: XMLHttpRequest";
            Nums[4] = "sec -ch-ua: \"Google Chrome\";v=\"107\", \"Chromium\";v=\"107\", \"=Not?A_Brand\";v=\"24\"";
            Nums[5] = "sec -ch-ua-arch: ";
            Nums[6] = "sec -ch-ua-full-version: \"107.0.0.0\"";
            Nums[7] = "sec -ch-ua-mobile: ?1";
            Nums[8] = "sec -ch-ua-platform: \"Android\"";
            Nums[9] = "sec -ch-ua-platform-version: \"11.0.0\"";
            Nums[10] = "Referer:https://rt.pornhub.com";
            Nums[11] = "Connection: keep-alive";
            Url = "https://rt.pornhub.com/chat/index";


            ResultHttpGet = ZennoPoster.HttpGet(Url, "", "UTF-8", ZennoLab.InterfacesLibrary.Enums.Http.ResponceType.HeaderAndBody, 5000, "", project.Profile.UserAgent, true, 5, Nums, "", true);


            var UserNameRegex = "(?<=class=\\\\\"usernameBadgesWrapper\\\\\"><a\\ rel=\\\\\"rel=\\\\\"nofollow\\\\\"\\\\\"\\ href=\\\\\"\\\\/model\\\\/).*?(?=\\\\\"\\ \\ title=\\\\\")";
            MatchCollection UserNameCol = Regex.Matches(ResultHttpGet, UserNameRegex);

            var UserLastMessageRegex = "(?<=\",\"lastMessageSent\":\").*?(?=\",\"hasTransaction\":false,\")";
            MatchCollection UserLastMessageCol = Regex.Matches(ResultHttpGet, UserLastMessageRegex);

            var DateLastMessageRegex = "(?<=\",\"lastAction\":\").*?(?=\",\"lastActionNice\":\")";
            MatchCollection DateLastMessageRegexCol = Regex.Matches(ResultHttpGet, DateLastMessageRegex);

            
            using (PornHubDb dataBaseModelContainer = new PornHubDb())
            {
                dataBaseModelContainer.Database.ExecuteSqlCommand("TRUNCATE TABLE [AccountDialog]");
                for (int i = 0; i < UserNameCol.Count; i++)
                {
                    AccountDialog accountAccountDialog = new AccountDialog();
                    accountAccountDialog.UserName = UserNameCol[i].ToString();
                    accountAccountDialog.UserLastMessage = UserLastMessageCol[i].ToString();
                    accountAccountDialog.LastMassageDate = DateLastMessageRegexCol[i].ToString();
                    dataBaseModelContainer.AccountDialogs.Add(accountAccountDialog);
                }
                dataBaseModelContainer.SaveChanges();
            }


            int Counter = 20;
            while (true)
            {
                project.SendInfoToLog("Страница номер: " + Counter, true);
                try
                {
                    Url = String.Format("https://rt.pornhub.com/chat/threads?offset={0}&limit=20", Counter);
                    var ResultHttpGet = ZennoPoster.HttpGet(Url, "", "UTF-8", ZennoLab.InterfacesLibrary.Enums.Http.ResponceType.BodyOnly, 5000, "", project.Profile.UserAgent, true, 5, Nums, "", true);

                    UserLastMessageRegex = "(?<=lastMessageSent\":\").*?(?=\",\")";

                    UserNameCol = Regex.Matches(ResultHttpGet, UserNameRegex);

                    UserLastMessageCol = Regex.Matches(ResultHttpGet, UserLastMessageRegex);

                    DateLastMessageRegexCol = Regex.Matches(ResultHttpGet, DateLastMessageRegex);

                    if (UserNameCol.Count == 0)
                    {
                        break;
                    }

                    using (PornHubDb dataBaseModelContainer = new PornHubDb())
                    {
                        for (int i = 0; i < UserNameCol.Count; i++)
                        {
                            AccountDialog accountAccountDialog = new AccountDialog();
                            accountAccountDialog.UserName = UserNameCol[i].ToString();
                            accountAccountDialog.UserLastMessage = UserLastMessageCol[i].ToString();
                            accountAccountDialog.LastMassageDate = DateLastMessageRegexCol[i].ToString();
                            dataBaseModelContainer.AccountDialogs.Add(accountAccountDialog);
                        }
                        dataBaseModelContainer.SaveChanges();
                    }

                    Counter += 20;
                    Thread.Sleep(500);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    break;
                }

            }




            //List<string> ListTemp = UserList.Distinct().ToList(); // создаем временный список
            //UserList.Clear(); // очищаем исходный список
            //UserList.AddRange(ListTemp); // из временного списка переносим в исходный список
            //project.SendInfoToLog("Закончили парсить отправленные заявки в друзья.", true);
            //return UserList;
        }
    }
}
