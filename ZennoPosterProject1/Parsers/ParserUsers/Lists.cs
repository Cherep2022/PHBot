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

        public Lists(IZennoPosterProjectModel project)
        {
            new ParsingContestUsers(project).StartContestParsing();
            new ParsingFriends(project).StartFriendParsing();
            new ParsingFriendsNotAccepted(project).StartFriendsNoAcceptedParsing();
        }

    }
}
