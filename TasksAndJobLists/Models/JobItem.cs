using System;
using System.ComponentModel;

namespace TasksAndJobLists.Models
{
    /// <summary>
    /// Application internal Job Model.
    /// </summary>
    public class JobItem : ViewModelBase, IComparable
    {
        int _position;
        bool _isChecked;
        string _description;
        string _Title;
        public int JobId { get; set; }
        public string CreatedOn { get; set; }
        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                _isChecked = value;
                Notify("IsChecked");
            }
        }
        public string hasDescription
        {
            get
            {
                if (!this.Description.Equals("<FlowDocument PagePadding=\"5, 0, 5, 0\" AllowDrop=\"True\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Paragraph></Paragraph></FlowDocument>"))
                {
                    return "Read More ..";
                }
                return "";
            }
        }
        public int ParentTaskÊntity { get; set; }
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
        
        public string Title
        {
            get { return _Title; }

            set
            {
                _Title = value;
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
            JobItem job = obj as JobItem;
            if (job == null)
            {
                throw new ArgumentException("Object is not a Job");
            }
            return this.Position.CompareTo(job.Position);
        }
        #endregion
    }
}
