using System;
using System.Data.Entity;
using System.Linq;
using ZennoPosterProject1.DataBase.Tables;

namespace ZennoPosterProject1.DataBase.Model
{
    public class PornHubDb : DbContext
    {
        public PornHubDb()
            : base("ZennoPosterProject1.DataBase.Model.PornHubDb")
        {
            Database.SetInitializer<PornHubDb>(null);
        }
        public virtual DbSet<AccountDialog> AccountDialogs { get; set; }
        public virtual DbSet<AccountFriend> AccountFriends { get; set; }
        public virtual DbSet<FriendInContest> FriendInContests { get; set; }
        public virtual DbSet<UserContest> UserContests { get; set; }
        public virtual DbSet<UserContestNotFriend> UserContestNotFriends { get; set; }
        public virtual DbSet<FriendNotAccepted> FriendNotAccepteds { get; set; }

    }
}