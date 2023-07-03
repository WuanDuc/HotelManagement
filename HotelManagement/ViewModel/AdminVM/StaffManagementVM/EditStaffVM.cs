using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using HotelManagement.Utilities;
using HotelManagement.Utils;
using HotelManagement.View.CustomMessageBoxWindow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotelManagement.ViewModel.AdminVM.StaffManagementVM
{
    public partial class StaffManagementVM : BaseVM
    {
        private async Task EditStaff(Window p)
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
            (bool isvalid, string mess) = IsValidData(Operation.UPDATE);
            if (isvalid)
            {                 
                StaffDTO staffDTO = new StaffDTO
                {
                    StaffId = StaffId,
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
                };
                if (filepath != null)
                {
                    FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                    byte[] photo_aray = new byte[fs.Length];
                    fs.Read(photo_aray, 0, photo_aray.Length);
                    staffDTO.Avatar = photo_aray;
                }
                else
                {
                    staffDTO.Avatar = SelectedItem.Avatar;
                }
                (bool issucessed, string messfromservice) = await StaffService.Ins.UpdateStaffInfo(staffDTO);
                if (issucessed)
                {
                    LoadStaffList(Operation.UPDATE, staffDTO);
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
