namespace TasksAndJobLists.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        JobId = c.Int(nullable: false, identity: true),
                        IsChecked = c.Boolean(nullable: false),
                        Position = c.Int(nullable: false),
                        Title = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        ParentPosition = c.Int(nullable: false),
                        Description = c.String(),
                        JobFinishedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.JobId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Jobs");
        }
    }
}
