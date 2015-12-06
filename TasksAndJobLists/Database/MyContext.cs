using System.Data.Entity;
using TasksAndJobLists.Models;

namespace TasksAndJobLists
{
    public class MyContext : DbContext
    {
        public MyContext()
            : base("name=TasksAndJobsDataBase")
        {

        }
        public DbSet<JobEntity> JobEntities { get; set; }
        public DbSet<TaskEntity> TasksEntities { get; set; }
    }
}
