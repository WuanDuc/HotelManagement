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

namespace HotelManagement.View.Staff.TroubleReport
{
    /// <summary>
    /// Interaction logic for TroubleReportPage.xaml
    /// </summary>
    public partial class TroubleReportPage : Page
    {
        public TroubleReportPage()
        {
            InitializeComponent();
        }
        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(FilterBox.Text)) return true;

            return ((item as TroubleDTO).Title.IndexOf(FilterBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);

        }
        private void filterbox_textchange(object sender, EventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(_ListView.ItemsSource);
            view.Filter = Filter;
            CollectionViewSource.GetDefaultView(_ListView.ItemsSource).Refresh();
        }
    }

}
