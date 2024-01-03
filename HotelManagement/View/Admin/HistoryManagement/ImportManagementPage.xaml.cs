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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotelManagement.View.Admin.HistoryManagement
{
    /// <summary>
    /// Interaction logic for ImportManagementPage.xaml
    /// </summary>
    public partial class ImportManagementPage : Page
    {
        public ImportManagementPage()
        {
            InitializeComponent();
        }

        private void cbb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (cbbmonth != null)
            {
                if(comboBox.SelectedIndex == 0)
                {
                    bodermonth.Visibility= Visibility.Collapsed;
                }
                if(comboBox.SelectedIndex == 1)
                {
                    bodermonth.Visibility= Visibility.Visible;
                }
            }
        }

        private bool Filter(object item)   //can sua//
        {
            return true;
        }
        private void filterbox_textchange(object sender, EventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(_ListView.ItemsSource);
            view.Filter = Filter;
            CollectionViewSource.GetDefaultView(_ListView.ItemsSource).Refresh();
        }
    }
}
