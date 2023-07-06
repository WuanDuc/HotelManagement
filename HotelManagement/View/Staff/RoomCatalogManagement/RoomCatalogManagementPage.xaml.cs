using BitMiracle.LibTiff.Classic;
using HotelManagement.DTOs;
using System;
using System.Collections.Generic;
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

namespace HotelManagement.View.Staff.RoomCatalogManagement
{
    /// <summary>
    /// Interaction logic for RoomCatalogManagementPage.xaml
    /// </summary>
    public partial class RoomCatalogManagementPage : Page
    {
        public RoomCatalogManagementPage()
        {
            InitializeComponent();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(SearchBox.Text))
                return true;
            else
                return ((item as RoomSettingDTO).RoomNameP.IndexOf(SearchBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0
                    || (item as RoomSettingDTO).RoomStatus.IndexOf(SearchBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0
                    || (item as RoomSettingDTO).RoomCleaningStatus.IndexOf(SearchBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0
                    || (((item as RoomSettingDTO).CustomerName != null) ? (item as RoomSettingDTO).CustomerName.IndexOf(SearchBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0 : false));

        }
        private void Search_SearchTextChange(object sender, EventArgs e)
        {
            CollectionView view1 = (CollectionView)CollectionViewSource.GetDefaultView(listRoom1.ItemsSource);
            if (view1 != null)
            {
                view1.Filter = Filter;
                //result.Text = Listviewmini.Items.Count.ToString();
                CollectionViewSource.GetDefaultView(listRoom1.ItemsSource).Refresh();
            }
            CollectionView view2 = (CollectionView)CollectionViewSource.GetDefaultView(listRoom2.ItemsSource);
            if (view2 != null)
            {
                view2.Filter = Filter;
                //result.Text = Listviewmini.Items.Count.ToString();
                CollectionViewSource.GetDefaultView(listRoom2.ItemsSource).Refresh();
            }
            CollectionView view3 = (CollectionView)CollectionViewSource.GetDefaultView(listRoom3.ItemsSource);
            if (view3 != null)
            {
                view3.Filter = Filter;
                //result.Text = Listviewmini.Items.Count.ToString();
                CollectionViewSource.GetDefaultView(listRoom3.ItemsSource).Refresh();
            }
        }

        private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem lbi = sender as ListBoxItem;
            listRoom1.SelectedItem = lbi.DataContext;
        }

        private void ListBoxItem2_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem lbi = sender as ListBoxItem;
            listRoom2.SelectedItem = lbi.DataContext;
        }

        private void ListBoxItem3_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem lbi = sender as ListBoxItem;
            listRoom3.SelectedItem = lbi.DataContext;
        }
    }
}
