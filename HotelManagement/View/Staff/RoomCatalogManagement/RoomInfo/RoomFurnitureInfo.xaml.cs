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

namespace HotelManagement.View.Staff.RoomCatalogManagement.RoomInfo
{
    /// <summary>
    /// Interaction logic for RoomFunitureInfo.xaml
    /// </summary>
    public partial class RoomFurnitureInfo : Window
    {
        public RoomFurnitureInfo()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(SearchBox.Text))
                return true;
            else
                return ((item as RoomFurnituresDetailDTO).FurnitureName.IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0
                    || (item as RoomFurnituresDetailDTO).FurnitureType.IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);


        }

        private void Search_SearchTextChange(object sender, EventArgs e)
        {
            CollectionView view1 = (CollectionView)CollectionViewSource.GetDefaultView(listFurniture.ItemsSource);
            if (view1 != null)
            {
                view1.Filter = Filter;
                //result.Text = Listviewmini.Items.Count.ToString();
                CollectionViewSource.GetDefaultView(listFurniture.ItemsSource).Refresh();
            }
        }
    }
}
