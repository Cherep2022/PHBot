namespace ZennoPosterProject1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountDialog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(maxLength: 50),
                        UserLastMessage = c.String(),
                        LastMassageDate = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.FriendsInContest",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.FriendNotAccepted",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.UsersContestNotFriends",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.UsersContest",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.UsersContest", new[] { "Id" });
            DropIndex("dbo.UsersContestNotFriends", new[] { "Id" });
            DropIndex("dbo.FriendNotAccepted", new[] { "Id" });
            DropIndex("dbo.FriendsInContest", new[] { "Id" });
            DropIndex("dbo.Friends", new[] { "Id" });
            DropIndex("dbo.AccountDialog", new[] { "Id" });
            DropTable("dbo.UsersContest");
            DropTable("dbo.UsersContestNotFriends");
            DropTable("dbo.FriendNotAccepted");
            DropTable("dbo.FriendsInContest");
            DropTable("dbo.Friends");
            DropTable("dbo.AccountDialog");
        }
    }
}
