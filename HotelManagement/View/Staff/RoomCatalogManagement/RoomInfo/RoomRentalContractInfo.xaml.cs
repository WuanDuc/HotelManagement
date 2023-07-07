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
    /// Interaction logic for RoomRentalContractInfo.xaml
    /// </summary>
    public partial class RoomRentalContractInfo : Window
    {
        public RoomRentalContractInfo()
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            PrintDialog printDialog = new PrintDialog();

            printDialog.PrintVisual(invoice, "Invoice");

        }
    }
}
