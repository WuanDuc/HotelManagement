using CinemaManagementProject.Utilities;
using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using HotelManagement.View.Admin;
using HotelManagement.View.Admin.StaffManagement;
using HotelManagement.View.Login;
using HotelManagement.View.Staff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HotelManagement.ViewModel.LoginVM
{
    public class LoginVM : BaseVM
    {
        public Window LoginWindow { get; set; }
        public static string Password { get; set; }
        private string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }
        private StaffDTO _currentStaff { get; set; }
        public StaffDTO CurrentStaff
        {
            get { return _currentStaff; }
            set
            {
                _currentStaff = value;
            }
        }
        private string _staffPosition { get; set; }
        public string StaffPosition
        {
            get { return _staffPosition; }
            set
            {
                _staffPosition = value;
            }
        }
        private string _currentStaffName { get; set; }
        public string CurrentStaffName
        {
            get { return _currentStaffName; }
            set
            {
                _currentStaffName = value;
            }
        }


        public static Frame MainFrame { get; set; }
        public ICommand LoadLoginPageCM { get; set; }
        public ICommand SaveLoginWindowNameCM { get; set; }
        public ICommand PasswordChangedCM { get; set; }
        public ICommand LoadForgotPageCM { get; set; }
        public ICommand LoginCM { get; set; }
        public ICommand GoToMainWorkingWindowCM { get; set; }
        public ICommand LoginPageCM { get; set; }
        public ICommand CloseCM { get; set; }
        public LoginVM()
        {
            LoadLoginPageCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                MainFrame = p;
                if (Properties.Settings.Default.isRemidUserAndPass)
                {
                    Username = Properties.Settings.Default.userNameSetting;
                    Password = Properties.Settings.Default.userPassSetting;
                }
                else
                {
                    Username = "";
                    Password = "";
                }

                p.Content = new LoginPage();
            });
            SaveLoginWindowNameCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                LoginWindow = p;
            });
            PasswordChangedCM = new RelayCommand<PasswordBox>(p => true, p =>
            {
                Password = p.Password;
            });
            LoadForgotPageCM = new RelayCommand<object>(p => true, p =>
            {
                MainFrame.Content = new ForgotPassPage();
            });
            LoginCM = new RelayCommand<Label>(p => true, async p =>
            {
                await CheckLogin(Username, Password, p);
            });
            GoToMainWorkingWindowCM = new RelayCommand<object>(p => true, p =>
            {
                if (CurrentStaff.Position == "Quản lý")
                {
                    LoginWindow.Hide();
                    AdminVM.AdminVM.CurrentStaff = CurrentStaff;
                    AdminWindow wd = new AdminWindow();
                    wd.Show();
                    LoginWindow.Close();
                }
                else if (CurrentStaff.Position == "Nhân viên")
                {
                    LoginWindow.Hide();
                    StaffVM.StaffVM.CurrentStaff = CurrentStaff;
                    StaffWindow wd = new StaffWindow();
                    wd.Show();
                    LoginWindow.Close();
                }
            });
            LoginPageCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainFrame.Content = new LoginPage();
                if (Properties.Settings.Default.isRemidUserAndPass)
                {
                    Username = Properties.Settings.Default.userNameSetting;
                    Password = Properties.Settings.Default.userPassSetting;
                }
                else
                {
                    Username = "";
                    Password = "";
                }
            });
            CloseCM = new RelayCommand<Window>(p => true, p =>
            {
                p.Close();
            });
        }

        public async Task CheckLogin(string username, string password, Label p)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                p.Content = "Vui lòng nhập đủ thông tin";
                return;
            }
            (bool isSuccess, string mess, StaffDTO staff) = await StaffService.Ins.CheckLogin(username, password);
            CurrentStaff = staff;
            if (isSuccess)
            {
                password = "";
                StaffPosition = staff.Position;
                CurrentStaffName = staff.StaffName;
                MainFrame.Content = new PositionPage();
            }
            else
            {
                p.Content = mess;
                return;
            }
        }
        Window GetWindowParent(Window p)
        {
            Window parent = p;

            while (parent.Parent != null)
            {
                parent = parent.Parent as Window;
            }

            return parent;
        }
    }
}
