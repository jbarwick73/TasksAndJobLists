using System;
using System.Windows;
using System.Linq;
using TasksAndJobLists.Models;
using TasksAndJobLists.Helpers;
using TasksAndJobLists.Views;

namespace TasksAndJobLists
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MyContext context = new MyContext();

        public MainWindow()
        {
            var DBCHECK = HelperMethods.CheckDB(context);
            if (DBCHECK.Error)
            {
                MessageBox.Show(DBCHECK.ExceptionMessage + "  " + DBCHECK.InnerExceptionMessage);
                Application.Current.Shutdown();
                Environment.Exit(0);
            }
            InitializeComponent();
            var Taskslist = HelperMethods.ConvertTaskEntitiesToTaskItems(context.TasksEntities.AsEnumerable().ToList());
            ViewModel viewModel = new ViewModel();

            viewModel.TaskItemCollection = Taskslist;
            viewModel.CurrentTask = null;
            viewModel.context = context;
            DataContext = viewModel;
            viewModel.IsLoading = Visibility.Hidden;
        }
    }
}
