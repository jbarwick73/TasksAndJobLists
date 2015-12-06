namespace TasksAndJobLists.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskEntityAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaskEntities",
                c => new
                    {
                        TaskEntityId = c.Int(nullable: false, identity: true),
                        ParentTaskEntity = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        Position = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TaskEntityId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TaskEntities");
        }
    }
}
