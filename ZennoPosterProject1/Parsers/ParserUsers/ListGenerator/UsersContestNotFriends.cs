using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZennoLab.CommandCenter;
using ZennoLab.InterfacesLibrary.ProjectModel;
using ZennoPosterProject1.DataBase.Model;
using ZennoPosterProject1.DataBase.Tables;
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
        public void GenerateUsersContestNotFriendsList()
        {
            List<string> UserFriendsList = new List<string>();
            using (PornHubDb dataBaseModelContainer = new PornHubDb())
            {
                foreach (var VARIABLE in dataBaseModelContainer.AccountFriends)
                {
                    UserFriendsList.Add(VARIABLE.UserName);
                }
            }

            List<string> UsersContestList = new List<string>();
            using (PornHubDb dataBaseModelContainer = new PornHubDb())
            {
                foreach (var VARIABLE in dataBaseModelContainer.UserContests)
                {
                    UsersContestList.Add(VARIABLE.UserName);
                }
            }

            List<string> UserFriendsNotAcceptedList = new List<string>();
            using (PornHubDb dataBaseModelContainer = new PornHubDb())
            {
                foreach (var VARIABLE in dataBaseModelContainer.FriendNotAccepteds)
                {
                    UserFriendsNotAcceptedList.Add(VARIABLE.UserName);
                }
            }

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


            using (PornHubDb dataBaseModelContainer = new PornHubDb())
            {
                dataBaseModelContainer.Database.ExecuteSqlCommand("TRUNCATE TABLE [UsersContestNotFriends]");
                foreach (var htmlNode in ListC)
                {
                    UserContestNotFriend userContestNotFriend = new UserContestNotFriend();
                    userContestNotFriend.UserName = htmlNode.ToLower().Replace(" ", "-");
                    dataBaseModelContainer.UserContestNotFriends.Add(userContestNotFriend);
                }
                dataBaseModelContainer.SaveChanges();
            }


            project.SendInfoToLog("Закончили.", true);

        }
    }
}