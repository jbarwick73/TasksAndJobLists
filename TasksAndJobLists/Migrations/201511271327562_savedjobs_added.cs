namespace TasksAndJobLists.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class savedjobs_added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SavedJobs",
                c => new
                    {
                        SavedJobId = c.Int(nullable: false, identity: true),
                        ParentTask = c.Int(nullable: false),
                        Position = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.SavedJobId);
            
            DropTable("dbo.TaskJobs");
        }
        
        public override void Down()
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
            
            DropTable("dbo.SavedJobs");
        }
    }
}
