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

namespace HotelManagement.View.Admin.RoomManagement
{
    /// <summary>
    /// Interaction logic for RoomManagementPage.xaml
    /// </summary>
    public partial class RoomManagementPage : Page
    {
        public RoomManagementPage()
        {
            InitializeComponent();
        }
        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(SearchBox.Text))
                return true;
            else
                return ((item as RoomDTO).RoomNumber.ToString().IndexOf(SearchBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0
                    || (item as RoomDTO).RoomTypeName.ToString().IndexOf(SearchBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0
                    || (item as RoomDTO).RoomStatus.ToString().IndexOf(SearchBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0
                    || (item as RoomDTO).RoomCleaningStatus.ToString().IndexOf(SearchBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0);
        }
        private void Search_SearchTextChange(object sender, EventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(RoomListView.ItemsSource);
            if (view != null)
            {
                view.Filter = Filter;
                result.Content = RoomListView.Items.Count;
                CollectionViewSource.GetDefaultView(RoomListView.ItemsSource).Refresh();
            }
        }
    }
}
