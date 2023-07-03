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

namespace HotelManagement.View.Admin.StaffManagement
{
    /// <summary>
    /// Interaction logic for StaffManagementPage.xaml
    /// </summary>
    public partial class StaffManagementPage : Page
    {
        public StaffManagementPage()
        {
            InitializeComponent();
        }
        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(FilterBox.Text)) return true;
           
            return ((item as StaffDTO).StaffId.IndexOf(FilterBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as StaffDTO).StaffName.IndexOf(FilterBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as StaffDTO).PhoneNumber.IndexOf(FilterBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as StaffDTO).CCCD.IndexOf(FilterBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0);
            
        }
        private void filterbox_textchange(object sender, EventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(_ListView.ItemsSource);
            view.Filter = Filter;
            CollectionViewSource.GetDefaultView(_ListView.ItemsSource).Refresh();
        }
    }
}
