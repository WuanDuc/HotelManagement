using HotelManagement.DTOs;
using HotelManagement.Model;
using HotelManagement.Model.Services;
using HotelManagement.Utilities;
using HotelManagement.Utils;
using HotelManagement.View.CustomMessageBoxWindow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace HotelManagement.ViewModel.StaffVM.TroubleReportVM
{
    public partial class TroubleReportVM : BaseVM
    {
        private async Task AddTrouble(Window p)
        {
            (bool isvalid, string mess) = IsValidData();
            if (isvalid)
            {
                FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                byte[] photo_aray = new byte[fs.Length];
                fs.Read(photo_aray, 0, photo_aray.Length);

                TroubleDTO trouble = new TroubleDTO
                {
                    Title = Title,
                    Description = Desription,
                    StartDate = StartDate,
                    StaffId = currentStaff.StaffId,
                    Level = Level.Tag.ToString(),
                    Reason = Reason.Tag.ToString(),
                    Avatar = photo_aray
                };
                (bool issucessed, string messfromservice, TroubleDTO newtrouble) = await TroubleService.Ins.AddTrouble(trouble, RentalContractId);
                if (issucessed)
                {
                    LoadTroubleList(Operation.CREATE, newtrouble);
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
        private (bool isvalid, string mess) IsValidData()
        {

            if (String.IsNullOrEmpty(Title) || String.IsNullOrEmpty(Desription) || Reason is null || Level is null || ImageTrouble is null)
            {
                return (false, "Vui lòng nhập đủ thông tin sự cố!");
            }
            if (Reason.Tag.ToString() == REASON.BYCUSTOMER && RentalContractId is null)
            {
                return (false, "Vui lòng nhập đủ thông tin sự cố!");
            }
            return (true, null);
        }

    }
}
