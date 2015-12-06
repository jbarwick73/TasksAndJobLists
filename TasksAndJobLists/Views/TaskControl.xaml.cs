using EntityFramework.Extensions;
using FsWpfControls.FsRichTextBox;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TasksAndJobLists.Helpers;
using TasksAndJobLists.Models;
using TasksAndTaskLists.Models;

namespace TasksAndJobLists.Views
{
    /// <summary>
    /// Interaction logic for TaskControl.xaml
    /// </summary>
    public partial class TaskControl : UserControl
    {
        

        public TaskControl()
        {
            InitializeComponent();
        }

        private void button_add_Click(object sender, RoutedEventArgs e)
        {
            var newTask = new TaskItem();
            newTask.Title = "New Task";
            newTask.Description = "<FlowDocument PagePadding=\"5, 0, 5, 0\" AllowDrop=\"True\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Paragraph>Enter task description here</Paragraph></FlowDocument>";
            newTask.Position = ((ViewModel)DataContext).TaskItemCollection.Count + 1;

            ((ViewModel)DataContext).TaskItemCollection.Add(newTask);
            TaskEntity newTaskEntity = HelperMethods.TaskItemToTaskEntity(newTask);
            ((ViewModel)DataContext).context.TasksEntities.Add(newTaskEntity);
            ((ViewModel)DataContext).context.SaveChanges();
            LoadTasks();
        }

        private void button_delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteTask();
        }

        private void button_load_Click(object sender, RoutedEventArgs e)
        {
            LoadTasks();
        }

        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            SaveTasks();
            LoadTasks();
        }

        private void SaveTasks()
        {
            var newItem = ((ViewModel)DataContext).TaskItemCollection[TaskList.SelectedIndex];
            var oldItem = ((ViewModel)DataContext).context.TasksEntities.Where(a => a.TaskEntityId.Equals(newItem.Id)).First();
            oldItem.Description = newItem.Description;
            oldItem.Title = newItem.Title;
            ((ViewModel)DataContext).context.SaveChanges();
        }
       
        private void EditableTextBlock_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void EditableTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {          
            //warn/return if jobs have been checkedboxed.
            if (((ViewModel)DataContext).JobItemCollectionDone.Count > 0)
            {
                var answer = MessageBox.Show("You have checked jobs as done, do You want to discard them ?", "Unsaved changes", MessageBoxButton.YesNo);
                if (answer == MessageBoxResult.No)
                {
                    return;
                }
                ((ViewModel)DataContext).JobItemCollectionDone = new ObservableCollection<JobItem>();
            }

            //If unsaved changes are present, warn user.
            if (((ViewModel)DataContext).IsJoblistDirty)
            {
                var answer = MessageBox.Show("You have unsaved changes, do You want to discard them ?", "Unsaved changes", MessageBoxButton.YesNo);
                if (answer == MessageBoxResult.No)
                {
                    return;
                }
                ((ViewModel)DataContext).IsJoblistDirty = false;
            }

            var thisId = ((TaskItem)((EditableTextBlock)sender).DataContext).Id;
            ((ViewModel)DataContext).CurrentTask = ((ViewModel)DataContext).TaskItemCollection.Where(a => a.Id.Equals(thisId)).First();
            var theCollection = HelperMethods.ConvertJobEntitiesToJobItems(((ViewModel)DataContext).context.JobEntities.Where(a => a.ParentTaskEntity.Equals(((ViewModel)DataContext).CurrentTask.Id)).OrderBy(a => a.Position).ToList());
            ((ViewModel)DataContext).JobItemCollection = theCollection;
            ((ViewModel)DataContext).HasTaskSelected = true;


        }

        private void LoadTasks()
        {
            
            ((ViewModel)DataContext).TaskItemCollection = HelperMethods.ConvertTaskEntitiesToTaskItems(((ViewModel)DataContext).context.TasksEntities.AsEnumerable().ToList());
            ((ViewModel)DataContext).IsLoading = System.Windows.Visibility.Hidden;
            ((ViewModel)DataContext).IsTasklistDirty = false;
        }

        private void TaskList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteTask();
            }
        }

        void DeleteTask()
        {
            int taskId = 0;
            if (TaskList.SelectedIndex == -1)
            {
                //Get task id of deleting task.
                taskId = ((ViewModel)DataContext).TaskItemCollection[TaskList.Items.Count-1].Id;

            }
            else
            {
                //Get task id of last task in list.
                taskId = ((ViewModel)DataContext).TaskItemCollection[TaskList.SelectedIndex].Id;

            }
            if (((ViewModel)DataContext).JobItemCollection.Count > 0 && TaskList.SelectedIndex != -1)
            {
                var answer = MessageBox.Show("Deleting task '" + ((ViewModel)DataContext).TaskItemCollection[TaskList.SelectedIndex].Title + "' will also delete all its " + ((ViewModel)DataContext).JobItemCollection.Count + " jobs, do you want that ?", "Unsaved changes", MessageBoxButton.YesNo);
                if (answer == MessageBoxResult.No)
                {
                    return;
                }
            }

        
            //Delete task.
            ((ViewModel)DataContext).context.TasksEntities.Where(a => a.TaskEntityId.Equals(taskId)).Delete();
            
            //Delete all jobs from deleted task.
            ((ViewModel)DataContext).context.JobEntities.Where(a => a.ParentTaskEntity.Equals(taskId)).Delete();
            ((ViewModel)DataContext).JobItemCollection = new ObservableCollection<JobItem>();

            LoadTasks();
            ((ViewModel)DataContext).IsTasklistDirty = false;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((ViewModel)DataContext).IsLoading = System.Windows.Visibility.Visible;

            //warn/return if jobs have been checkedboxed.
            if (((ViewModel)DataContext).JobItemCollectionDone.Count > 0)
            {
                var answer = MessageBox.Show("You have marked jobs as done, do You want to discard them ?", "Unsaved changes", MessageBoxButton.YesNo);
                if (answer == MessageBoxResult.No)
                {
                    return;
                }
                ((ViewModel)DataContext).JobItemCollectionDone = new ObservableCollection<JobItem>();
            }

            //If unsaved changes are present, warn user.
            if (((ViewModel)DataContext).IsJoblistDirty)
            {
                var answer = MessageBox.Show("You have unsaved changes, do You want to discard them ?", "Unsaved changes", MessageBoxButton.YesNo);
                if (answer == MessageBoxResult.No)
                {
                    return;
                }
                ((ViewModel)DataContext).IsJoblistDirty = false;
                ((ViewModel)DataContext).HasTaskSelected = true;
            }

            TaskItem thisTask = ((Grid)sender).DataContext as TaskItem;
            var thisId = thisTask.Id;
            ((ViewModel)DataContext).CurrentTask = ((ViewModel)DataContext).TaskItemCollection.Where(a => a.Id.Equals(thisId)).First();
            var theCollection = HelperMethods.ConvertJobEntitiesToJobItems(((ViewModel)DataContext).context.JobEntities.Where(a => a.ParentTaskEntity.Equals(((ViewModel)DataContext).CurrentTask.Id)).OrderBy(a => a.Position).ToList());
            ((ViewModel)DataContext).JobItemCollection = theCollection;
            ((ViewModel)DataContext).IsLoading = System.Windows.Visibility.Hidden;
        }

        private void MyGreatEditableTextBlock_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            ((FsRichTextBox)sender as FsRichTextBox).UpdateDocumentBindings();
            SaveTasks();

        }
    }
}
