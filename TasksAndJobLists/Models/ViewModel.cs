using GongSolutions.Wpf.DragDrop;
using System.Collections.ObjectModel;
using TasksAndTaskLists.Models;
using System;
using System.Windows;
using TasksAndJobLists.Helpers;

namespace TasksAndJobLists.Models
{
    public class ViewModel : ViewModelBase, IDropTarget
    {

        public ViewModel()
        {
            JobItemCollection = new ObservableCollection<JobItem>();
            JobItemCollectionDone = new ObservableCollection<JobItem>();
            TaskItemCollection = new ObservableCollection<TaskItem>();
            _taskItemCollection = new ObservableCollection<TaskItem>();
            _jobItemCollection = new ObservableCollection<JobItem>();
            _currentTask = new TaskItem();
            _currentJobItem = new JobItem();
            PropertyChanged += ViewModel_PropertyChanged;
            _jobItemCollection.CollectionChanged += _jobItemCollection_CollectionChanged;
            _currentJobItem.PropertyChanged += _currentJobItem_PropertyChanged;
            _currentTask.PropertyChanged += _currentTask_PropertyChanged;
            
        }

        private void _currentTask_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            string bah = "";
        }

        private void _currentJobItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }

        private void _jobItemCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            isJobListDirty = true;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != null && e.PropertyName.ToString().Contains("Title"))
            {
                string propName = e.PropertyName.ToString();
            }


        }
        
        JobItem _currentJobItem;
        TaskItem _currentTask;
        private System.Windows.Visibility isLoading;

        private MyContext _context;
        private ObservableCollection<JobItem> _jobItemCollectionDone;
        private ObservableCollection<TaskItem> _taskItemCollection;
        private ObservableCollection<JobItem> _jobItemCollection;

        public MyContext context
        {
            get { return _context; }
            set { _context = value; }
        }

        public System.Windows.Visibility IsLoading
        {
            get
            {
                return isLoading;
            }
            set
            {
                isLoading = value;
                Notify("IsLoading");
            }
        }

        private bool edit;

        public bool Edit
        {
            get { return edit; }
            set
            {
                edit = value;
                Notify("Edit");
                if (value)
                {
                    IsJoblistDirty = true;
                }
            }
        }

        private bool hasTaskSelected;

        public bool HasTaskSelected
        {
            get { return hasTaskSelected; }
            set { hasTaskSelected = value; }
        }


        private bool isJobListDirty;

        public bool IsJoblistDirty
        {
            get
            {
                return isJobListDirty;
            }
            set
            {
                isJobListDirty = value;
                Notify("IsJoblistDirty");
            }
        }

        private bool isTaskListDirty;

        public bool HasJobsDone
        {
            get
            {
                if (_jobItemCollectionDone.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool IsTasklistDirty
        {
            get
            {
                return isTaskListDirty;
            }
            set
            {
                isTaskListDirty = value;
                Notify("IsTasklistDirty");
            }
        }



        /// <summary>
        /// Gets or sets the current.
        /// </summary>
        /// <value>The current.</value>
        public JobItem CurrentJobItem
        {
            get
            {
                return _currentJobItem;
            }
            set
            {
                _currentJobItem = value;
                Notify("CurrentJobItem");
            }
        }

        /// <summary>
        /// Gets or sets the job collection.
        /// </summary>
        /// <value>The person collection.</value>
        public ObservableCollection<JobItem> JobItemCollection
        {
            get
            {
                return _jobItemCollection;
            }
            set
            {
                _jobItemCollection = value;
                Notify("JobItemCollection");
            }
        }

        /// <summary>
        /// Gets or sets the job collection.
        /// </summary>
        /// <value>The person collection.</value>
        public ObservableCollection<JobItem> JobItemCollectionDone
        {
            get
            {
                return _jobItemCollectionDone;
            }
            set
            {
                _jobItemCollectionDone = value;
                Notify("JobItemCollectionDone");
            }
        }

        /// <summary>
        /// Gets or sets the current TaskItem.
        /// </summary>
        /// <value>The current TaskItem.</value>
        public TaskItem CurrentTask
        {
            get
            {
                return _currentTask;
            }
            set
            {
                _currentTask = value;
                Notify("CurrentTask");
                Notify("");
                Notify(null);
            }
        }

        /// <summary>
        /// Gets or sets the TaskItem collection.
        /// </summary>
        public ObservableCollection<TaskItem> TaskItemCollection
        {
            get
            {
                return _taskItemCollection;
            }
            set
            {
                _taskItemCollection = value;
                Notify("TaskItemCollection");
            }
        }

        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = DragDropEffects.Move;
            // test for handled
            dropInfo.NotHandled = true; // now the DefaultDropHandler should work
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            dropInfo.NotHandled = false; // now the DefaultDropHandler should work

            if (dropInfo.DragInfo.Data is TaskItem)
            {
                TaskItem taskItem = dropInfo.DragInfo.Data as TaskItem;
                TaskItemCollection = HelperMethods.RenumberTaskCollection(HelperMethods.MoveTask(TaskItemCollection, taskItem, dropInfo.DragInfo.SourceIndex, dropInfo.InsertIndex));
            }

            if (dropInfo.DragInfo.Data is JobItem)
            {
                if (JobItemCollectionDone.Count > 0)
                {
                    MessageBox.Show("Please uncheck all job items before reordering.");
                }
                else
                {
                    JobItem jobItem = dropInfo.DragInfo.Data as JobItem;
                    JobItemCollection = HelperMethods.RenumberJobCollection(HelperMethods.MoveJob(JobItemCollection, jobItem, dropInfo.DragInfo.SourceIndex, dropInfo.InsertIndex));
                    IsJoblistDirty = true;

                }
            }
        }
    }
}
