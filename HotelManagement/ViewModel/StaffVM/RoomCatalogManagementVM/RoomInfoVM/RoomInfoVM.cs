using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using HotelManagement.Utils;
using HotelManagement.View.Admin.CustomerManagement;
using HotelManagement.View.CustomMessageBoxWindow;
using HotelManagement.View.Staff.RoomCatalogManagement.RoomInfo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HotelManagement.ViewModel.StaffVM.RoomCatalogManagementVM
{
    public partial class RoomCatalogManagementVM : BaseVM
    {


        private ComboBoxItem _SelectedRoomCleaningStatus;
        public ComboBoxItem SelectedRoomCleaningStatus
        {
            get { return _SelectedRoomCleaningStatus; }
            set { _SelectedRoomCleaningStatus = value; OnPropertyChanged(); }
        }

        private string _SelectedFurnitureTypeCbbFilter;
        public string SelectedFurnitureTypeCbbFilter
        {
            get { return _SelectedFurnitureTypeCbbFilter; }
            set
            {
                _SelectedFurnitureTypeCbbFilter = value; OnPropertyChanged();

            }
        }
        private ObservableCollection<RoomCustomerDTO> _ListCustomer;
        public ObservableCollection<RoomCustomerDTO> ListCustomer
        {
            get { return _ListCustomer; }
            set { _ListCustomer = value; OnPropertyChanged(); }
        }
        private RoomCustomerDTO _SelectedCustomer;
        public RoomCustomerDTO SelectedCustomer
        {
            get { return _SelectedCustomer; }
            set { _SelectedCustomer = value; OnPropertyChanged(); }
        }
        private string _CustomerAddress;
        public string CustomerAddress
        {
            get { return _CustomerAddress; }
            set { _CustomerAddress = value; OnPropertyChanged(); }
        }
        private List<string> _ListFurnitureType;
        public List<string> ListFurnitureType
        {
            get => _ListFurnitureType;
            set
            {
                _ListFurnitureType = value;
                OnPropertyChanged();
            }
        }
        private string _CCCD;
        public string CCCD
        {
            get { return _CCCD; }
            set { _CCCD = value; OnPropertyChanged(); }
        }
        private string _CustomerName;
        public string CustomerName
        {
            get { return _CustomerName; }
            set { _CustomerName = value; OnPropertyChanged(); }
        }
        private ComboBoxItem _SelectedType;
        public ComboBoxItem SelectedType
        {
            get { return _SelectedType; }
            set { _SelectedType = value; OnPropertyChanged(); }
        }
        public async Task ChangeRoomStatusFunc(RoomWindow p)
        {
                p.Close();
                RefreshCM.Execute(MainPage);
        }
        public async Task UpdateRoomInfoFunc(RoomWindow p)
        {
                p.Close();
                RefreshCM.Execute(MainPage);
        }
        public async Task SaveCustomerFunc(AddCusWindow p)
        {

            if (string.IsNullOrEmpty(p.tbName.Text) || string.IsNullOrEmpty(p.tbAddress.Text) || string.IsNullOrEmpty(p.tbCCCD.Text))
            {
                CustomMessageBox.ShowOk("Vui lòng điền đầy đủ thông tin!", "Thông báo", "Ok", CustomMessageBoxImage.Warning);
                return;
            }
            foreach (var i in p.tbCCCD.Text)
            {
                if (!"0123456789".Contains(i))
                {
                    CustomMessageBox.ShowOk("Sai định dạng CCCD!", "Thông Báo", "OK", CustomMessageBoxImage.Warning);
                    return;
                }
            }
            if (p.tbCCCD.Text.Length != 12)
            {
                CustomMessageBox.ShowOk("Sai định dạng CCCD!", "Thông Báo", "OK", CustomMessageBoxImage.Warning);
                return;
            }
            RoomCustomerDTO newCus = new RoomCustomerDTO
            {
                CustomerName = p.tbName.Text,
                CustomerType = SelectedType.Content.ToString(),
                CustomerAddress = p.tbAddress.Text,
                CCCD = p.tbCCCD.Text,
                RentalContractId = SelectedRoom.RentalContractId,

            };

            (bool isSucessed, string mess, List<RoomCustomerDTO> listCustomer) = await RoomCustomerService.Ins.AddRoomCustomer(newCus);
            if (isSucessed)
            {
                ListCustomer = new ObservableCollection<RoomCustomerDTO>(listCustomer);
                p.Close();
                CustomMessageBox.ShowOk(mess, "Thông báo", "OK", CustomMessageBoxImage.Success);
            }
            else
            {
                CustomMessageBox.ShowOk(mess, "Lỗi", "OK", CustomMessageBoxImage.Error);
            }
        }
        public async Task SaveEditCustomerFunc(EditCusWindow p)
        {


            if (string.IsNullOrEmpty(p.tbName.Text) || string.IsNullOrEmpty(p.tbAddress.Text) || string.IsNullOrEmpty(p.tbCCCD.Text))
            {
                CustomMessageBox.ShowOk("Vui lòng điền đầy đủ thông tin!", "Thông báo", "Ok", CustomMessageBoxImage.Warning);
                return;
            }
            foreach (var i in p.tbCCCD.Text)
            {
                if (!"0123456789".Contains(i))
                {
                    CustomMessageBox.ShowOk("Sai định dạng CCCD!", "Thông Báo", "OK", CustomMessageBoxImage.Warning);
                    return;
                }
            }
            if (p.tbCCCD.Text.Length != 12)
            {
                CustomMessageBox.ShowOk("Sai định dạng CCCD!", "Thông Báo", "OK", CustomMessageBoxImage.Warning);
                return;
            }
            RoomCustomerDTO updateCus = new RoomCustomerDTO
            {
                CustomerName = p.tbName.Text,
                CustomerType = SelectedType.Content.ToString(),
                CustomerAddress = p.tbAddress.Text,
                CCCD = p.tbCCCD.Text,
                RentalContractId = SelectedRoom.RentalContractId,
                RoomCustomerId = SelectedCustomer.RoomCustomerId,
                STT = SelectedCustomer.STT,
            };

            (bool isSucessed, string mess, List<RoomCustomerDTO> listCustomer) = await RoomCustomerService.Ins.UpdateRoomCustomer(updateCus);
            if (isSucessed)
            {
                ListCustomer = new ObservableCollection<RoomCustomerDTO>(listCustomer);
                p.Close();
                CustomMessageBox.ShowOk(mess, "Thông báo", "OK", CustomMessageBoxImage.Success);
            }
            else
            {
                CustomMessageBox.ShowOk(mess, "Lỗi", "OK", CustomMessageBoxImage.Error);
            }
        }
        public async Task DeleteCustomerFunc()
        {


            CustomMessageBoxResult res = CustomMessageBox.ShowOkCancel("Bạn có chắc chắn muốn xóa vị khách này?", "Thông báo", "Ok", "Cancel", CustomMessageBoxImage.Question);
            if (res == CustomMessageBoxResult.Cancel) return;



            (bool isSucessed, string mess, List<RoomCustomerDTO> listCustomer) = await RoomCustomerService.Ins.DeleteRoomCustomer(SelectedCustomer);
            if (isSucessed)
            {
                ListCustomer = new ObservableCollection<RoomCustomerDTO>(listCustomer);
                CustomMessageBox.ShowOk(mess, "Thông báo", "OK", CustomMessageBoxImage.Success);
            }
            else
            {
                CustomMessageBox.ShowOk(mess, "Lỗi", "OK", CustomMessageBoxImage.Error);
            }
        }
        private void ChangeListFurnitureType()
        {
        }

    }
}
