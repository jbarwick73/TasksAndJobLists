namespace TasksAndJobLists.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobModelUpdated : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Jobs", "CreatedOn", c => c.String());
            DropColumn("dbo.Jobs", "JobFinishedTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Jobs", "JobFinishedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Jobs", "CreatedOn", c => c.DateTime(nullable: false));
        }
    }
}
