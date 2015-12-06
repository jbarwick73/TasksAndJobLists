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

namespace TasksAndJobLists.Views
{
    /// <summary>
    /// Interaction logic for JobControl.xaml
    /// </summary>
    public partial class JobControl : UserControl
    {
        public JobControl()
        {
            InitializeComponent();
        }

        #region button commands
        void button_add_Click(object sender, RoutedEventArgs e)
        {
            if (((ViewModel)DataContext).CurrentTask == null)
            {
                MessageBox.Show("Select a Task to add jobs first.");
                return;
            }
            if (((ViewModel)DataContext).JobItemCollectionDone.Count > 0)
            {
                MessageBox.Show("Uncheck all jobs first.");
            }
            else
            {
                JobItem taskJob = new JobItem();
                taskJob.Title = "New Job Title";
                taskJob.Description = "<FlowDocument PagePadding=\"5, 0, 5, 0\" AllowDrop=\"True\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Paragraph></Paragraph></FlowDocument>";
                taskJob.Position = ((ViewModel)DataContext).JobItemCollection.Count + 1;
                ((ViewModel)DataContext).JobItemCollection.Add(taskJob);
                ((ViewModel)DataContext).JobItemCollection = HelperMethods.RenumberJobCollection(((ViewModel)DataContext).JobItemCollection);



                //if (TaskjobList.SelectedIndex != -1)
                //{
                //    taskJob.Position = TaskjobList.SelectedIndex + 1;
                //    ((ViewModel)DataContext).JobItemCollection.Insert(TaskjobList.SelectedIndex + 1,taskJob);
                //    TaskjobList.SelectedIndex = -1;
                //}
                //else
                //{
                //    taskJob.Position = ((ViewModel)DataContext).JobItemCollection.Count + 1;
                //    ((ViewModel)DataContext).JobItemCollection.Add(taskJob);
                //    ((ViewModel)DataContext).JobItemCollection = HelperMethods.RenumberJobCollection(((ViewModel)DataContext).JobItemCollection);
                //}



                ((ViewModel)DataContext).IsJoblistDirty = true;
            }

            TaskjobList.SelectedIndex = TaskjobList.Items.Count;

        }

        private void button_load_Click(object sender, RoutedEventArgs e)
        {
            if (((ViewModel)DataContext).IsJoblistDirty)
            {
                var answer = MessageBox.Show("Discard changes?", "Unsaved changes", MessageBoxButton.YesNo);
                if (answer == MessageBoxResult.No)
                {
                    return;
                }
            }

            if (((ViewModel)DataContext).JobItemCollectionDone.Count > 0)
            {
                var answer = MessageBox.Show("Discard checked jobs?", "Checked jobs", MessageBoxButton.YesNo);
                if (answer == MessageBoxResult.No)
                {
                    return;
                }
            }
            if (((ViewModel)DataContext).CurrentTask == null)
            {
                MessageBox.Show("Select a Task first");
                return;
            }
            var list = HelperMethods.ConvertJobEntitiesToJobItems(((ViewModel)DataContext).context.JobEntities.Where(a => a.ParentTaskEntity.Equals(((ViewModel)DataContext).CurrentTask.Id)).AsEnumerable().OrderBy(a => a.Position).ToList());
            ((ViewModel)DataContext).JobItemCollection = list;
            ((ViewModel)DataContext).JobItemCollectionDone = new ObservableCollection<JobItem>();
            ((ViewModel)DataContext).IsJoblistDirty = false;

        }

        /// <summary>
        /// Saves the _items collection to  the database.
        /// saving is prevented as long as jobs are checked
        /// all jobs with given parenttask id are deleted and recreated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void button_save_Click(object sender, RoutedEventArgs e)
        {
            ((ViewModel)DataContext).JobItemCollection = HelperMethods.RenumberJobCollection(((ViewModel)DataContext).JobItemCollection);
            if (((ViewModel)DataContext).JobItemCollectionDone.Count > 0)
            {
                MessageBox.Show("Please uncheck all checked job items before saving.");
            }
            else
            {
                var saveTheJobs = new List<JobEntity>();
                foreach (var item in ((ViewModel)DataContext).JobItemCollection)
                {
                    var newJob = HelperMethods.JobItemToJobEntity(item);

                    var oldJob = new JobEntity();

                    if (((ViewModel)DataContext).context.JobEntities.Where(a => a.JobEntityId.Equals(newJob.JobEntityId)).Any())
                    {
                        oldJob = ((ViewModel)DataContext).context.JobEntities.Where(a => a.JobEntityId.Equals(newJob.JobEntityId)).First();
                        oldJob.JobEntityId = newJob.JobEntityId;
                        oldJob.ParentTaskEntity = newJob.ParentTaskEntity;
                        oldJob.Description = newJob.Description;
                        oldJob.Title = newJob.Title;
                        oldJob.Position = newJob.Position;
                        oldJob.LastEditedDate = DateTime.Now.ToString();
                        ((ViewModel)DataContext).context.Entry(oldJob).State = System.Data.Entity.EntityState.Modified;
                        ((ViewModel)DataContext).context.SaveChanges();
                    }
                    else
                    {
                        newJob.LastEditedDate = DateTime.Now.ToString();
                        newJob.ParentTaskEntity = ((ViewModel)DataContext).CurrentTask.Id;
                        ((ViewModel)DataContext).context.JobEntities.Add(newJob);
                        ((ViewModel)DataContext).context.SaveChanges();
                    }


                }
                ((ViewModel)DataContext).IsJoblistDirty = false;
            }
        }

