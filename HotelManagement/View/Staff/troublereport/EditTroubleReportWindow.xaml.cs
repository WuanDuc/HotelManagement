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

namespace HotelManagement.View.Staff.TroubleReport
{
    /// <summary>
    /// Interaction logic for EditTroubleReportWindow.xaml
    /// </summary>
    public partial class EditTroubleReportWindow : Window
    {
        public EditTroubleReportWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (txtboxMPT != null)
            {
                switch (comboBox.SelectedIndex)
                {
                    case 0:
                        {
                            txtboxMPT.Visibility = Visibility.Visible;
                            break;
                        }
                    case 1:
                        {
                            txtboxMPT.Visibility = Visibility.Hidden;
                            masophieu.Text = null;
                            break;
                        }
                }
            }
        }
    }
}
