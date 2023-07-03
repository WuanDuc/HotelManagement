using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using HotelManagement.View.Admin.RoomTypeManagement;
using Microsoft.Office.Interop.Excel;
using HotelManagement.Model;
using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using HotelManagement.Utils;

namespace HotelManagement.ViewModel.AdminVM.RoomTypeManagementVM
{
    public partial class RoomTypeManagementVM:BaseVM
    {
        public ICommand LoadEditRoomTypeCM { get; set; }

        public void LoadEditRoomType()
        {
            RoomTypeID = SelectedItem.RoomTypeId;
            RoomTypeName = SelectedItem.RoomTypeName;
            RoomTypePrice = SelectedItem.RoomTypePrice;
            RoomTypeNote = SelectedItem.RoomTypeNote;
        }
        public async Task UpdateRoomTypeFunc(System.Windows.Window p)
        {

            if (RoomTypeID != null && IsValidData())
            {
                RoomTypeDTO roomType = new RoomTypeDTO
                {
                    RoomTypeId = RoomTypeID,
                    RoomTypeName = RoomTypeName,
                    RoomTypePrice = RoomTypePrice,
                    RoomTypeNote = RoomTypeNote,
                };

                (bool successUpdateRoomType, string messageFromUpdateRoomType) = await RoomTypeService.Ins.UpdateRoomType(roomType);

                if (successUpdateRoomType)
                {
                    isSaving = false;
                    CustomMessageBox.ShowOk(messageFromUpdateRoomType, "Thông báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                    LoadRoomTypeListView(Operation.UPDATE, roomType);
                    p.Close();
                }
                else
                {
                    CustomMessageBox.ShowOk(messageFromUpdateRoomType, "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                }
            }
            else
            {
                CustomMessageBox.ShowOk("Vui lòng nhập đủ thông tin!", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
            }
        }
    }
}
