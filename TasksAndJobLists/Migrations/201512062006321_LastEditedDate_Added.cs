namespace TasksAndJobLists.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LastEditedDate_Added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobEntities", "LastEditedDate", c => c.String());
            AddColumn("dbo.TaskEntities", "LastEditedDate", c => c.String());
            DropColumn("dbo.JobEntities", "lastEdited");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobEntities", "lastEdited", c => c.String());
            DropColumn("dbo.TaskEntities", "LastEditedDate");
            DropColumn("dbo.JobEntities", "LastEditedDate");
        }
    }
}
