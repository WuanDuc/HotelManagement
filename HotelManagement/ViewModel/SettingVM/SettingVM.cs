using System;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System.Windows.Media;
using System.Net.Cache;
using System.Windows.Media.Imaging;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;
using HotelManagement.DTOs;
using HotelManagement.Utilities;
using HotelManagement.View.SettingManagement;
using HotelManagement.View.Admin;
using HotelManagement.Model.Services;
using HotelManagement.Utils;
using HotelManagement.Model;
using HotelManagement.View;
using HotelManagement.View.Staff;
using CinemaManagementProject.Utilities;

namespace HotelManagement.ViewModel.SettingVM
{
    public partial class SettingVM : BaseVM
    {
        public StaffDTO currentStaff;
        public static Window parentWindow;
        private string _staffName { get; set; }
        public string StaffName
        {
            set
            {
                _staffName = value;
                OnPropertyChanged();
            }
            get { return _staffName; }
        }

        private string _staffEmail { get; set; }
        public string StaffEmail
        {
            set
            {
                _staffEmail = value;
                OnPropertyChanged();
            }
            get { return _staffEmail; }
        }
        private string _currentCode { get; set; }
        public string CurrentCode
        {
            get { return _currentCode; }
            set { _currentCode = value; }
        }
        private string _position { get; set; }
        public string Position
        {
            set
            {
                _position = value;
                OnPropertyChanged();
            }
            get { return _position; }
        }
        public string _avatarName { get; set; }
        public string AvatarName
        {
            get { return _avatarName; }
            set { _avatarName = value; OnPropertyChanged(); }
        }
        private bool _isEdit { get; set; }
        private ConfirmWindow confirmWD { get; set; }
        public bool IsEdit
        {
            get { return _isEdit; }
            set { _isEdit = value; OnPropertyChanged(); }
        }
        private bool _isEditEmail { get; set; }
        public bool IsEditEmail
        {
            get { return _isEditEmail; }
            set { _isEditEmail = value; OnPropertyChanged(); }
        }
        private int randomCode;
        private string _error { get; set; }
        public string Error
        {
            get { return _error; }
            set { _error = value; OnPropertyChanged(); }
        }
        private ImageSource _imageSource { get; set; }
        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set { _imageSource = value; OnPropertyChanged(); }
        }
        private MaterialDesignThemes.Wpf.PackIconKind iconEditEmail { get; set; }
        public MaterialDesignThemes.Wpf.PackIconKind IconEditEmail
        {
            get { return iconEditEmail; }
            set { iconEditEmail = value; OnPropertyChanged(); }
        }
        public string filePath;
        private RegistryKey reg { get; set; }
        private bool _isCheckedAutoStart { get; set; }
        public bool IsCheckedAutoStart
        {
            get { return _isCheckedAutoStart; }
            set { _isCheckedAutoStart = value; OnPropertyChanged(); }
        }

        private bool _isCheckedRemindLogin { get; set; }
        public bool IsCheckedRemindLogin
        {
            get { return _isCheckedRemindLogin; }
            set { _isCheckedRemindLogin = value; OnPropertyChanged(); }
        }
        private Brush _colorPicked { get; set; }
        public Brush ColorPicked
        {
            get { return _colorPicked; }
            set { _colorPicked = value; OnPropertyChanged(); }
        }
        private bool _isToResetPage { get; set; }
        public bool IsToResetPage
        {
            get { return _isToResetPage; }
            set { _isToResetPage = value; OnPropertyChanged(); }
        }

