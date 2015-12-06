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
    public partial class DoneJobControl : UserControl
    {
        

        public DoneJobControl()
        {
            InitializeComponent();
        }

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
                //((ViewModel)DataContext).JobItemCollectionDone.BubbleSort();
            }
        }
    }
}
