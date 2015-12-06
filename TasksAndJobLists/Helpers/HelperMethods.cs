using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Markup;
using TasksAndJobLists.Models;
using TasksAndTaskLists.Models;

namespace TasksAndJobLists.Helpers
{
    public static class HelperMethods
    {
        public static DbCheck CheckDB(MyContext context)
        {
            var rslt = new DbCheck();
            rslt.Error = false;
            var csb = new SqlConnectionStringBuilder(context.Database.Connection.ConnectionString);
            csb.ConnectTimeout = 5;
            try
            {
                var c = new SqlConnection(csb.ToString());
                c.Open();
            }
            catch (Exception ex)
            {
                rslt.Error = true;
                rslt.ExceptionMessage = ex.Message;
                rslt.InnerExceptionMessage = ex.InnerException.Message;
            }
            return rslt;
        }

        //Converts a List<TaskEntity> to ObservableCollection<TaskItem> for application internal use.
        public static ObservableCollection<TaskItem> ConvertTaskEntitiesToTaskItems(List<TaskEntity> SavedTasks)
        {
            var rslt = new ObservableCollection<TaskItem>();
            foreach (var item in SavedTasks)
            {
                TaskItem newitem = new TaskItem();
                newitem.Title = item.Title;
                newitem.Description = item.Description;
                newitem.Position = item.Position;
                newitem.ParentTaskEntity = item.ParentTaskEntity;
                newitem.Id = item.TaskEntityId;
                rslt.Add(newitem);
            }
            return rslt;
        }

        //Converts a List<TaskEntity> to ObservableCollection<TaskItem> for application internal use.
        public static ObservableCollection<JobItem> ConvertJobEntitiesToJobItems(List<JobEntity> SavedJobs)
        {
            var rslt = new ObservableCollection<JobItem>();
            foreach (var item in SavedJobs)
            {
                var newitem = new JobItem();
                newitem.Title = item.Title;
                newitem.Description = item.Description;
                newitem.Position = item.Position;
                newitem.ParentTaskÊntity = item.ParentTaskEntity;
                newitem.JobId = item.JobEntityId;
                rslt.Add(newitem);
            }
            return rslt;
        }

        public static TaskEntity TaskItemToTaskEntity(TaskItem item)
        {
            var rslt = new TaskEntity();
            rslt.Description = item.Description;
            rslt.ParentTaskEntity = item.ParentTaskEntity;
            rslt.Position = item.Position;
            rslt.Title = item.Title;
            rslt.TaskEntityId = item.Id;

            return rslt;
        }

        public static JobEntity JobItemToJobEntity(JobItem item)
        {
            var rslt = new JobEntity();
            rslt.Description = item.Description;
            rslt.ParentTaskEntity = item.ParentTaskÊntity;
            rslt.Position = item.Position;
            rslt.Title = item.Title;
            rslt.JobEntityId = item.JobId;
            return rslt;
        }

        public static FlowDocument SetRTF(string xamlString)
        {
            StringReader stringReader = new StringReader(xamlString);
            System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(stringReader);
            Section sec = XamlReader.Load(xmlReader) as Section;
            FlowDocument doc = new FlowDocument();
            while (sec.Blocks.Count > 0)
            {
                var block = sec.Blocks.FirstBlock;
                sec.Blocks.Remove(block);
                doc.Blocks.Add(block);
            }
            return doc;
        }

        public static ObservableCollection<TaskItem> MoveTask(ObservableCollection<TaskItem> TaskItemCollection, TaskItem source, int sourceIndex, int targetIndex)
        {
            if (sourceIndex < targetIndex)
            {
                TaskItemCollection.Insert(targetIndex + 1, source);
                TaskItemCollection.RemoveAt(sourceIndex);
            }
            else
            {
                int removeIndex = sourceIndex + 1;
                if (TaskItemCollection.Count + 1 > removeIndex)
                {
                    TaskItemCollection.Insert(targetIndex, source);
                    TaskItemCollection.RemoveAt(removeIndex);
                }
            }
            return TaskItemCollection;
        }

        public static ObservableCollection<JobItem> MoveJob(ObservableCollection<JobItem> JobItemCollection, JobItem source, int sourceIndex, int targetIndex)
        {
            if (sourceIndex < targetIndex)
            {
                JobItemCollection.Insert(targetIndex + 1, source);
                JobItemCollection.RemoveAt(sourceIndex);
            }
            else
            {
                int removeIndex = sourceIndex + 1;
                if (JobItemCollection.Count + 1 > removeIndex)
                {
                    JobItemCollection.Insert(targetIndex, source);
                    JobItemCollection.RemoveAt(removeIndex);
                }
            }
            return JobItemCollection;
        }

        public static ObservableCollection<JobItem> RenumberJobCollection(ObservableCollection<JobItem> JobItemCollection)
        {
                for (int i = 0; i < JobItemCollection.Count; i++)
                {
                    JobItemCollection[i].Position = i + 1;
                }
            return JobItemCollection;
        }

        public static ObservableCollection<TaskItem> RenumberTaskCollection(ObservableCollection<TaskItem> TaskItemCollection)
        {
            for (int i = 0; i < TaskItemCollection.Count; i++)
            {
                TaskItemCollection[i].Position = i + 1;
            }
            return TaskItemCollection;
        }

    }
}
