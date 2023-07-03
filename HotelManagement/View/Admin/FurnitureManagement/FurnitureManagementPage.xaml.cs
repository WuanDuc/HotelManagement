using CinemaManagementProject.Utilities;
using HotelManagement.DTOs;
using HotelManagement.ViewModel;
using HotelManagement.ViewModel.AdminVM.FurnitureManagementVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotelManagement.View.Admin.FurnitureManagement
{
    /// <summary>
    /// Interaction logic for FurnitureManagementPage.xaml
    /// </summary>
    public partial class FurnitureManagementPage : Page
    {


        public FurnitureManagementPage()
        {
            InitializeComponent();
        }
        
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DoubleAnimation animationWidth = new DoubleAnimation(450, TimeSpan.FromSeconds(0.2));
            DoubleAnimation animationOpacity = new DoubleAnimation(0.0, 1.0, TimeSpan.FromSeconds(0.2));

            Storyboard storyboard = new Storyboard();

            Storyboard.SetTargetName(animationOpacity, AddMoreFurniture.Name);
            Storyboard.SetTargetProperty(animationOpacity, new PropertyPath(Control.OpacityProperty));
            storyboard.Children.Add(animationOpacity);
            storyboard.Begin(this);

            AddMoreFurniture.BeginAnimation(WidthProperty, animationWidth);

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation animation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
            AddMoreFurniture.BeginAnimation(WidthProperty, animation);
            DoubleAnimation animationOpacity = new DoubleAnimation(1.0, 0.0, TimeSpan.FromSeconds(0.2));

            Storyboard storyboard = new Storyboard();

            Storyboard.SetTargetName(animationOpacity, AddMoreFurniture.Name);
            Storyboard.SetTargetProperty(animationOpacity, new PropertyPath(Control.OpacityProperty));
            storyboard.Children.Add(animationOpacity);
            storyboard.Begin(this);
        }

        private void AvatarMask_MouseMove(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            Border mask = (Border)grid.FindName("Mask");
            mask.Opacity = 0.25;
            StackPanel st = (StackPanel)grid.FindName("ChooseType");
            st.Visibility = Visibility.Visible;
        }

        private void AvatarMask_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            Border mask = (Border)grid.FindName("Mask");
            mask.Opacity = 0;
            StackPanel st = (StackPanel)grid.FindName("ChooseType");
            st.Visibility = Visibility.Collapsed;

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

    }
}
