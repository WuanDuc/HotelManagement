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

namespace HotelManagement.View.Admin.RoomFurnitureManagement
{
    /// <summary>
    /// Interaction logic for RoomFurnitureImportWindow.xaml
    /// </summary>
    public partial class RoomFurnitureImportWindow : Window
    {
        public RoomFurnitureImportWindow()
        {
            InitializeComponent();
        }

        private void RoomFurnitureImportWD_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void CloseButton2_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SearchBox_SearchTextChange(object sender, EventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListViewFurniture.ItemsSource);
            if (view != null)
            {
                view.Filter = Filter;
                CollectionViewSource.GetDefaultView(ListViewFurniture.ItemsSource).Refresh();
            }
        }
        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(SearchBox.Text))
                return true;
            else
                return ((item as FurnitureDTO).FurnitureName.IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void ItemFurniture_MouseMove(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            Rectangle rec = (Rectangle)grid.FindName("Mask");
            rec.Opacity = 0.1;
        }

        private void ItemFurniture_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            Rectangle rec = (Rectangle)grid.FindName("Mask");
            rec.Opacity = 0;
        }
    }
}