        void button_delete_Click(object sender, RoutedEventArgs e)
        {
            if (((ViewModel)DataContext).JobItemCollectionDone.Count > 0)
            {
                MessageBox.Show("Please uncheck all checked job items before deleting.");
            }
            else
            {

                if (TaskjobList.SelectedIndex > -1)
                {
                    var thisItem = ((ViewModel)DataContext).JobItemCollection[TaskjobList.SelectedIndex];
                    ((ViewModel)DataContext).JobItemCollection.RemoveAt(TaskjobList.SelectedIndex);
                    ((ViewModel)DataContext).context.JobEntities.Where(a => a.JobEntityId.Equals(thisItem.JobId)).Delete();
                }
                else
                {
                    if (TaskjobList.Items.Count > 0)
                    {
                        var thisItem = ((ViewModel)DataContext).JobItemCollection[TaskjobList.Items.Count - 1];
                        ((ViewModel)DataContext).JobItemCollection.RemoveAt(TaskjobList.Items.Count - 1);
                        ((ViewModel)DataContext).context.JobEntities.Where(a => a.JobEntityId.Equals(thisItem.JobId)).Delete();
                    }
                }
                ((ViewModel)DataContext).IsJoblistDirty = true;
                ((ViewModel)DataContext).JobItemCollection = HelperMethods.RenumberJobCollection(((ViewModel)DataContext).JobItemCollection);
            }
        }

        void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Window win1 = new Window();
            win1.Width = 1024;
            win1.Content = new JobDetailsControl();

            ViewModel viewModel = new ViewModel();
            var position = ((JobItem)((TextBlock)sender).DataContext).Position - 1;
            viewModel.CurrentJobItem = ((ViewModel)DataContext).JobItemCollection[position];
            win1.DataContext = viewModel;
            win1.ShowDialog();
            win1.Activate();
        }
        #endregion

        /// <summary>
        /// Moves jobs from _items to _itemsdone observablecollection on checkboxchange event,
        /// updates the databinding and sorts the collections.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            if ((bool)(sender as CheckBox).IsChecked)
            {
                var newitems = new ObservableCollection<JobItem>();
                var doneitems = new ObservableCollection<JobItem>();

                foreach (var item in ((ViewModel)DataContext).JobItemCollection)
                {
                    if (item.IsChecked)
                    {
                        var olditem = item;
                        doneitems.Add(olditem);
                    }
                    else
                    {
                        newitems.Add(item);
                    }
                }

                foreach (var item in ((ViewModel)DataContext).JobItemCollectionDone)
                {
                    if (item.IsChecked)
                    {
                        doneitems.Add(item);
                    }
                }
                newitems.BubbleSort();
                doneitems.BubbleSort();
                ((ViewModel)DataContext).JobItemCollection = newitems;
                ((ViewModel)DataContext).JobItemCollectionDone = doneitems;
            }
            else
            {
                var doneitems = new ObservableCollection<JobItem>();
                var undoneitems = new ObservableCollection<JobItem>();

                foreach (var item in ((ViewModel)DataContext).JobItemCollectionDone)
                {
                    if (!item.IsChecked)
                    {
                        var olditem = item;
                        ((ViewModel)DataContext).JobItemCollection.Add(olditem);
                    }
                    else
                    {
                        doneitems.Add(item);
                    }
                }
                foreach (var item in ((ViewModel)DataContext).JobItemCollectionDone)
                {
                    if (!item.IsChecked)
                    {
                        doneitems.Remove(item);
                    }
                    ((ViewModel)DataContext).JobItemCollectionDone = doneitems;
                }

                ((ViewModel)DataContext).JobItemCollection.BubbleSort();
                ((ViewModel)DataContext).JobItemCollectionDone.BubbleSort();
            }
        }


        /// <summary>
        /// Updates the Documentbinding of the RichTextBox upon lostfocus (changes made in the textbox are persisted to the datamodel)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void EditBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ((FsRichTextBox)sender as FsRichTextBox).UpdateDocumentBindings();
            ((ViewModel)DataContext).IsTasklistDirty = true;
        }


        private void TaskjobList_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var bah = "huh";
        }

        private void EditableTextBlock_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {

            string hah = "";
        }

        private void EditableTextBlock_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            string hah = "";
        }

        private void EditableTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string hah = "";
        }

        private void EditableTextBlock_ManipulationCompleted_1(object sender, ManipulationCompletedEventArgs e)
        {
            string hah = "";
        }
    }
}
