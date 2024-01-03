using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using HotelManagement.Utils;
using HotelManagement.View.CustomMessageBoxWindow;
using HotelManagement.View.Staff.RoomCatalogManagement.RoomOrder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagement.ViewModel.StaffVM.RoomCatalogManagementVM
{
    public partial class RoomCatalogManagementVM : BaseVM
    {
        private double _sumOrder;
        public double SumOrder
        {
            get { return _sumOrder; }
            set { _sumOrder = value; OnPropertyChanged(); }
        }
        private ComboBoxItem _selectedItemFilter;
        public ComboBoxItem SelectedItemFilter
        {
            get => _selectedItemFilter;
            set
            {
                _selectedItemFilter = value;
                OnPropertyChanged();
            }
        }
        //public void LoadProductListView(Operation operation, ServiceDTO service = null)
        //{
        //    if(operation == Operation.UPDATE_PROD_QUANTITY)
        //    { 
        //        ServiceDTO serviceUpdateQuantity = ProductList.FirstOrDefault(item => item.ServiceId == service.ServiceId);
        //        if (serviceUpdateQuantity == null)
        //            return;
        //        ProductList[ProductList.IndexOf(serviceUpdateQuantity)] = service;
        //        AllProducts[AllProducts.IndexOf(serviceUpdateQuantity)] = service;
        //    }
        //}
    }
}
