
using System.Collections.Generic;
using ZennoLab.InterfacesLibrary.ProjectModel;
using ZennoPosterProject1.DataBase.Model;
using ZennoPosterProject1.DataBase.Tables;

namespace UserParser.Parsers.ListGenerator
{
    public class FriendsInContest
    {
        readonly IZennoPosterProjectModel project;

        public FriendsInContest(IZennoPosterProjectModel project)
        {
            this.project = project;
        }
        public void GenerateFriendsInContestList()
        {
            project.SendInfoToLog("Генерируем лист друзей участвующих в конкурсе.", true);
            var ListOutput = new List<string>();
            int counter = 1;

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





            foreach (var Friend in UserFriendsList)
            {
                foreach (var Contest in UsersContestList)
                {
                    if (Friend.ToLower() == Contest.ToLower())
                    {
                        ListOutput.Add( "https://rt.pornhub.com/model/" + Friend);
                        counter++;
                    }
                }
            }

            using (PornHubDb dataBaseModelContainer = new PornHubDb())
            {
                dataBaseModelContainer.Database.ExecuteSqlCommand("TRUNCATE TABLE [FriendsInContest]");
                foreach (var htmlNode in ListOutput)
                {
                    FriendInContest friendInContest = new FriendInContest();
                    friendInContest.UserName = htmlNode.ToLower().Replace(" ", "-");
                    dataBaseModelContainer.FriendInContests.Add(friendInContest);
                }
                dataBaseModelContainer.SaveChanges();
            }


            project.SendInfoToLog("Закончили.", true);
        }
    }
}