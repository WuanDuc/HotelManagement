using HotelManagement.View.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Mail;
using System.Configuration;
using System.Windows.Controls;
using HotelManagement.Utilities;
using System.Net;
using System.Windows;
using System.IO;
using HotelManagement.Model;
using System.Data.Entity;
using CinemaManagementProject.Utilities;

namespace HotelManagement.ViewModel.LoginVM
{
    public class ForgotPassVM : BaseVM
    {
        public bool IsLoadding = false;
        private string _currentEmail;
        public string CurrentEmail
        {
            get { return _currentEmail; }
            set { _currentEmail = value; OnPropertyChanged(); }
        }
        private string _currentCode;
        public string CurrentCode
        {
            get { return _currentCode; }
            set { _currentCode = value; OnPropertyChanged(); }
        }
        private string _newPass;
        public string NewPass
        {
            get { return _newPass; }
            set { _newPass = value; OnPropertyChanged(); }
        }
        private string _rePass;
        public string RePass
        {
            get { return _rePass; }
            set { _rePass = value; OnPropertyChanged(); }
        }
        private int randomCode;

        public ICommand LoginPageCM { get; set; }
        public ICommand SendCodeCM { get; set; }
        public ICommand CheckCodeCM { get; set; }
        public ICommand ConfirmCodeCM { get; set; }
        public ForgotPassVM()
        {
            LoginPageCM = new RelayCommand<object>(p => true, p =>
            {
                LoginVM.MainFrame.Content = new LoginPage();
            });
            SendCodeCM = new RelayCommand<Label>(p => true, async p =>
            {
                if (CheckValidEmail(CurrentEmail, p))
                {
                    if (Helper.CheckEmailStaff(CurrentEmail))
                    {
                        Random randomNumber = new Random();
                        randomCode = randomNumber.Next(111111, 999999);
                        IsLoadding = true;
                        await SendMailToStaff(CurrentEmail, randomCode);
                        IsLoadding = false;
                    }
                    else p.Content = "Không phải tài khoản nhân viên công ty!";
                }
            });
            CheckCodeCM = new RelayCommand<Label>(p => true, p =>
            {
                if (CurrentCode == randomCode.ToString())
                {
                    LoginVM.MainFrame.Content = new ChangePassPage();
                }
                else p.Content = "Mã code vừa nhập chưa chính xác!";
            });
            ConfirmCodeCM = new RelayCommand<Label>(p => true, async p =>
            {
                if (NewPass == RePass)
                {
                    using (var context = new HotelManagementEntities())
                    {
                        var staff = await context.Staffs.FirstOrDefaultAsync(s => s.Email == CurrentEmail);
                        if (staff == null) return;
                        staff.Password = NewPass;
                        await context.SaveChangesAsync();
                    }
                    LoginVM.MainFrame.Content = new LoginPage();
                    CurrentEmail = "";
                    CurrentCode = "";
                }
                else p.Content = "Mật khẩu nhập lại không trùng khớp!";
            });
        }

        private async Task SendMailToStaff(string currentEmail, int randomCode)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string APP_EMAIL = appSettings["APP_EMAIL"];
                string APP_PASSWORD = appSettings["APP_PASSWORD"];

                //Tạo mail
                MailMessage mail = new MailMessage(APP_EMAIL, currentEmail);
                mail.To.Add(currentEmail);
                mail.Subject = "Lấy lại mật khẩu đăng nhập";
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
                MessageBox.Show(ex.ToString());
            }
        }

        private string GetHTMLTemplate(string code)
        {
            string resetPasswordTemplate = Helper.GetEmailTemplatePath(RESET_PASSWORD_FILE);
            string HTML = File.ReadAllText(resetPasswordTemplate).Replace("{RESET_PASSWORD_CODE}", code);
            return HTML;
        }
        const string RESET_PASSWORD_FILE = "EmailResetPassTemplate.txt";

        private bool CheckValidEmail(string currentEmail, Label p)
        {
            if (string.IsNullOrEmpty(currentEmail))
            {
                p.Content = "Vui lòng nhập đủ thông tin";
                return false;
            }
            if (!RegexUtilities.IsValidEmail(currentEmail))
            {
                p.Content = "Vui lòng nhập đúng địa chỉ Email";
                return false;
            }
            p.Content = "";
            return true;
        }
    }
}
