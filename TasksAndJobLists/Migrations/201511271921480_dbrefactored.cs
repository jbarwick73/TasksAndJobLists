namespace TasksAndJobLists.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbrefactored : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        JobId = c.Int(nullable: false, identity: true),
                        ParentTask = c.Int(nullable: false),
                        Position = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        lastEdited = c.String(),
                    })
                .PrimaryKey(t => t.JobId);
            
            DropTable("dbo.SavedJobs");
        }
        
        public override void Down()
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
            
            DropTable("dbo.Jobs");
        }
    }
}
