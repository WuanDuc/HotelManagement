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

namespace HotelManagement.View.Admin.CustomerManagement
{
    /// <summary>
    /// Interaction logic for CustomerManagementPage.xaml
    /// </summary>
    public partial class CustomerManagementPage : Page
    {
        public CustomerManagementPage()
        {
            InitializeComponent();
        }

        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(FilterBox.Text)) return true;
            
            return ((item as CustomerDTO).CustomerId.IndexOf(FilterBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0)
                || ((item as CustomerDTO).CustomerName.IndexOf(FilterBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0)
                || ((item as CustomerDTO).PhoneNumber.IndexOf(FilterBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0)
                || ((item as CustomerDTO).CCCD.IndexOf(FilterBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0);
            
        }
        private void filterbox_textchange(object sender, EventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(_ListView.ItemsSource);
            view.Filter = Filter;
            CollectionViewSource.GetDefaultView(_ListView.ItemsSource).Refresh();
        }
    }
}
