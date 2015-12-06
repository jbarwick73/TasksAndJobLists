
namespace TasksAndJobLists.Models
{
    /// <summary>
    /// EF Job Model for database usage.
    /// </summary>
    public class JobEntity
        {
            public int JobEntityId { get; set; }
            public int ParentTaskEntity { get; set; }
            public int Position { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string LastEditedDate { get; set; }
    }
}
