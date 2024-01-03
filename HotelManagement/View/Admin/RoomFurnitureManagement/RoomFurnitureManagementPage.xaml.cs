using BitMiracle.LibTiff.Classic;
using HotelManagement.DTOs;
using IronXL.Drawing;
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

namespace HotelManagement.View.Admin.RoomFurnitureManagement
{
    /// <summary>
    /// Interaction logic for RoomFurnitureManagementPage.xaml
    /// </summary>
    public partial class RoomFurnitureManagementPage : Page
    {
        public RoomFurnitureManagementPage()
        {
            InitializeComponent();
        }

        private void SearchBox_SearchTextChange(object sender, EventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListViewFurnitureRoom.ItemsSource);
            if (view != null)
            {
                view.Filter = Filter;
                CollectionViewSource.GetDefaultView(ListViewFurnitureRoom.ItemsSource).Refresh();
            }
        }

        private bool Filter(object item)
        {               
            return true;
        }

        private void AvatarMask_MouseMove(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            Rectangle mask = (Rectangle)grid.FindName("MaskOver");
            mask.Opacity = 0.25;
            StackPanel st = (StackPanel)grid.FindName("ChooseType");
            st.Visibility = Visibility.Visible;
        }

        private void AvatarMask_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            Rectangle mask = (Rectangle)grid.FindName("MaskOver");
            mask.Opacity = 0;
            StackPanel st = (StackPanel)grid.FindName("ChooseType");
            st.Visibility = Visibility.Collapsed;

        }
    }
}
