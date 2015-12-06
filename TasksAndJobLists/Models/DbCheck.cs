namespace TasksAndJobLists.Models
{
    public class DbCheck
    {
        public bool Error { get; set; }
        public string ExceptionMessage { get; set; }
        public string InnerExceptionMessage { get; set; }
    }
}
