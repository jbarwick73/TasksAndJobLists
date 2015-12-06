
namespace TasksAndJobLists.Models
{
    public class TaskEntity
    {
        public int TaskEntityId { get; set; }
        public int ParentTaskEntity { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Position { get; set; }
        public string LastEditedDate { get; set; }
    }
}
