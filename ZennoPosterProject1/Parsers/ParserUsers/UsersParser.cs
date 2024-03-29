﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserParser.Parsers.ListGenerator;
using ZennoLab.CommandCenter;
using ZennoLab.InterfacesLibrary.ProjectModel;

namespace ZennoPosterProject1.Parsers.ParserUsers
{
    internal class UsersParser
    {
        readonly IZennoPosterProjectModel project;
        private readonly Instance instance;

        private Lists UserList;
        private FriendsInContest friendsInContest;
        private UsersContestNotFriends usersContestNotFriends;

        public UsersParser(IZennoPosterProjectModel project, Instance instance)
        {
            this.instance = instance;
            this.project = project;
            this.UserList = new Lists(project);
            this.friendsInContest = new FriendsInContest(project);
            this.usersContestNotFriends = new UsersContestNotFriends(project,instance);
            
        }

        public void StartListGenerator()
        { 
            friendsInContest.GenerateFriendsInContestList();
            usersContestNotFriends.GenerateUsersContestNotFriendsList();
        }
    }
}
