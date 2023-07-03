using HotelManagement.DTOs;
using HotelManagement.View.CustomMessageBoxWindow;
using HotelManagement.View.Admin.CustomerManagement;
using HotelManagement.Model.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System;
using System.Windows.Controls;
using System.Windows;
using HotelManagement.Utilities;
using System.Linq;
using HotelManagement.Utils;

namespace HotelManagement.ViewModel.AdminVM.CustomerManagementVM
{
    public partial class CustomerManagementVM : BaseVM
    {
        private ObservableCollection<CustomerDTO> _customerList;
        public ObservableCollection<CustomerDTO> CustomerList
        {
            get { return _customerList; }
            set { _customerList = value; OnPropertyChanged(); }
        }
        private CustomerDTO _selectedItem;
        public CustomerDTO SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }
        private bool _isSaving;
        public bool IsSaving
        {
            get { return _isSaving; }
            set { _isSaving = value; }
        }
        private string _customerId;
        public string CustomerId
        {
            get { return _customerId; }
            set { _customerId = value; OnPropertyChanged(); }
        }
        private string _fullname;
        public string FullName
        {
            get { return _fullname; }
            set { _fullname = value; OnPropertyChanged(); }
        }
        private string _phonenumber;
        public string Phonenumber
        {
            get { return _phonenumber; }
            set
            { _phonenumber = value; OnPropertyChanged(); }
        }
        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }
        private string _cccd;
        public string Cccd
        {
            get { return _cccd; }
            set
            { _cccd = value; OnPropertyChanged(); }
        }
        private Nullable<System.DateTime> _birthday;
        public Nullable<System.DateTime> Birthday
        {
            get { return _birthday; }
            set { _birthday = value; OnPropertyChanged(); }
        }
        private ComboBoxItem _gender;
        public ComboBoxItem Gender
        {
            get { return _gender; }
            set { _gender = value; OnPropertyChanged(); }
        }
        private ComboBoxItem _customerType;
        public ComboBoxItem CustomerType
        {
            get { return _customerType; }
            set { _customerType = value; OnPropertyChanged(); }
        }
        private string _address;
        public string Address
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged(); }
        }


        public ICommand FirstLoadCM { get; set; }
        public ICommand OpenAddCustomerCM { get; set; }
        public ICommand AddCustomerCM { get; set; }
        public ICommand OpenEditCustomerCM { get; set; }
        public ICommand EditCustomerCM { get; set; }
        public ICommand DeleteCustomerCM { get; set; }
        public CustomerManagementVM()
        {

            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                CustomerList = new ObservableCollection<CustomerDTO>(await Task.Run(() => CustomerService.Ins.GetAllCustomer()));
            });
            OpenAddCustomerCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                AddCustomerWindow wd = new AddCustomerWindow();
                ResetData();
                wd.Show();
            });
            AddCustomerCM = new RelayCommand<Window>(p => { if (IsSaving) return false; return true; }, async p => {
                IsSaving = true;
                await AddNewCustomer(p);
                Reload();
                IsSaving = false;
            });
            OpenEditCustomerCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                EditCustomerWindow wd = new EditCustomerWindow();
                ResetData();
                FullName = SelectedItem.CustomerName;
                Phonenumber = SelectedItem.PhoneNumber;
                Email = SelectedItem.Email;
                Cccd = SelectedItem.CCCD;
                Address = SelectedItem.CustomerAddress;
                CustomerId = SelectedItem.CustomerId;
                wd.birthday.Text = SelectedItem.DateOfBirth.ToString();
                wd.Gender.Text = SelectedItem.Gender;
                wd.custype.Text = SelectedItem.CustomerType;
                wd.ShowDialog();
            });
            EditCustomerCM = new RelayCommand<Window>((p) => { if (IsSaving) return false; return true; }, async (p) =>
            {
                IsSaving = true;
                await EditCustomer(p);
                Reload();
                IsSaving = false;
            });
            DeleteCustomerCM = new RelayCommand<Window>(p => true, async p =>
            {
                var kq = CustomMessageBox.ShowOkCancel("Bạn có chắc muốn xoá khách hàng này không?", "Cảnh báo", "OK", "Cancel", CustomMessageBoxImage.Warning);
                if (kq == CustomMessageBoxResult.OK)
                {
                    (bool issucced, string mess) = await CustomerService.Ins.DeleteCustomer(SelectedItem.CustomerId);
                    if (issucced)
                    {
                        LoadCustomerList(Operation.DELETE);
                        Reload();
                        CustomMessageBox.ShowOk("Xóa thành công!", "Thông báo", "OK", CustomMessageBoxImage.Success);
                    }
                    else CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", CustomMessageBoxImage.Error);
                }
            });
        }

        private void LoadCustomerList(Operation oper, CustomerDTO cus = null)
        {
            switch (oper)
            {
                case Operation.CREATE:
                    CustomerList.Add(cus);
                    break;
                case Operation.UPDATE:
                    var updcus = CustomerList.FirstOrDefault(s => s.CustomerId == cus.CustomerId);
                    CustomerList[CustomerList.IndexOf(updcus)] = cus;
                    break;
                case Operation.DELETE:
                    for (int i = 0; i < CustomerList.Count(); i++)
                    {
                        if (CustomerList[i].CustomerId == SelectedItem.CustomerId)
                        {
                            CustomerList.Remove(CustomerList[i]);
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void ResetData()
        {
            FullName = null;
            Phonenumber = null;
            Email = null;
            Cccd = null;
            Birthday = null;
            Gender = null;
            CustomerType = null;
            Address = null;
        }
        private async void Reload()
        {
            CustomerList = new ObservableCollection<CustomerDTO>(await Task.Run(() => CustomerService.Ins.GetAllCustomer()));
        }
    }
}
