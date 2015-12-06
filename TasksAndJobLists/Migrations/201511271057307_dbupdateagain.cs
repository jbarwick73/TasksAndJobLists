namespace TasksAndJobLists.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbupdateagain : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaskJobs",
                c => new
                    {
                        TaskJobId = c.Int(nullable: false, identity: true),
                        CreatedOn = c.String(),
                        IsChecked = c.Boolean(nullable: false),
                        ParentTask = c.Int(nullable: false),
                        Position = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.TaskJobId);
            
            DropTable("dbo.Jobs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        JobId = c.Int(nullable: false, identity: true),
                        CreatedOn = c.String(),
                        IsChecked = c.Boolean(nullable: false),
                        ParentTask = c.Int(nullable: false),
                        Position = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.JobId);
            
            DropTable("dbo.TaskJobs");
        }
    }
}
