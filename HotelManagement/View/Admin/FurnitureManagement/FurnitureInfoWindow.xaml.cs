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
    /// Interaction logic for FurnitureInfoWindow.xaml
    /// </summary>
    public partial class FurnitureInfoWindow : Window
    {
        public FurnitureInfoWindow()
        {
            InitializeComponent();
        }

        private void FurnitureInfoWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CloseButton_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
