using HotelManagement.View.Admin;
using HotelManagement.View.HelpScreen;
using HotelManagement.ViewModel.AdminVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace HotelManagement.ViewModel.HelpScreenVM
{
    public class HelpScreenVM : BaseVM
    {
        public ICommand CloseCM { get; set; }
        public ICommand Load_AboutUs { get; set; }
        public ICommand Load_TermOfUse { get; set; }
        public ICommand Load_Frequently_asked_questions { get; set; }
        public ICommand Load_PrivacyPolicy { get; set; }
        public ICommand FB_Group_Command { get; set; }

        public HelpScreenVM()
        {

            FB_Group_Command = new RelayCommand<object>((uri) => { return true; }, (uri) =>
            {
                string myUri = !uri.ToString().Contains("https://") && !uri.ToString().Contains("http://") ? "http://" + uri.ToString() : uri.ToString();
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(myUri));
                if (AdminVM.AdminVM.CurrentStaff != null)
                {
                    AdminVM.AdminVM.adminVM.setNavigateHelpScreen();
                }
                else
                {
                    StaffVM.StaffVM.staffVM.setNavigateHelpScreen();
                }
            });
            Load_PrivacyPolicy = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Window w1 = new PrivacyPolicy();
                w1.ShowDialog();
            });
            Load_TermOfUse = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Window w1 = new TermOfUse();
                w1.ShowDialog();
            });
            Load_AboutUs = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Window w1 = new AboutUs();
                w1.ShowDialog();
            });
            Load_Frequently_asked_questions = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Window w1 = new Frequently_asked_questions();
                w1.ShowDialog();
            });
            CloseCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
        }



    }
}
