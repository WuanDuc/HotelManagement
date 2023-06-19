using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HotelManagement.View.Admin.FurnitureManagement
{
    /// <summary>
    /// Interaction logic for FurnitureImportWindow.xaml
    /// </summary>
    public partial class FurnitureImportWindow : Window
    {
        public FurnitureImportWindow()
        {
            InitializeComponent();
        }

        private void FurnitureImportWD_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }    
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
