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
using System.Windows.Shapes;

namespace HotelManagement.View.Staff.RoomCatalogManagement.RoomOrder
{
    /// <summary>
    /// Interaction logic for RoomOrderProducts.xaml
    /// </summary>
    public partial class RoomOrderProducts : Window
    {
        public RoomOrderProducts()
        {
            InitializeComponent();
        }

        private void Search_SearchTextChange(object sender, EventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListViewProducts.ItemsSource);
            if (view != null)
            {
                view.Filter = Filter;
                CollectionViewSource.GetDefaultView(ListViewProducts.ItemsSource).Refresh();
            }
        }
        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(SearchBox.Text))
                return true;
            else
                return ((item as ServiceDTO).ServiceName.IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
