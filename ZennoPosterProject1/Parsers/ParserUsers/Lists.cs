using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parsers;
using ZennoLab.InterfacesLibrary.ProjectModel;

namespace ZennoPosterProject1.Parsers.ParserUsers
{
    public class Lists
    {
        readonly IZennoPosterProjectModel project;
        public List<string> UsersInContest { get; set; }
        public List<string> Friends{ get; set; }
        public List<string> FriendsNotAccepted { get; set; }

        public Lists(IZennoPosterProjectModel project)
        {
            this.UsersInContest = new ParsingContestUsers(project).StartContestParsing();
            this.Friends = new ParsingFriends(project).StartFriendParsing();
            this.FriendsNotAccepted = new ParsingFriendsNotAccepted(project).StartFriendsNoAcceptedParsing();
        }

    }
}
