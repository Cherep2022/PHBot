using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZennoLab.CommandCenter;
using ZennoLab.InterfacesLibrary.ProjectModel;
using ZennoPosterProject1.Parsers.ParserUsers;

namespace UserParser.Parsers.ListGenerator
{
    public class UsersContestNotFriends
    {
        readonly IZennoPosterProjectModel project;
        private readonly Instance instance;

        public UsersContestNotFriends(IZennoPosterProjectModel project, Instance instance)
        {
            this.project = project;
            this.instance = instance;
        }
        public List<string> GenerateUsersContestNotFriendsList(List<string> UsersContestList, List<string> UserFriendsList, List<string> UserFriendsNotAcceptedList)
        {
            project.SendInfoToLog("Генерируем лист участников конкурса которых нет в друзьях.", true);
            int counter = 1;
            

            var ListA = UsersContestList;
            var ListB = UserFriendsList;
            var ListC = new List<string>();
            var ListD = UserFriendsNotAcceptedList;

            foreach (var user in ListD)
            {
                ListB.Add(user);
            }

            foreach (var item in ListA)
            {
                foreach (var item2 in ListB)
                {
                    if (item2 == item)
                    {
                        ListC.Add(item);
                    }
                }
            }
            foreach (var item in ListC)
            {
                ListA.Remove(item);
            }
            ListC.Clear();

            foreach (var VARIABLE in ListA)
            {
                ListC.Add("https://rt.pornhub.com/model/" + VARIABLE);
                counter++;
            }
            project.SendInfoToLog("Закончили.", true);


            string path = project.Directory + @"/Result/UsersInContestNotFriends.txt";

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllLines(path, ListC, Encoding.UTF8);
            project.SendInfoToLog("Закончили.", true);
            return ListC;
        }
    }
}