using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using HotelManagement.Utilities;
using HotelManagement.View.CustomMessageBoxWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotelManagement.ViewModel.AdminVM.StaffManagementVM
{
    public partial class StaffManagementVM : BaseVM
    {
        private async Task ChangePassword(Window p)
        {
            (bool isvalid, string error) = IsValidPassword();
            if (isvalid)
            {
                (bool issucessed, string messfromservice) = await StaffService.Ins.UpdatePassword(StaffId,Password);
                if (issucessed)
                {                   
                    p.Close();
                    CustomMessageBox.ShowOk(messfromservice, "Thông báo", "OK", CustomMessageBoxImage.Success);
                }
                else
                {
                    CustomMessageBox.ShowOk(messfromservice, "Lỗi", "OK", CustomMessageBoxImage.Error);
                }
            }
            else CustomMessageBox.ShowOk(error, "Cảnh báo", "OK", CustomMessageBoxImage.Warning);
        }
        private (bool isvalid, string mess) IsValidPassword()
        {
            if (String.IsNullOrEmpty(Password))
            {
                return (false, "Vui lòng nhập mật khẩu!");
            }
            if (Password != Repass)
            {
                return (false, "Mật khẩu và mật khẩu nhập lại không trùng khớp!");
            }
            return (true, null);
        }
    }
}
