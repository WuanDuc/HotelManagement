using HotelManagement.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotelManagement.View.Login
{
    /// <summary>
    /// Interaction logic for ForgotPassPage.xaml
    /// </summary>
    public partial class ForgotPassPage : Page
    {
        public ForgotPassPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text;
            if (RegexUtilities.IsValidEmail(email))
            {
                if (Helper.CheckEmailStaff(email))
                {
                    EmailKP.Visibility = Visibility.Collapsed;
                    CodeNum.Visibility = Visibility.Visible;
                    btnSendcode.Visibility = Visibility.Collapsed;
                    btnCheckcode.Visibility = Visibility.Visible;
                    InfoText.Text = "Chúng tôi đã gửi mã có 6 chữ số đến tài khoản " + SetHideEmail(EmailBox.Text);
                }
            }
            EmailBox.Focus();

        }
        private string SetHideEmail(string email)
        {
            int start = 0;
            int end = 0;
            int length = email.Length;
            for (int i = 0; i < length; i++)
                if (email[i] == '@')
                    end = i - 1;
            start = end - (int)((float)1 / 3 * (end - start + 1)) + 1;
            string secure = "";
            for (int i = 0; i < end - start + 1; i++)
                secure += '*';
            email = email.Remove(start, length - start) + secure + email.Remove(0, end + 1);
            return email;
        }
    }
}
