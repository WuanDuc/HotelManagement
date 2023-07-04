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
        private ServiceDTO _CleaningServices;
        public ServiceDTO CleaningService
        {
            get { return _CleaningServices; }
            set { _CleaningServices = value; OnPropertyChanged(); }
        }
        private ServiceDTO _LaundryService;
        public ServiceDTO LaundryService
        {
            get { return _LaundryService; }
            set { _LaundryService = value; OnPropertyChanged(); }
        }
        private ObservableCollection<ServiceUsingDTO> _ListService;
        public ObservableCollection<ServiceUsingDTO> ListService
        {
            get { return _ListService; }
            set { _ListService = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ServiceDTO> _ProducList;
        public ObservableCollection<ServiceDTO> ProductList
        {
            get { return _ProducList; }
            set { _ProducList = value; OnPropertyChanged(); }
        }
        private ObservableCollection<ServiceDTO> _AllProducts;
        public ObservableCollection<ServiceDTO> AllProducts
        {
            get { return _AllProducts; }
            set { _AllProducts = value; OnPropertyChanged(); }
        }
        private ObservableCollection<ServiceDTO> _orderList;
        public ObservableCollection<ServiceDTO> OrderList
        {
            get => _orderList;
            set
            {
                _orderList = value;
                OnPropertyChanged();
            }
        }
        private double _sumOrder;
        public double SumOrder
        {
            get { return _sumOrder; }
            set { _sumOrder = value; OnPropertyChanged(); }
        }
        private ServiceDTO selectedProduct;
        public ServiceDTO SelectedProduct
        {
            get { return selectedProduct; }
            set { selectedProduct = value; OnPropertyChanged(); }
        }

        private ServiceDTO serviceCache;
        public ServiceDTO ServiceCache
        {
            get { return serviceCache; }
            set { serviceCache = value; OnPropertyChanged(); }
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
        public async Task SaveUsingCleaningService(Window p)
        {
            ServiceUsingDTO serviceUsingDTO = new ServiceUsingDTO
            {
                RentalContractId = SelectedRoom.RentalContractId,
                ServiceId = CleaningService.ServiceId,
                UnitPrice = CleaningService.ServicePrice,
                Quantity = 1,
            };
            (bool isSucceed, string message) = await ServiceUsingHelper.Ins.SaveService(serviceUsingDTO);
            if (isSucceed)
            {
                p.Close();
                CustomMessageBox.ShowOk(message, "Thông báo", "Ok", CustomMessageBoxImage.Success);
                ListService = new ObservableCollection<ServiceUsingDTO>(await ServiceUsingHelper.Ins.GetListUsingService(SelectedRoom.RentalContractId));

            }
            else
            {
                CustomMessageBox.ShowOk(message, "Thông báo", "Ok", CustomMessageBoxImage.Error);

            }
        }
        public async Task SaveUsingLaundryService(RoomOrderLaundry p)
        {
            ServiceUsingDTO serviceUsingDTO = new ServiceUsingDTO
            {
                RentalContractId = SelectedRoom.RentalContractId,
                ServiceId = LaundryService.ServiceId,
                UnitPrice = LaundryService.ServicePrice,
                Quantity = int.Parse(p.tbKg.Text),
            };
            (bool isSucceed, string message) = await ServiceUsingHelper.Ins.SaveService(serviceUsingDTO);
            if (isSucceed)
            {
                p.Close();
                CustomMessageBox.ShowOk(message, "Thông báo", "Ok", CustomMessageBoxImage.Success);
                ListService = new ObservableCollection<ServiceUsingDTO>(await ServiceUsingHelper.Ins.GetListUsingService(SelectedRoom.RentalContractId));

            }
            else
            {
                CustomMessageBox.ShowOk(message, "Thông báo", "Ok", CustomMessageBoxImage.Error);

            }
        }
        public async Task LoadAllProduct()
        {
            (bool isSuccess, string messageReturn, List<ServiceDTO> listProduct) = await Task.Run(() => ServiceHelper.Ins.GetAllProduct());

            if (isSuccess)
            {
                AllProducts = new ObservableCollection<ServiceDTO>(listProduct);
                ProductList = new ObservableCollection<ServiceDTO>(listProduct);
                OrderList = new ObservableCollection<ServiceDTO>();
                SumOrder = 0;
            }
            else
            {
                CustomMessageBox.ShowOk(messageReturn, "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
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
        public void LoadProductToBill()
        {
            if (ServiceCache.Quantity > 0)
            {
                try
                {
                    ServiceCache.Quantity -= 1;
                    if (!OrderList.Contains(ServiceCache))
                    {
                        ServiceCache.ImportQuantity = 1;
                        OrderList.Add(ServiceCache);
                        SumOrder += ServiceCache.ServicePrice;
                    }
                    else
                    {
                        ServiceCache.ImportQuantity += 1;
                        SumOrder += ServiceCache.ServicePrice;
                    }
                }
                catch (Exception e)
                {
                    CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", CustomMessageBoxImage.Error);
                }
            }
            else
            {
                CustomMessageBox.ShowOk("Bạn đã chọn hết số lượng sản phẩm này!", "Cảnh báo", "Ok", CustomMessageBoxImage.Warning);
            }
        }
        public void DecreaseProductInBill()
        {
            if (ServiceCache.ImportQuantity > 1)
            {
                try
                {
                    ServiceCache.ImportQuantity -= 1;
                    SumOrder -= ServiceCache.ServicePrice;
                    ServiceCache.Quantity += 1;
                }
                catch (Exception e)
                {
                    CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", CustomMessageBoxImage.Error);
                }
            }
            else
            {
                if (CustomMessageBox.ShowOkCancel("Bạn có muốn xóa sản phẩm?", "Cảnh báo", "Xóa", "Không", CustomMessageBoxImage.Warning)
                == CustomMessageBoxResult.OK)
                {
                    OrderList.Remove(ServiceCache);
                    SumOrder -= ServiceCache.ServicePrice;
                    ServiceCache.Quantity += 1;
                }
            }
        }
        public void IncreaseProductInBill()
        {
            if (ServiceCache.Quantity > 0)
            {
                try
                {
                    ServiceCache.ImportQuantity += 1;
                    SumOrder += ServiceCache.ServicePrice;
                    ServiceCache.Quantity -= 1;
                }
                catch (Exception e)
                {
                    CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", CustomMessageBoxImage.Error);
                }
            }
            else
            {
                CustomMessageBox.ShowOk("Bạn đã chọn hết số lượng sản phẩm này!", "Cảnh báo", "Ok", CustomMessageBoxImage.Warning);
            }
        }
        public void DeleteProductInBill()
        {
            try
            {
                if (CustomMessageBox.ShowOkCancel("Bạn có muốn xóa sản phẩm?", "Cảnh báo", "Xóa", "Không", CustomMessageBoxImage.Warning)
               == CustomMessageBoxResult.OK)
                {
                    ServiceCache.Quantity += ServiceCache.ImportQuantity;
                    SumOrder -= (ServiceCache.ServicePrice * ServiceCache.ImportQuantity);
                    ServiceCache.ImportQuantity = 0;
                    OrderList.Remove(ServiceCache);
                }
            }
            catch (Exception e)
            {
                CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", CustomMessageBoxImage.Error);
            }
        }
        public async Task AddOrderProduct(Window p)
        {
            if (OrderList.Count == 0)
            {
                CustomMessageBox.ShowOk("Vui lòng chọn sản phẩm!", "Thông báo", "Ok", CustomMessageBoxImage.Warning);
                return;
            }
            (bool isSucceed, string message) = await ServiceUsingHelper.Ins.SaveUsingProduct(OrderList, SelectedRoom);
            if (isSucceed)
            {
                CustomMessageBox.ShowOk(message, "Thông báo", "Ok", CustomMessageBoxImage.Success);
                SumOrder = 0;
                OrderList = null;
                ListService = new ObservableCollection<ServiceUsingDTO>(await ServiceUsingHelper.Ins.GetListUsingService(SelectedRoom.RentalContractId));
                p.Close();
            }
            else
            {
                CustomMessageBox.ShowOk(message, "Thông báo", "Ok", CustomMessageBoxImage.Error);
            }
        }
    }
}
