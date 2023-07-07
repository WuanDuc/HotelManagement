using HotelManagement.DTOs;
using HotelManagement.Model;
using HotelManagement.Model.Services;
using HotelManagement.Utils;
using HotelManagement.View.Admin;
using IronXL.Formatting;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static HotelManagement.Utilities.Helper;

namespace HotelManagement.ViewModel.AdminVM.ServiceManagementVM
{
    public partial class ServiceManagementVM
    {
        public string ImportPrice { get; set; }
        public string ImportQuantity { get; set; }
        public async Task ImportService(ServiceDTO serviceSelected, Window wd, AdminWindow mainWD)
        {
            try
            {
                if (string.IsNullOrEmpty(ImportQuantity))
                {
                    CustomMessageBox.ShowOk("Vui lòng nhập số lượng", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                    return;
                }
                if (string.IsNullOrEmpty(ImportPrice))
                {
                    CustomMessageBox.ShowOk("Vui lòng nhập giá nhập", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                    return;
                }

                int quantity;
                double price;
                bool isIntQuantity = Int32.TryParse(ImportQuantity, out quantity);
                bool isFloatPrice = double.TryParse(ImportPrice, out price);

                if (!isIntQuantity ||quantity <= 0)
                {
                    CustomMessageBox.ShowOk("Số lượng là một số nguyên dương", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                    return;
                }
                if (!isFloatPrice || price <= 0)
                {
                    CustomMessageBox.ShowOk("Giá nhập phải là số dương", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                    return;
                }

                serviceCache.ImportPrice = serviceSelected.ImportPrice = price;
                serviceCache.ImportQuantity = serviceSelected.ImportQuantity = quantity;
                    
                (bool isSuccess, string messageReturn) = await Task.Run(() => ServiceHelper.Ins.ImportService(serviceSelected, serviceCache.ImportPrice, serviceCache.ImportQuantity));
                if (isSuccess)
                {
                    CustomMessageBox.ShowOk(messageReturn, "Thành công", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                    serviceCache.Quantity = serviceCache.Quantity + serviceCache.ImportQuantity;
                    LoadProductListView(Operation.UPDATE_PROD_QUANTITY, new ServiceDTO(serviceCache));
                    wd.Close();
                    mainWD.MaskOverSideBar.Visibility = Visibility.Collapsed;
                }
                else
                {
                    CustomMessageBox.ShowOk(messageReturn, "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                }
            }
            catch (EntityException e)
            {
                CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
            }
            catch (Exception e)
            {
                CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
            }
        }
    }
}
