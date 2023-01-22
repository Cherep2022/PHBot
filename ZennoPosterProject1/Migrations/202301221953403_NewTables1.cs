namespace ZennoPosterProject1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTables1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FriendsInContest", "UserName", c => c.String(maxLength: 150));
            AlterColumn("dbo.UsersContestNotFriends", "UserName", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UsersContestNotFriends", "UserName", c => c.String(maxLength: 50));
            AlterColumn("dbo.FriendsInContest", "UserName", c => c.String(maxLength: 50));
        }
    }
}
