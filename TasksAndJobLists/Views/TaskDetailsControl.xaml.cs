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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FsWpfControls.FsRichTextBox;
using TasksAndJobLists.Helpers;
using TasksAndJobLists.Models;
using TasksAndTaskLists.Models;

namespace TasksAndJobLists.Views
{
    /// <summary>
    /// Interaction logic for TaskInformationControl.xaml
    /// </summary>
    public partial class TaskDetailsControl : UserControl
    {
        
        public TaskDetailsControl()
        {
            InitializeComponent();
          // var task = ((ViewModel)DataContext).CurrentTask;
            //EditBox.Document = HelperMethods.SetRTF(task.Description);
        }

        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            var currentTask = ((ViewModel)DataContext).CurrentTask;

            var insertThis = HelperMethods.TaskItemToTaskEntity(currentTask);
            var oldItem = ((ViewModel)DataContext).context.TasksEntities.Where(a => a.TaskEntityId.Equals(insertThis.TaskEntityId)).First();
            oldItem.Title = insertThis.Title;
            oldItem.Description = insertThis.Description;
            ((ViewModel)DataContext).context.SaveChanges();
            ((ViewModel)DataContext).TaskItemCollection = new ObservableCollection<TaskItem>();
            var list = HelperMethods.ConvertTaskEntitiesToTaskItems(((ViewModel)DataContext).context.TasksEntities.AsEnumerable().ToList());
            foreach (var item in list)
            {
                ((ViewModel)DataContext).TaskItemCollection.Add(item);
            }
            LoadTasks();
        }

        private void EditBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ((FsRichTextBox)sender as FsRichTextBox).UpdateDocumentBindings();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        
        private void LoadTasks()
        {
            ((ViewModel)DataContext).TaskItemCollection = new ObservableCollection<TaskItem>();
            var list = HelperMethods.ConvertTaskEntitiesToTaskItems(((ViewModel)DataContext).context.TasksEntities.AsEnumerable().ToList());
            foreach (var item in list)
            {
                ((ViewModel)DataContext).TaskItemCollection.Add(item);
            }
        }

    }
}
