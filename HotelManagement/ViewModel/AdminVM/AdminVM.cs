using CinemaManagementProject.Utilities;
using HotelManagement.DTOs;
using HotelManagement.Model;
using HotelManagement.Model.Services;
using HotelManagement.View.Admin;
using HotelManagement.View.Admin.StatisticalManagement;
using HotelManagement.View.CustomMessageBoxWindow;
using HotelManagement.View.Login;
using HotelManagement.View.Staff;
using HotelManagement.ViewModel.StaffVM.RoomCatalogManagementVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HotelManagement.ViewModel.AdminVM
{
    public class AdminVM : BaseVM
    {

        public static StaffDTO CurrentStaff;

        private string _staffname;
        public string StaffName
        {
            get { return _staffname; }
            set { _staffname = value; OnPropertyChanged(); }
        }
        public string _avatarName { get; set; }
        public string AvatarName
        {
            get { return _avatarName; }
            set { _avatarName = value; OnPropertyChanged(); }
        }
        private string nameTypeMenuBind;
        public string NameTypeMenuBind
        {
            get { return nameTypeMenuBind; }
            set { nameTypeMenuBind = value; OnPropertyChanged(); }
        }
        private ImageSource _imageSource { get; set; }
        public ImageSource AvatarSource
        {
            get { return _imageSource; }
            set { _imageSource = value; OnPropertyChanged(); }
        }

        private string _StaffId;
        public string StaffId
        {
            get { return _StaffId; }
            set { _StaffId = value; OnPropertyChanged(); }
        }
        private object _currentView;
        public object CurrentView 
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }
        private static AdminVM _ins;
        public static AdminVM Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new AdminVM();
                }
                return _ins;
            }
            private set { _ins = value; }
        }

        public static AdminVM adminVM;
   
        public ICommand FirstLoadCM { get; set; }
        private void Furniture(object obj) => CurrentView = new FurnitureManagementVM.FurnitureManagementVM();
        private void Service(object obj) => CurrentView = new ServiceManagementVM.ServiceManagementVM();
        private void RoomFurniture(object obj) => CurrentView = new RoomFurnitureManagementVM.RoomFurnitureManagementVM();
        private void Setting(object obj) => CurrentView = new SettingVM.SettingVM();
        private void BookingRoom(object obj) => CurrentView = new BookingRoomManagementVM.BookingRoomManagementVM();
        private void Room(object obj) => CurrentView = new RoomManagementVM.RoomManagementVM();
        private void RoomType(object obj) => CurrentView = new RoomTypeManagementVM.RoomTypeManagementVM();
        private void Statiscal(object obj) => CurrentView = new StatisticalManagementVM.StatisticalManagementVM();
        private void HelpScreen(object obj) => CurrentView = new HelpScreenVM.HelpScreenVM();
        private void Customer(object obj) => CurrentView = new CustomerManagementVM.CustomerManagementVM();
        private void Staff(object obj) => CurrentView = new StaffManagementVM.StaffManagementVM();
        private void History(object obj) => CurrentView = new HistoryManagementVM.HistoryManagementVM();
        private void Trouble(object obj) => CurrentView = new TroubleManagementVM.TroubleManagementVM();
        private void RoomCatalog(object obj) => CurrentView = new RoomCatalogManagementVM();

        public ICommand FurnitureCommand { get; set; }
        public ICommand ServiceCommand { get; set; }
        public ICommand RoomFurnitureCommand { get; set; }
        public ICommand SettingCommand { get; set; }
        public ICommand BookingRoomCommand { get; set; }
        public ICommand RoomCommand { get; set; }
        public ICommand RoomTypeCommand { get; set; }
        public ICommand StatiscalCommand { get; set; }
        public ICommand HelpScreenCommand { get; set; }
        public ICommand CustomerCommand { get; set; }
        public ICommand StaffCommand { get; set; }
        public ICommand HistoryCommand { get; set; }
        public ICommand TroubleCommand { get; set; }
        public ICommand RoomCatalogCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
        public AdminVM()
        {
            AdminWindow tk =  Application.Current.Windows.OfType<AdminWindow>().FirstOrDefault();

            adminVM = this;
            FurnitureCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Furniture(p);
                NameTypeMenuBind = "QUẢN LÍ TIỆN NGHI";
            });
            ServiceCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Service(p);
                NameTypeMenuBind = "QUẢN LÍ DỊCH VỤ - SẢN PHẨM";
            });
            RoomFurnitureCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                RoomFurniture(p);
                NameTypeMenuBind = "TIỆN NGHI TRONG PHÒNG";
            });
            SettingCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Setting(p);
                NameTypeMenuBind = "CÀI ĐẶT";
            });
            BookingRoomCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                BookingRoom(p);
                NameTypeMenuBind = "QUẢN LÍ ĐẶT PHÒNG";
            });
            RoomCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Room(p);
                NameTypeMenuBind = "QUẢN LÍ PHÒNG";
            });
            RoomTypeCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                RoomType(p);
                NameTypeMenuBind = "QUẢN LÍ LOẠI PHÒNG";
            });
            StatiscalCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Statiscal(p);
                NameTypeMenuBind = "TỔNG QUAN";
            });
            HelpScreenCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                HelpScreen(p);
                NameTypeMenuBind = "GIÚP ĐỠ";
            });
            CustomerCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Customer(p);
                NameTypeMenuBind = "QUẢN LÍ KHÁCH HÀNG";
            });
            StaffCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Staff(p);
                NameTypeMenuBind = "QUẢN LÍ NHÂN VIÊN";
            });
            HistoryCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                History(p);
                NameTypeMenuBind = "TRA CỨU THU CHI";
            });
            TroubleCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Trouble(p);
                NameTypeMenuBind = "QUẢN LÍ SỰ CỐ";
            });
            RoomCatalogCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                RoomCatalog(p);
                NameTypeMenuBind = "QUẢN LÍ SỬ DỤNG PHÒNG";
            });
            FirstLoadCM = new RelayCommand<Rectangle>((p) => { return true; }, (p) =>
            {
                CurrentView = new StatisticalManagementVM.StatisticalManagementVM();
                StaffName = CurrentStaff.StaffName;
                NameTypeMenuBind = "TỔNG QUAN";
                SetAvatarName(StaffName);
                StaffId = CurrentStaff.StaffId;
                if (CurrentStaff.Avatar != null)
                    AvatarSource = LoadAvatarImage(CurrentStaff.Avatar);
                else
                    AvatarSource = null;
                p.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom(Properties.Settings.Default.MainAppColor);
            });
            LogOutCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (CustomMessageBox.ShowOkCancel("Bạn thật sự muốn đăng xuất không?","Cảnh báo", "Đăng xuất", "Không", CustomMessageBoxImage.Warning) == CustomMessageBoxResult.OK)
                {
                    LoginWindow loginwd = new LoginWindow();
                    AdminWindow st = Application.Current.Windows.OfType<AdminWindow>().FirstOrDefault();
                    st.Close();
                    CurrentStaff = null;
                    loginwd.Show();
                }
            });
        }
        public void setNavigateHelpScreen()
        {
            CurrentView = new HelpScreenVM.HelpScreenVM();
        }
        public void SetAvatarName(string staffName)
        {
            string[] trimNames = staffName.Split(' ');
            AvatarName = trimNames[trimNames.Length - 1][0].ToString() + trimNames[0][0].ToString();
        }
        public BitmapImage LoadAvatarImage(byte[] data)
        {
            MemoryStream strm = new MemoryStream();
            strm.Write(data, 0, data.Length);
            strm.Position = 0;
            System.Drawing.Image img = System.Drawing.Image.FromStream(strm);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }
    }
}
