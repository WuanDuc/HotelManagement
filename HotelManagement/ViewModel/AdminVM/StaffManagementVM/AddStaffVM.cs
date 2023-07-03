using HotelManagement.View.CustomMessageBoxWindow;
using System;
using HotelManagement.Utilities;
using System.Threading.Tasks;
using System.Windows;
using HotelManagement.DTOs;
using System.IO;
using HotelManagement.Model.Services;
using HotelManagement.Utils;
using System.Linq;

namespace HotelManagement.ViewModel.AdminVM.StaffManagementVM
{
    public partial class StaffManagementVM : BaseVM
    {
        private async Task AddStaff(Window p)
        {
            if (Email != null)
            {
                if (Email.Trim() == "") Email = null;
                else
                {
                    if (!Utilities.RegexUtilities.IsValidEmail(Email))
                    {
                        CustomMessageBox.ShowOk("Email không hợp lệ", "Cảnh báo", "OK", CustomMessageBoxImage.Warning);
                        return;
                    }
                }
            }
            foreach (var i in Cccd)
            {
                if (!"0123456789".Contains(i))
                {
                    CustomMessageBox.ShowOk("Sai định dạng CCCD!", "Thông Báo", "OK", CustomMessageBoxImage.Warning);
                    return;
                }
            }
            if (Cccd.Length != 12)
            {
                CustomMessageBox.ShowOk("Sai định dạng CCCD!", "Thông Báo", "OK", CustomMessageBoxImage.Warning);
                return;
            }
            (bool isvalid, string mess) =  IsValidData(Operation.CREATE);
            if (isvalid)
            {
                FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                byte[] photo_aray = new byte[fs.Length];
                fs.Read(photo_aray, 0, photo_aray.Length);

                StaffDTO staffDTO = new StaffDTO
                {
                    StaffName = FullName,
                    Email = Email,
                    PhoneNumber = Phonenumber,
                    CCCD = Cccd,
                    StaffAddress=Address,
                    DateOfBirth = (DateTime)Birthday,
                    dateOfStart = (DateTime)Startdate,
                    Gender = Gender.Tag.ToString(),
                    Position = Position.Tag.ToString(),
                    Username = Username,
                    Password = Password,
                    Avatar = photo_aray
                };
                (bool issucessed, string messfromservice, StaffDTO newstaff) = await StaffService.Ins.AddStaff(staffDTO);
                if (issucessed)
                {
                    LoadStaffList(Operation.CREATE, staffDTO);
                    p.Close();
                    CustomMessageBox.ShowOk(messfromservice, "Thông báo", "OK", CustomMessageBoxImage.Success);
                }
                else
                {
                    CustomMessageBox.ShowOk(messfromservice, "Lỗi", "OK", CustomMessageBoxImage.Error);
                }
            }
            else
            {
                CustomMessageBox.ShowOk(mess, "Cảnh báo", "OK", CustomMessageBoxImage.Warning);
            }     
        }
    }
}
