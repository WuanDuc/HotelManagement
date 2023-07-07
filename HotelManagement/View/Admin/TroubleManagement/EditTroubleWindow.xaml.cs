using HotelManagement.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HotelManagement.View.Admin.TroubleManagement
{
    /// <summary>
    /// Interaction logic for EditTroubleWindow.xaml
    /// </summary>
    public partial class EditTroubleWindow : Window
    {
        public EditTroubleWindow()
        {
            InitializeComponent();
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (predictgrid.Visibility == Visibility.Visible)
            {
                predictgrid.Visibility = Visibility.Collapsed;
               
            }
            else if (predictgrid.Visibility == Visibility.Collapsed)
            {
               
                
                cbbStatusByCustomer.SelectedIndex = 0;
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text.Length == 0)
                tb.Text = "0";
        }

        private void cbbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            ComboBox cbb = sender as ComboBox;
            if (cbb.SelectedIndex == 0)
            {
                fixdate.Visibility = Visibility.Visible;
                finishdate.Visibility = Visibility.Collapsed;
                fixprice.Visibility = Visibility.Collapsed;
            }
            if (cbb.SelectedIndex == 1)
            {
                fixdate.Visibility = Visibility.Visible;
                finishdate.Visibility = Visibility.Visible;
                fixprice.Visibility = Visibility.Visible;
            }
            if (cbb.SelectedIndex == 2)
            {
                fixdate.Visibility = Visibility.Collapsed;
                finishdate.Visibility = Visibility.Collapsed;
                fixprice.Visibility = Visibility.Collapsed;
            }
            
        }

        private void cbbStatus1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbb = sender as ComboBox;
            if(cbb.SelectedIndex == 0)
            {
                predictgrid.Visibility = Visibility.Visible;
                fixdate.Visibility = Visibility.Collapsed;
                finishdate.Visibility = Visibility.Collapsed;
                fixprice.Visibility = Visibility.Collapsed;
            }
            if (cbb.SelectedIndex == 1)
            {
                predictgrid.Visibility = Visibility.Visible;
                fixdate.Visibility = Visibility.Visible;
                finishdate.Visibility = Visibility.Collapsed;
                fixprice.Visibility = Visibility.Collapsed;
            }
            if (cbb.SelectedIndex == 2)
            {
                predictgrid.Visibility = Visibility.Collapsed;
                fixdate.Visibility = Visibility.Visible;
                finishdate.Visibility = Visibility.Visible;
                fixprice.Visibility = Visibility.Visible;
            }
            if (cbb.SelectedIndex == 3)
            {
                predictgrid.Visibility = Visibility.Collapsed;
                fixdate.Visibility = Visibility.Collapsed;
                finishdate.Visibility = Visibility.Collapsed;
                fixprice.Visibility = Visibility.Collapsed;
            }
        }

        private void TextBox_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
    }
}
