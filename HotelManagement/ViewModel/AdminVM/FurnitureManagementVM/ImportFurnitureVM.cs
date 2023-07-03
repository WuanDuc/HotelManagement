using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using HotelManagement.Utils;
using HotelManagement.View.Admin;
using IronXL.Formatting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static HotelManagement.Utilities.Helper;

namespace HotelManagement.ViewModel.AdminVM.FurnitureManagementVM
{
    public partial class FurnitureManagementVM : BaseVM
    {
        public string ImportQuantity { get; set; }
        public string ImportPrice { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateDateString { get; set; }


        private ObservableCollection<FurnitureDTO> orderList;
        public ObservableCollection<FurnitureDTO> OrderList
        {
            get { return orderList; }
            set { orderList = value; OnPropertyChanged(); }
        }
        private float sumMoney;
        public float SumMoney
        {
            get { return sumMoney; }
            set { sumMoney = value; OnPropertyChanged(); }
        }
        public string SumMoneyStr { get; set; }
        public async Task ImportFurniture(FurnitureDTO furnitureSelected, Window wd, AdminWindow mainWD)
        {
            try
            {
                if(string.IsNullOrEmpty(ImportQuantity))
                {
                    CustomMessageBox.ShowOk("Vui lòng nhập số lượng", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                    return;
                }
                if (string.IsNullOrEmpty(ImportPrice))
                {
                    CustomMessageBox.ShowOk("Vui lòng nhập giá nhập", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                    return;
                }
                if(!Number.IsNumeric(ImportQuantity))
                {
                    CustomMessageBox.ShowOk("Vui lòng nhập chữ số cho trường số lượng", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                    return;
                }
                if (!Number.IsNumeric(ImportPrice))
                {
                    CustomMessageBox.ShowOk("Vui lòng nhập chữ số cho trường giá nhập", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                    return;
                }
                if (!Number.IsPositive(ImportPrice))
                {
                    CustomMessageBox.ShowOk("Giá nhập không được âm", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                    return;
                }
                if (!Number.IsPositive(ImportQuantity))
                {
                    CustomMessageBox.ShowOk("Số lượng là một số lớn hơn 0", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                    return;
                }

                (bool isSuccess, string messageReturn) = await Task.Run(() => FurnitureService.Ins.ImportFurniture(furnitureSelected, ImportPrice, ImportQuantity));
                if(isSuccess)
                {
                    CustomMessageBox.ShowOk(messageReturn, "Thành công", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                    furnitureCache.Quantity = furnitureCache.Quantity + int.Parse(ImportQuantity);
                    LoadFurnitureListView(Operation.UPDATE_PROD_QUANTITY, furnitureCache);
                    wd.Close();
                
                    mainWD.MaskOverSideBar.Visibility = Visibility.Collapsed;
                }    
                else
                {
                    CustomMessageBox.ShowOk(messageReturn, "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                }
                ImportQuantity = null;
                ImportPrice = null;
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
        public void LoadItemToList(FurnitureDTO furnitureSelected)
        {
           FurnitureDTO furniture = OrderList.FirstOrDefault(item => item.FurnitureID == furnitureSelected.FurnitureID);
           if(furniture == null)
           {
                furnitureSelected.ImportPrice = 0;
                furnitureSelected.ImportQuantity = 1;
                OrderList.Add(furnitureSelected);
                return;
           }
           else
           {
                furniture.ImportQuantity = furniture.ImportQuantity + 1;
                SumMoney += furniture.ImportPrice;
           }
        }
        public async Task ImportListFurniture(Window wd, AdminWindow mainWD)
        {
            (bool isSuccess, string messageReturn, List<FurnitureDTO> listReturned) = await Task.Run(() => FurnitureService.Ins.ImportListFurniture(OrderList));
            if(isSuccess)
            {
                CustomMessageBox.ShowOk(messageReturn, "Thành công", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                for (int i = 0; i < listReturned.Count; i++)
                    LoadFurnitureListView(Operation.UPDATE_PROD_QUANTITY, listReturned[i]);
                OrderList.Clear();
                wd.Close();
                mainWD.MaskOverSideBar.Visibility = Visibility.Collapsed;
            }    
            else
            {
                CustomMessageBox.ShowOk(messageReturn, "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                wd.Close();
                mainWD.MaskOverSideBar.Visibility = Visibility.Collapsed;
            }    
        }
        public string DateTimeToString(DateTime dt)
        {
            string date;
            date = dt.Day + "/" + dt.Month + "/" + dt.Year + " " + dt.Hour + ":" + dt.Minute + ":" + dt.Second;
            return date;
        }
    }
}
