using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using HotelManagement.Utilities;
using HotelManagement.View.Admin.StaffManagement;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using System.IO;
using HotelManagement.Model;
using HotelManagement.View.CustomMessageBoxWindow;
using HotelManagement.Utils;

namespace HotelManagement.ViewModel.AdminVM.StaffManagementVM
{
    public partial class StaffManagementVM : BaseVM
    {
        private ObservableCollection<StaffDTO> _staffList;
        public ObservableCollection<StaffDTO> StaffList
        {
            get { return _staffList; }
            set { _staffList = value; OnPropertyChanged(); }
        }

        private StaffDTO _selectedItem;
        public StaffDTO SelectedItem
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
        private string _staffId;
        public string StaffId
        {
            get { return _staffId; }
            set { _staffId = value; OnPropertyChanged(); }
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
        private Nullable<System.DateTime> _startdate;
        public Nullable<System.DateTime> Startdate
        {
            get { return _startdate; }
            set { _startdate = value; OnPropertyChanged(); }
        }
        private ComboBoxItem _gender;
        public ComboBoxItem Gender
        {
            get { return _gender; }
            set { _gender = value; OnPropertyChanged(); }
        }
        private ComboBoxItem _position;
        public ComboBoxItem Position
        {
            get { return _position; }
            set { _position = value; OnPropertyChanged(); }
        }
        private string _address;
        public string Address
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged(); }
        }
        private string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }
        private string _repass;
        public string Repass
        {
            get { return _repass; }
            set { _repass = value; OnPropertyChanged(); }
        }
        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set { _imageSource = value; OnPropertyChanged(); }
        }
        private bool IsImageChanged = false;
        private string filepath;

        public ICommand FirstLoadCM { get; set; }
        public ICommand OpenAddStaffCM { get; set; }
        public ICommand AddStaffCM { get; set; }
        public ICommand UploadImgCM { get; set; }
        public ICommand OpenEditStaffCM { get; set; }
        public ICommand DeleteStaffCM { get; set; }
        public ICommand EditStaffCM { get; set; }
        public ICommand OpenChangePasswordCM { get; set; }
        public ICommand ChangePasswordCM { get; set; }
        public StaffManagementVM()
        {
            FirstLoadCM = new RelayCommand<object>(p => true, async p =>
            {
                StaffList = new ObservableCollection<StaffDTO>(await StaffService.Ins.GetAllStaff());
            });
            OpenAddStaffCM = new RelayCommand<object>(p => true, async p =>
            {
                AddStaffWindow wd = new AddStaffWindow();
                Repass = null;
                ResetData();
                wd.ShowDialog();
            });
            AddStaffCM = new RelayCommand<Window>(p => { if (IsSaving) return false; return true; }, async p =>
            {
                IsSaving = true;
                await AddStaff(p);
                Reload();
                IsSaving = false;
            });
            UploadImgCM = new RelayCommand<Window>(p => true, async p =>
            {
                OpenFileDialog openfile = new OpenFileDialog();
                openfile.Title = "Select an image";
                openfile.Filter = "Image File (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg; *.png";
                if (openfile.ShowDialog() == DialogResult.OK)
                {
                    IsImageChanged = true;
                    filepath = openfile.FileName;
                    LoadImage();
                    return;
                }
                IsImageChanged = false;
            });
            OpenEditStaffCM = new RelayCommand<object>(p => true, async p =>
            {
                EditStaffWindow wd = new EditStaffWindow();
                ResetData();
                StaffId = SelectedItem.StaffId;
                FullName = SelectedItem.StaffName;
                Phonenumber = SelectedItem.PhoneNumber;
                Email = SelectedItem.Email;
                Cccd = SelectedItem.CCCD;
                Address = SelectedItem.StaffAddress;
                Username = SelectedItem.Username;
                wd.Gender.Text = SelectedItem.Gender;
                wd.Position.Text = SelectedItem.Position;
                wd.Birthday.Text = SelectedItem.DateOfBirth.ToString();
                wd.Startdate.Text = SelectedItem.dateOfStart.ToString();
                ImageSource = Helper.LoadBitmapImage(SelectedItem.Avatar);
                wd.ShowDialog();
            });
            EditStaffCM = new RelayCommand<Window>(p => { if (IsSaving) return false; return true; }, async p =>
            {
                IsSaving = true;
                await EditStaff(p);
                Reload();
                IsSaving = false;
            });
            OpenChangePasswordCM = new RelayCommand<object>(p => true, async p =>
            {
                ChangePasswordWindow wd = new ChangePasswordWindow();
                Password = null;
                Repass = null;
                wd.ShowDialog();
            });
            DeleteStaffCM = new RelayCommand<Window>(p => true, async p =>
            {
                var kq = CustomMessageBox.ShowOkCancel("Bạn có chắc muốn xoá nhân viên này không?", "Cảnh báo", "OK", "Cancel", CustomMessageBoxImage.Warning);
                if (kq == CustomMessageBoxResult.OK)
                {
                    (bool issucced, string mess) = await StaffService.Ins.DeleteStaff(SelectedItem.StaffId);
                    if (issucced)
                    {
                        LoadStaffList(Operation.DELETE);
                        Reload();
                        CustomMessageBox.ShowOk("Xóa thành công!", "Thông báo", "OK", CustomMessageBoxImage.Success);
                    }
                    else CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", CustomMessageBoxImage.Error);
                }
            });
            ChangePasswordCM = new RelayCommand<Window>(p => { if (IsSaving) return false; return true; }, async p =>
            {
                IsSaving = true;
                await ChangePassword(p);
                IsSaving = false;
            });
        }

        private void LoadImage()
        {
            BitmapImage _image = new BitmapImage();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = new Uri(filepath, UriKind.RelativeOrAbsolute);
            _image.EndInit();
            ImageSource = _image;

        }

        private async void Reload()
        {
            StaffList = new ObservableCollection<StaffDTO>(await StaffService.Ins.GetAllStaff());
        }

        private async void ResetData()
        {
            StaffId = null;
            FullName = null;
            Phonenumber = null;
            Email = null;
            Cccd = null;
            Startdate = null;
            Birthday = null;
            Gender = null;
            Position = null;
            Address = null;
            Username = null;
            Password = null;
            ImageSource = null;
            Repass = null;
        }
        private void LoadStaffList(Operation oper, StaffDTO staff = null)
        {
            switch (oper)
            {
                case Operation.CREATE:
                    StaffList.Add(staff);
                    break;
                case Operation.UPDATE:
                    var updstaff = StaffList.FirstOrDefault(s => s.StaffId == staff.StaffId);
                    StaffList[StaffList.IndexOf(updstaff)] = staff;
                    break;
                case Operation.DELETE:
                    for (int i = 0; i < StaffList.Count(); i++)
                    {
                        if (StaffList[i].StaffId == SelectedItem.StaffId)
                        {
                            StaffList.Remove(StaffList[i]);
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        private (bool isvalid, string mess) IsValidData(Operation oper)
        {
            if (oper == Operation.CREATE || oper == Operation.UPDATE_PASSWORD)
            {
                if (String.IsNullOrEmpty(Password))
                {
                    return (false, "Vui lòng nhập mật khẩu!");
                }
                if (Password != Repass)
                {
                    return (false, "Mật khẩu và mật khẩu nhập lại không trùng khớp!");
                }
            }
            if (String.IsNullOrEmpty(FullName) || String.IsNullOrEmpty(Phonenumber) || String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(Address) || String.IsNullOrEmpty(Cccd) || Gender is null || Startdate is null || Birthday is null)
            {
                return (false, "Vui lòng nhập đủ thông tin nhân viên!");
            }
            (bool isv, string err) = IsValidAge((DateTime)Birthday);
            if (!isv) return (false, err);
            if (!Helper.IsPhoneNumber(Phonenumber)) return (false, "Số điện thoại không hợp lệ!");
            return (true, null);
        }

        private (bool isv, string err) IsValidAge(DateTime birthday)
        {
            // Save today's date.
            var today = DateTime.Today;

            // Calculate the age.
            var age = today.Year - birthday.Year;

            // Go back to the year in which the person was born in case of a leap year
            if (birthday.DayOfYear > today.DayOfYear) age--;

            if (age < 18) return (false, "Nhân viên chưa đủ 18 tuổi!");
            return (true, null);
        }
    }
}
