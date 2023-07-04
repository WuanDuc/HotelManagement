
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
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotelManagement.ViewModel.StaffVM.TroubleReportVM
{
    public partial class TroubleReportVM : BaseVM
    {
        public async Task UpdateRPTrouble(Window p)
        {
            (bool isvalid, string mess) = IsValidData();
            if (isvalid)
            {

                TroubleDTO trouble = new TroubleDTO
                {
                    TroubleId = TroubleId,
                    Title = Title,
                    Description = Desription,
                    StartDate = StartDate,
                    StaffId = "NV001",
                    Level = Level.Tag.ToString(),
                    Reason = Reason.Tag.ToString(),

                };
                if (filepath != null)
                {
                    FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                    byte[] photo_aray = new byte[fs.Length];
                    fs.Read(photo_aray, 0, photo_aray.Length);
                    trouble.Avatar = photo_aray;
                }
                else
                {
                    trouble.Avatar = SelectedItem.Avatar;
                }
                (bool issucessed, string messfromservice) = await TroubleService.Ins.EditTrouble(trouble, RentalContractId);
                if (issucessed)
                {
                    LoadTroubleList(Operation.UPDATE, trouble);
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
