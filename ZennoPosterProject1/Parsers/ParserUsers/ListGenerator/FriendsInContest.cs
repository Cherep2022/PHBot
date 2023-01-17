using Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZennoLab.InterfacesLibrary.ProjectModel;
using ZennoPosterProject1.Parsers.ParserUsers;

namespace UserParser.Parsers.ListGenerator
{
    public class FriendsInContest
    {
        readonly IZennoPosterProjectModel project;

        public FriendsInContest(IZennoPosterProjectModel project)
        {
            this.project = project;
        }
        public List<string> GenerateFriendsInContestList(List<string> UsersContestList, List<string> UserFriendsList)
        {
            project.SendInfoToLog("Генерируем лист друзей участвующих в конкурсе.", true);
            var ListOutput = new List<string>();
            int counter = 1;
            foreach (var Friend in UserFriendsList)
            {
                foreach (var Contest in UsersContestList)
                {
                    if (Friend.ToLower() == Contest.ToLower())
                    {
                        ListOutput.Add("https://rt.pornhub.com/model/" + Friend);
                        counter++;
                    }
                }
            }
            string path = "FriendsInContest.html";

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllLines(path, ListOutput, Encoding.UTF8);
            return ListOutput;
            project.SendInfoToLog("Закончили.", true);
        }
    }
}