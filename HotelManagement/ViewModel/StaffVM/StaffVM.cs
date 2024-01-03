using CinemaManagementProject.Utilities;
using HotelManagement.DTOs;
using HotelManagement.View.Admin;
using HotelManagement.View.CustomMessageBoxWindow;
using HotelManagement.View.Login;
using HotelManagement.View.Staff;
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

namespace HotelManagement.ViewModel.StaffVM
{
    public class StaffVM : BaseVM
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
        private string nameTypeMenuBind;
        public string NameTypeMenuBind
        {
            get { return nameTypeMenuBind; }
            set { nameTypeMenuBind = value; OnPropertyChanged(); }
        }
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }
        private static StaffVM _ins;
        public static StaffVM Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new StaffVM();
                }
                return _ins;
            }
            private set { _ins = value; }
        }

        public static StaffVM staffVM;
        private void HelpScreen(object obj) => CurrentView = new HelpScreenVM.HelpScreenVM();
        private void BookingRoom(object obj) => CurrentView = new BookingRoomManagementVM.BookingRoomManagementVM();
        private void RoomCatalog(object obj) => CurrentView = new RoomCatalogManagementVM.RoomCatalogManagementVM();
        private void TroubleRp(object obj) => CurrentView = new TroubleReportVM.TroubleReportVM();
        private void Setting(object obj) => CurrentView = new SettingVM.SettingVM();

        public ICommand BookingRoomCommand { get; set; }
        public ICommand FirstLoadCM { get; set; }
        public ICommand RoomCatalogCommand { get; set; }
        public ICommand TroubleRpCommand { get; set; }
        public ICommand HelpScreenCommand { get; set; }
        public ICommand SettingCommand { get; set; }
        public ICommand LogOutCommand { get; set; }

        public StaffVM()
        {
            staffVM = this;
            StaffWindow tk = Application.Current.Windows.OfType<StaffWindow>().FirstOrDefault();
            RoomCatalogCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                RoomCatalog(p);
                NameTypeMenuBind = "QUẢN LÍ SỬ DỤNG PHÒNG";
            });
            TroubleRpCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                TroubleRp(p);
                NameTypeMenuBind = "BÁO CÁO SỰ CỐ";
            });
            BookingRoomCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                BookingRoom(p);
                NameTypeMenuBind = "QUẢN LÍ ĐẶT PHÒNG";
            });
            HelpScreenCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                HelpScreen(p);
                NameTypeMenuBind = "GIÚP ĐỠ";
            });
            SettingCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Setting(p);
                NameTypeMenuBind = "CÀI ĐẶT";
            });

            FirstLoadCM = new RelayCommand<Rectangle>((p) => { return true; }, (p) =>
            {
                StaffName = CurrentStaff.StaffName;
                CurrentView = new RoomCatalogManagementVM.RoomCatalogManagementVM();
                NameTypeMenuBind = "QUẢN LÍ SỬ DỤNG PHÒNG";
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
                if (CustomMessageBox.ShowOkCancel("Bạn thật sự muốn đăng xuất không?", "Cảnh báo", "Đăng xuất", "Không", CustomMessageBoxImage.Warning) == CustomMessageBoxResult.OK)
                {
                    LoginWindow loginwd = new LoginWindow();
                    StaffWindow st = Application.Current.Windows.OfType<StaffWindow>().FirstOrDefault();
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
