using System;
using System.ComponentModel;
using TasksAndJobLists.Models;

namespace TasksAndTaskLists.Models
{
    /// <summary>
    /// Application internal Task Model.
    /// </summary>
    public class TaskItem : ViewModelBase, IComparable
    {
        int _position;
        bool _isChecked;
        string _description;

        public int Id { get; set; }
        public string CreatedOn { get; set; }

        public int ParentTaskEntity { get; set; }

        public int Position
        {
            get { return _position; }
            set
            {
                _position = value;
                // Call OnPropertyChanged whenever the property is updated
                Notify("Position");
            }
        }
        private string _title;
        public string Title {
            get
            {
                return _title;
            }
            set
          {
                _title = value;
                Notify("Title");
                IsDirty = true;
            }
        }
        public bool Edit { get; set; }
        public string Description
        {
            get { return _description; }

            set
            {
                _description = value;
                Notify("Description");
                IsDirty = true;
            }
        }

        #region CompareTo Implementation
        public int CompareTo(object obj)
        {
            TaskItem Task = obj as TaskItem;
            if (Task == null)
            {
                throw new ArgumentException("Object is not a Task");
            }
            return this.Position.CompareTo(Task.Position);
        }
        #endregion
    }
}