        private bool _isEnglish { get; set; }
        public bool IsEnglish
        {
            get { return _isEnglish; }
            set { _isEnglish = value; OnPropertyChanged(); }
        }
        public ICommand FirstLoadCM { get; set; }
        public ICommand EditNameCM { get; set; }
        public ICommand EditEmailCM { get; set; }
        public ICommand ConfirmButtonCM { get; set; }
        public ICommand UploadImageCM { get; set; }
        public ICommand ChangePassCM { get; set; }
        public ICommand AutoStartAppCM { get; set; }
        public ICommand RemindLoginAppCM { get; set; }
        public ICommand ColorPickerCM { get; set; }
        public ICommand CLoseColorPickerCM { get; set; }
        public ICommand ChooseColorCM { get; set; }
        public ICommand ConfirmCurrentPassCM { get; set; }
        public ICommand CloseResetPassCM { get; set; }
        public ICommand ChooseLanguageCM { get; set; }
        public ICommand ConfirmNewPassCM { get; set; }
        AdminWindow tk;
        StaffWindow st;
        public SettingVM()
        {
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (AdminVM.AdminVM.CurrentStaff != null)
                {
                    currentStaff = AdminVM.AdminVM.CurrentStaff;
                    tk = Application.Current.Windows.OfType<AdminWindow>().FirstOrDefault();
                }
                else
                {
                    currentStaff = StaffVM.StaffVM.CurrentStaff;
                    st = Application.Current.Windows.OfType<StaffWindow>().FirstOrDefault();
                }
                StaffName = currentStaff.StaffName;
                StaffEmail = currentStaff.Email;
                Position = currentStaff.Position;
                if (reg.GetValue("HotelManagementApp") == null)
                    IsCheckedAutoStart = false;
                else
                    IsCheckedAutoStart = true;
                CheckedRemindLogin();

                if (currentStaff.Avatar != null)
                    ImageSource = LoadAvatarImage(currentStaff.Avatar);
                else
                    ImageSource = null;
                ColorPicked = (SolidColorBrush)new BrushConverter().ConvertFrom(Properties.Settings.Default.MainAppColor);
                IsToResetPage = false;
                IsEditEmail = false;
                IconEditEmail = PackIconKind.Pencil;
                SetAvatarName(StaffName);
                IsEdit = false;
            });
            EditNameCM = new RelayCommand<MaterialDesignThemes.Wpf.PackIcon>((p) => { return true; }, async (p) =>
            {
                if (string.IsNullOrEmpty(StaffName))
                {
                    CustomMessageBox.ShowOk(IsEnglish ? "Do not leave the blank name!" : "Không được để tên trống!", IsEnglish ? "Warning" : "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                    return;
                }
                if (IsEdit == false)
                {
                    p.Kind = MaterialDesignThemes.Wpf.PackIconKind.ContentSaveEdit;
                    IsEdit = true;
                }
                else
                {
                    (bool isSuccessEdit, string messageReturn) = await Task.Run(() => SettingService.Ins.EditName(StaffName, currentStaff.StaffId));
                    if (isSuccessEdit == false)
                    {
                        CustomMessageBox.ShowOkCancel(messageReturn, IsEnglish ? "Error" : "Lỗi", "OK", IsEnglish ? "Cancel" : "Hủy", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                        return;
                    }
                    currentStaff.StaffName = StaffName;
                    SetAvatarName(StaffName);
                    if (AdminVM.AdminVM.CurrentStaff != null)
                    {
                        tk.FullName.Text = StaffName;
                        tk.AvatarName.Text = AvatarName;
                    }
                    else
                    {
                        st.FullName.Text = StaffName;
                        st.AvatarName.Text = AvatarName;
                    }
                    CustomMessageBox.ShowOkCancel(messageReturn, IsEnglish ? "Success" : "Thành công", "OK", IsEnglish ? "Cancel" : "Hủy", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                    p.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pencil;
                    IsEdit = false;
                }
            });
            EditEmailCM = new RelayCommand<MaterialDesignThemes.Wpf.PackIcon>((p) => { return true; }, async (p) =>
            {
                if (string.IsNullOrEmpty(StaffEmail))
                {
                    CustomMessageBox.ShowOk(IsEnglish ? "Do not leave the blank name!" : "Không được để mail trống!", IsEnglish ? "Warning" : "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                    return;
                }
                if (IsEditEmail == false)
                {
                    p.Kind = MaterialDesignThemes.Wpf.PackIconKind.ContentSaveEdit;
                    IconEditEmail = PackIconKind.ContentSaveEdit;
                    IsEditEmail = true;
                }
                else
                {
                    if (!EmailFormat.IsValidEmail(StaffEmail))
                    {
                        CustomMessageBox.ShowOkCancel(IsEnglish ? "Invalid Email" : "Email không đúng", IsEnglish ? "Error" : "Lỗi", "Ok", IsEnglish ? "Cancel" : "Hủy", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        Random randomNumber = new Random();
                        randomCode = randomNumber.Next(111111, 999999);
                        await SendMailToStaff(StaffEmail, randomCode);

                        confirmWD = new ConfirmWindow();
                        confirmWD.ShowDialog();
                        p.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pencil;
                        IsEditEmail = false;
                    }

                }
            });
            ConfirmButtonCM = new RelayCommand<Label>((p) => { return true; }, async (p) =>
            {
                if (CurrentCode == randomCode.ToString())
                {
                    if (string.IsNullOrEmpty(StaffName))
                    {
                        CustomMessageBox.ShowOk(IsEnglish ? "Do not leave the blank name!" : "Không được để tên trống!", IsEnglish ? "Warning" : "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                        return;
                    }
                    (bool isSuccessEdit, string messageReturn) = await Task.Run(() => SettingService.Ins.EditEmail(StaffEmail, currentStaff.StaffId));
                    if (isSuccessEdit == false)
                    {
                        CustomMessageBox.ShowOkCancel(messageReturn, IsEnglish ? "Error" : "Lỗi", "OK", IsEnglish ? "Cancel" : "Hủy", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                        return;
                    }
                    currentStaff.Email = StaffEmail;
                    CustomMessageBox.ShowOkCancel(messageReturn, IsEnglish ? "Success" : "Thành công", "OK", IsEnglish ? "Cancel" : "Hủy", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                    IconEditEmail = PackIconKind.Pencil;
                    confirmWD.Close();
                    IsEditEmail = false;
                }
                else
                    Error = IsEnglish ? "This code is invalid!" : "Mã code vừa nhập chưa chính xác!";
            });
            UploadImageCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                OpenFileDialog openfile = new OpenFileDialog();
                openfile.Title = "Select an image";
                openfile.Filter = "Image File (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg; *.png";
                if (openfile.ShowDialog() == true)
                {
                    filePath = openfile.FileName;
                    LoadImage();

                    using (var context = new HotelManagementEntities())
                    {
                        Staff updateStaff = context.Staffs.FirstOrDefault(x => x.StaffId == currentStaff.StaffId);

                        if (updateStaff == null)
                            return;
                        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                        byte[] photo_aray = new byte[fs.Length];
                        fs.Read(photo_aray, 0, photo_aray.Length);
                        updateStaff.Avatar = photo_aray;
                        currentStaff.Avatar = photo_aray;
                        if (AdminVM.AdminVM.CurrentStaff != null)
                        {
                            tk.IconAvatar.ImageSource = LoadAvatarImage(photo_aray);
                        }
                        else
                        {
                            st.IconAvatar.ImageSource = LoadAvatarImage(photo_aray);
                        }
                        context.SaveChanges();
                        CustomMessageBox.ShowOk(IsEnglish ? "Update successful" : "Cập nhật thành công", IsEnglish ? "Notice" : "Thông báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                    }
                }
            });
            ChangePassCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                IsToResetPage = false;
                ResetPassWindow resetPassWD = new ResetPassWindow();
                resetPassWD.ShowDialog();
            });
            ConfirmNewPassCM = new RelayCommand<PasswordBox>((p) => { return true; }, async (p) =>
            {
                try
                {
                    using (var db = new HotelManagementEntities())
                    {
                        Staff updateStaff = await db.Staffs.FirstOrDefaultAsync(x => x.StaffId == currentStaff.StaffId);

                        if (updateStaff == null)
                            return;

                        updateStaff.Password = p.Password;
                        await db.SaveChangesAsync();
                        currentStaff.Password = p.Password;
                        CustomMessageBox.ShowOk(IsEnglish ? "Update successful" : "Cập nhật thành công", IsEnglish ? "Notice" : "Thông báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                    }
                }
                catch (EntityException)
                {
                    CustomMessageBox.ShowOk(IsEnglish ? "Lost database connection" : "Mất kết nối cơ sở dữ liệu", IsEnglish ? "Error" : "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                }
                catch (Exception e)
                {
                    CustomMessageBox.ShowOk(IsEnglish ? "System Error" : "Lỗi hệ thống", IsEnglish ? "Error" : "Lỗi", "Ok", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                }
            });
            AutoStartAppCM = new RelayCommand<ToggleButton>((p) => { return true; }, (p) =>
            {
                if (p.IsChecked == true)
                    reg.SetValue("HotelManagementApp", System.Reflection.Assembly.GetExecutingAssembly().Location);
                else
                    reg.DeleteValue("HotelManagementApp");
            });
            RemindLoginAppCM = new RelayCommand<ToggleButton>((p) => { return true; }, (p) =>
            {
                if (p.IsChecked == true)
                {
                    Properties.Settings.Default.userNameSetting = currentStaff.Username;
                    Properties.Settings.Default.userPassSetting = currentStaff.Password;
                    Properties.Settings.Default.isRemidUserAndPass = true;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.userNameSetting = string.Empty;
                    Properties.Settings.Default.userPassSetting = string.Empty;
                    Properties.Settings.Default.isRemidUserAndPass = false;
                    Properties.Settings.Default.Save();
                }
            });
            ColorPickerCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                p.Visibility = Visibility.Visible;
            });
            CLoseColorPickerCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                p.Visibility = Visibility.Collapsed;
            });
            ChooseColorCM = new RelayCommand<Rectangle>((p) => { return true; }, (p) =>
            {
                ColorPicked = p.Fill;
                if (AdminVM.AdminVM.CurrentStaff != null)
                {
                    tk.Overlay.Fill = p.Fill;
                }
                else
                {
                    st.Overlay.Fill = p.Fill;
                }
                SolidColorBrush solidColorBrush = (SolidColorBrush)ColorPicked;
                Properties.Settings.Default.MainAppColor = solidColorBrush.Color.ToString();
                Properties.Settings.Default.Save();
            });
            ConfirmCurrentPassCM = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                if (string.IsNullOrEmpty(p.Password))
                {
                    CustomMessageBox.ShowOk("Mật khẩu trống, xin vui lòng nhập mật khẩu!", "Thông báo", "Xác nhận", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                    return;
                }
                if (p.Password == currentStaff.Password)
                {
                    IsToResetPage = true;
                    Error = string.Empty;
                }
                else
                    Error = IsEnglish ? "Wrong password" : "Sai mật khẩu";
            });
            CloseResetPassCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
                IsToResetPage = false;
                Error = string.Empty;
            });
            ChooseLanguageCM = new RelayCommand<ComboBox>((p) => { return true; }, (p) =>
            {
                if (p.SelectedItem == null)
                    return;
                bool isEN = !p.Text.Equals("English") ? true : false;
                Properties.Settings.Default.isEnglish = isEN;
                Properties.Settings.Default.Save();
                LanguageManager.SetLanguageDictionary(isEN ? LanguageManager.ELanguage.English : LanguageManager.ELanguage.VietNamese);
            });
        }
        public void SetAvatarName(string staffName)
        {
            string[] trimNames = staffName.Split(' ');
            AvatarName = trimNames[trimNames.Length - 1][0].ToString() + trimNames[0][0].ToString();
        }
        public async Task SendMailToStaff(string staffEmail, int randomCode)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string APP_EMAIL = appSettings["APP_EMAIL"];
                string APP_PASSWORD = appSettings["APP_PASSWORD"];

                //Tạo mail
                MailMessage mail = new MailMessage(APP_EMAIL, staffEmail);
                mail.To.Add(staffEmail);
                mail.Subject = IsEnglish ? "Verification of user accounts" : "Xác minh tài khoản người dùng";
                //Attach file
                mail.IsBodyHtml = true;

                string htmlBody;

                htmlBody = GetHTMLTemplate(randomCode.ToString());

                mail.Body = htmlBody;

                //tạo Server

                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                SmtpServer.Port = 587;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential(APP_EMAIL, APP_PASSWORD);
                SmtpServer.EnableSsl = true;
                await SmtpServer.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowOk(IsEnglish ? "System Error" : "Lỗi hệ thống", IsEnglish ? "Error" : "Lỗi", "Ok", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
            }
        }
        public void LoadImage()
        {
            BitmapImage _image = new BitmapImage();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = new Uri(filePath, UriKind.RelativeOrAbsolute);
            _image.EndInit();
            ImageSource = _image;
        }
        private void CheckedRemindLogin()
        {
            if (currentStaff.Username == Properties.Settings.Default.userNameSetting)
                if (Properties.Settings.Default.isRemidUserAndPass)
                    IsCheckedRemindLogin = true;
                else
                    IsCheckedRemindLogin = false;
            else
                IsCheckedRemindLogin = false;
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
        public string GetHTMLTemplate(string ConfirmCode)
        {
            string resetPasswordTemplate = Helper.GetEmailTemplatePath(RESET_PASSWORD_FILE);
            string HTML = File.ReadAllText(resetPasswordTemplate).Replace("{RESET_PASSWORD_CODE}", ConfirmCode);
            return HTML;
        }
        const string RESET_PASSWORD_FILE = "EmailResetPassTemplate.txt";
    }
}
