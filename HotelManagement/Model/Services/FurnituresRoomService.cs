using HotelManagement.DTOs;
using HotelManagement.Utils;
using HotelManagement.View.Staff.RoomCatalogManagement.RoomInfo;
using IronXL.Formatting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace HotelManagement.Model.Services
{
    public class FurnituresRoomService
    {
        private FurnituresRoomService() { }
        private static FurnituresRoomService _ins;
        public static FurnituresRoomService Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new FurnituresRoomService();
                return _ins;
            }
            private set { _ins = value; }
        }

        public async Task<(bool, string, List<FurnituresRoomDTO>)> GetAllFurnituresRoom()
        {
            try
            {
                List<FurnituresRoomDTO> furnituresRoomDTOs = new List<FurnituresRoomDTO>();

                using(HotelManagementEntities db = new HotelManagementEntities())
                {
                    furnituresRoomDTOs = await (
                        from room in db.Rooms

                        select new FurnituresRoomDTO
                        {
                            RoomId = room.RoomId,
                            RoomNumber = (int)room.RoomNumber,
                            CustomerName = ROOM_STATUS.READY,
                            RoomStatus = room.RoomStatus,
                            Note = room.Note,
                            RoomType = room.RoomType.RoomTypeName ?? string.Empty,
                            CustomerQuantity = 0
                        }
                    ).ToListAsync();

                    furnituresRoomDTOs.ForEach(item =>
                    {
                        if(item.RoomStatus == ROOM_STATUS.RENTING)
                        {
                            RentalContract renContract = db.RentalContracts.FirstOrDefault(i => i.RoomId == item.RoomId && i.Validated == true);
                            if(renContract != null)
                            {
                                item.CustomerName = renContract.Customer.CustomerName;
                                item.CustomerQuantity = renContract.RoomCustomers.Count();
                            }
                        }
                        item.SetOtherProperty();
                    });


                    return (true, "Thành công", furnituresRoomDTOs);
                }
            }
            catch(EntityException e)
            {
                return (false, "Mất kết nối cơ sở dữ liệu" ,null);
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống", null);
            }

        }
        public async Task<(bool, string, List<FurnitureDTO>)> GetAllFurnituresIn(FurnituresRoomDTO roomSelected)
        {
            try
            {
                List<FurnitureDTO> furnituresRoomDTOs = new List<FurnitureDTO>();

                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    furnituresRoomDTOs = await (
                        from p in db.RoomFurnituresDetails
                        where p.RoomId == roomSelected.RoomId
                        select new FurnitureDTO
                        {
                            FurnitureID = p.FurnitureId,
                            FurnitureName = p.Furniture.FurnitureName,
                            FurnitureAvatarData = p.Furniture.FurnitureAvatar,
                            FurnitureType = p.Furniture.FurnitureType,
                            InUseQuantity = (int)p.Quantity,
                            DeleteInRoomQuantity = (int)p.Quantity,
                        }
                    ).ToListAsync();

                    furnituresRoomDTOs.ForEach(item => item.SetAvatar());

                    return (true, "Thành công", furnituresRoomDTOs);
                }
            }
            catch (EntityException e)
            {
                return (false, "Mất kết nối cơ sở dữ liệu", null);
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống", null);
            }
        }
        public async Task<(bool, string, List<FurnitureDTO>)> GetAllFurniture()
        {
            try
            {
                List<FurnitureDTO> listFurniture = new List<FurnitureDTO>();
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    listFurniture = await (
                        from p in db.Furnitures
                        let Sum =
                        (
                            from rfd in db.RoomFurnituresDetails
                            where p.FurnitureId == rfd.FurnitureId
                            select rfd.Quantity
                        ).Sum()
                        select new FurnitureDTO
                        {
                            FurnitureID = p.FurnitureId,
                            FurnitureAvatarData = p.FurnitureAvatar,
                            FurnitureName = p.FurnitureName,
                            FurnitureType = p.FurnitureType,
                            Quantity = (int)p.FurnitureStorage.QuantityFurniture,
                            TotalUseQuantity = (int)(Sum == null ? 0 : Sum),
                        }
                    ).ToListAsync();
                }
                listFurniture.ForEach(item => { item.SetRemaining(); item.SetAvatar(); });

                return (true, "", listFurniture);
            }
            catch (EntityException e)
            {
                return (false, "Mất kết nối cơ sở dữ liệu", null);
            }
            catch (Exception ex)
            {
                return (false, "Lỗi hệ thống", null);
            }
        }
        
        public async Task<(bool, string, List<FurnitureDTO>)> ImportListFurnitureToRoom(ObservableCollection<FurnitureDTO> orderList, FurnituresRoomDTO roomFurnitureSelected)
        {
            try
            {
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    int Length = orderList.Count; 
                    List<FurnitureDTO> listFurniture = new List<FurnitureDTO>();
                    
                    for (int i = 0; i < Length; i++)
                    {
                        FurnitureDTO temp = new FurnitureDTO(orderList[i]);
                        RoomFurnituresDetail furnitureInRoom = await db.RoomFurnituresDetails.FirstOrDefaultAsync(item => item.RoomId == roomFurnitureSelected.RoomId && temp.FurnitureID == item.FurnitureId);
                        if (furnitureInRoom != null)
                            furnitureInRoom.Quantity += temp.QuantityImportRoom;
                        else
                        {
                            furnitureInRoom = new RoomFurnituresDetail
                            {
                                RoomId = roomFurnitureSelected.RoomId,
                                FurnitureId = temp.FurnitureID,
                                Quantity = temp.QuantityImportRoom,
                            };
                            db.RoomFurnituresDetails.Add(furnitureInRoom);
                        }
                        listFurniture.Add(temp);
                        orderList[i].QuantityImportRoom = 0;
                    }
                    await db.SaveChangesAsync();
                    return (true, "Nhập tiện nghi thành công", listFurniture);
                }
            }
            catch (EntityException e)
            {
                return (false, "Mất kết nối cơ sở dữ liệu", null);
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống", null);
            }
        }

        public async Task<(bool, string)> DeleteFurnitureRoom(string roomFurnitureSelectedID, FurnitureDTO selectedFurniture)
        {
            try
            {
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    RoomFurnituresDetail roomFurnituresDetail = await db.RoomFurnituresDetails.FirstOrDefaultAsync(item => item.RoomId == roomFurnitureSelectedID && item.FurnitureId == selectedFurniture.FurnitureID);
                    if (roomFurnituresDetail == null)
                        return (false, "Không tìm thấy thông tin trong cơ sở dữ liệu");

                    db.RoomFurnituresDetails.Remove(roomFurnituresDetail);
                    await db.SaveChangesAsync();
                    return (true, "Xóa tiện nghi thành công");
                }
            }
            catch (EntityException e)
            {
                return (false, "Mất kết nối cơ sở dữ liệu");
            }
            catch (Exception ex)
            {
                return (false, "Lỗi hệ thống");
            }
        }
        public async Task<(bool, string)> DeleteListFurnitureRoom(string roomFurnitureSelectedID, ObservableCollection<FurnitureDTO> DeleteList)
        {
            try
            {
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    int lengh = DeleteList.Count();
                    for(int i = 0; i <lengh; i++ )
                    {
                        FurnitureDTO temp = DeleteList[i];
                        RoomFurnituresDetail roomFurnituresDetail = await db.RoomFurnituresDetails.FirstOrDefaultAsync(item => item.RoomId == roomFurnitureSelectedID && item.FurnitureId == temp.FurnitureID);
                        if (roomFurnituresDetail == null)
                            return (false, "Không tìm thấy thông tin trong cơ sở dữ liệu");

                        if (temp.DeleteInRoomQuantity == temp.InUseQuantity)
                            db.RoomFurnituresDetails.Remove(roomFurnituresDetail);
                        else
                            roomFurnituresDetail.Quantity -= temp.DeleteInRoomQuantity;
                    }    

                    await db.SaveChangesAsync();
                    return (true, "Xóa tiện nghi thành công");
                }
            }
            catch (EntityException e)
            {
                return (false, "Mất kết nối cơ sở dữ liệu");
            }
            catch (Exception ex)
            {
                return (false, "Lỗi hệ thống");
            }
        }

        public async Task<List<RoomFurnituresDetailDTO>> GetRoomFurnituresDetail(string roomId)
        {
            try
            {
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    var listRoomFurniture = await (from a in db.RoomFurnituresDetails
                                                   join b in db.Furnitures
                                                   on a.FurnitureId equals b.FurnitureId
                                                   where a.RoomId == roomId
                                                   select new RoomFurnituresDetailDTO
                                                   {
                                                       RoomId = roomId,
                                                       FurnitureId= a.FurnitureId,
                                                       FurnitureName= b.FurnitureName,
                                                       FurnitureType= b.FurnitureType,
                                                       Quantity = a.Quantity,
                                                       FurnitureAvatarData = b.FurnitureAvatar
                                                   }
                                                   ).ToListAsync();
                    foreach (var item in listRoomFurniture)
                    {
                        item.SetAvatar();
                    }
                    return listRoomFurniture;
                }

               
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        
    }
}
