namespace TasksAndJobLists.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Extended : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobEntities",
                c => new
                    {
                        JobEntityId = c.Int(nullable: false, identity: true),
                        ParentTaskEntity = c.Int(nullable: false),
                        Position = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        lastEdited = c.String(),
                    })
                .PrimaryKey(t => t.JobEntityId);
            
            DropTable("dbo.Jobs");
        }
        
        public override void Down()
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
            
            DropTable("dbo.JobEntities");
        }
    }
}
