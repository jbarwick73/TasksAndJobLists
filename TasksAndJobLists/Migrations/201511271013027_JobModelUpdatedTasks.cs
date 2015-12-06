namespace TasksAndJobLists.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobModelUpdatedTasks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "ParentTask", c => c.Int(nullable: false));
            DropColumn("dbo.Jobs", "ParentPosition");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Jobs", "ParentPosition", c => c.Int(nullable: false));
            DropColumn("dbo.Jobs", "ParentTask");
        }
    }
}
