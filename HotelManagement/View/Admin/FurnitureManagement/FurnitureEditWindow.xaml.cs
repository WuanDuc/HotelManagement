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

namespace HotelManagement.View.Admin.FurnitureManagement
{
    /// <summary>
    /// Interaction logic for FurnitureEditWindow.xaml
    /// </summary>
    public partial class FurnitureEditWindow : Window
    {
        public FurnitureEditWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void FurnitureEditWD_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }    
        }
        private void BtnAdd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectTypeFurniture.Visibility = Visibility.Collapsed;
            AddNewTypeFurniture.Visibility = Visibility.Visible;
            AddNewTypeFurniture.IsEnabled = true;
            TypeBoxAddNewTypeFurniture.Text = "";
            TypeBoxAddNewTypeFurniture.Focus();
            SelectTypeFurniture.IsEnabled = false;
        }

        private void BtnFilter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectTypeFurniture.Visibility = Visibility.Visible;
            AddNewTypeFurniture.Visibility = Visibility.Collapsed;
            AddNewTypeFurniture.IsEnabled = false;
            SelectTypeFurniture.IsEnabled = true;
        }
    }
}
