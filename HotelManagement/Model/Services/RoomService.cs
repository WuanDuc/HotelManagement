using Google.Apis.Util;
using HotelManagement.DTOs;
using HotelManagement.Utils;
using IronXL.Formatting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotelManagement.Model.Services
{
    public class RoomService
    {
        public RoomService() { }
        private static RoomService _ins;
        public static RoomService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new RoomService();
                }
                return _ins;
            }
            private set { _ins = value; }
        }
        public async Task<List<RoomDTO>> GetAllRoom()
        {
            try
            {
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    List<RoomDTO> RoomDTOs = await (
                        from r in db.Rooms
                        join temp in db.RoomTypes
                        on r.RoomTypeId equals temp.RoomTypeId into gj
                        from d in gj.DefaultIfEmpty()
                        select new RoomDTO
                        {
                            // DTO = db
                            RoomId = r.RoomId,
                            RoomNumber = (int)r.RoomNumber,
                            RoomTypeName = d.RoomTypeName,
                            RoomTypeId = d.RoomTypeId,
                            Note = r.Note,
                            RoomCleaningStatus = r.RoomCleaningStatus,
                            RoomStatus = r.RoomStatus,
                        }
                    ).ToListAsync();
                    RoomDTOs.Reverse();
                    return RoomDTOs;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private string CreateNextRoomCode(string maxCode)
        {
            if (maxCode == "")
            {
                return "PH001";
            }
            int index = (int.Parse(maxCode.Substring(2)) + 1);
            string CodeID = index.ToString();
            while (CodeID.Length < 3) CodeID = "0" + CodeID;

            return "PH" + CodeID;
        }
        public async Task<(bool, string, RoomDTO)> AddRoom(RoomDTO newRoom)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    Room r = context.Rooms.Where((Room Room) => Room.RoomNumber == newRoom.RoomNumber).FirstOrDefault();

                    if (r != null)
                    {
                                 
                            return (false, $"Phòng {r.RoomNumber} đã tồn tại!", null);

                    }
                    else
                    {
                        var listid = await context.Rooms.Select(s => s.RoomId).ToListAsync();
                        string maxId = "";

                        if (listid.Count > 0)
                            maxId = listid[listid.Count - 1];
                        string id = CreateNextRoomCode(maxId);
                        Room room = new Room
                        {
                            RoomId = id,
                            RoomNumber = newRoom.RoomNumber,
                            RoomTypeId = newRoom.RoomTypeId,
                            Note = newRoom.Note,
                            RoomStatus = newRoom.RoomStatus,
                            RoomCleaningStatus = newRoom.RoomCleaningStatus,
                        };
                        context.Rooms.Add(room);
                        await context.SaveChangesAsync();
                        newRoom.RoomId = room.RoomId;
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return (false, "DbEntityValidationException", null);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, $"Error Server {e}", null);
            }
            return (true, "Thêm phòng thành công", newRoom);
        }

        public async Task<(bool, string)> DeleteRoom(string Id)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    Room room = await (from p in context.Rooms
                                       where p.RoomId == Id 
                                       select p).FirstOrDefaultAsync();
                    if (room == null)
                    {
                        return (false, "Loại phòng này không tồn tại!");
                    }
                    context.Rooms.Remove(room);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                return (false, "Phòng này đã áp dụng trước đây và đã có khách đặt. Không thể xóa!");
            }
            return (true, "Xóa phòng thành công");
        }

        public async Task<(bool, string)> UpdateRoom(RoomDTO updatedRoom)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    Room room = context.Rooms.Find(updatedRoom.RoomId);

                    if (room is null)
                    {
                        return (false, "Phòng này không tồn tại!");
                    }

                    // ở dưới phải đợi thêm r fix, code coment ở dưới tức là khi
                    // phòng đã có người đặt hoặc đang thuê thì không thể chỉnh sửa

                    //bool IsExistRoomNumber = context.Rooms.Any((Room r) => r.RoomId != room.RoomTypeId && r.RoomNumber == updatedRoom.RoomNumber);
                    //if (IsExistRoomNumber)
                    //{
                    //    return (false, "Phòng đang được sử dụng không thể chỉnh sửa!");
                    //}

                    room.RoomId = updatedRoom.RoomId;
                    room.RoomNumber = updatedRoom.RoomNumber;
                    room.RoomStatus = updatedRoom.RoomStatus;
                    room.Note = updatedRoom.Note;
                    room.RoomCleaningStatus = updatedRoom.RoomCleaningStatus;
                    room.RoomTypeId = updatedRoom.RoomTypeId;

                    await context.SaveChangesAsync();
                    return (true, "Cập nhật thành công");
                }
            }
            catch (DbEntityValidationException)
            {
                return (false, "DbEntityValidationException");
            }
            catch (DbUpdateException e)
            {
                return (false, $"DbUpdateException: {e.Message}");
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống");
            }

        }

        public async Task<List<RoomDTO>> GetRooms()
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    var roomList = await (from r in context.Rooms
                                          join t in context.RoomTypes
                                          on r.RoomTypeId equals t.RoomTypeId
                                          select new RoomDTO
                                          {
                                              RoomId = r.RoomId,
                                              RoomNumber = r.RoomNumber,
                                              RoomTypeId = r.RoomTypeId,
                                              Note = r.Note,
                                              RoomStatus = r.RoomStatus,
                                              RoomCleaningStatus = r.RoomCleaningStatus,
                                              Price = (double)t.Price,
                                          }
                                          ).ToListAsync();
                    return roomList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task AutoUpdateDb()
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    var roomRentingList = await context.Rooms.Where(x => x.RoomStatus == ROOM_STATUS.RENTING).Select(x => x.RoomId).ToListAsync();

                    var list1 = await context.RentalContracts.ToListAsync();
                    var rentalContractListId = list1.Where(x => x.CheckOutDate + x.StartTime <= DateTime.Today + DateTime.Now.TimeOfDay
                    && roomRentingList.Contains(x.RoomId) == false).Select(x => x.RentalContractId).ToList();
                    string t = "";
                    for (int i = 0; i < rentalContractListId.Count; i++)
                    {

                        t += $@"'{rentalContractListId[i]}'";
                        if (i != rentalContractListId.Count - 1)
                        {
                            t += ",";
                        }
                    }
                    if (t != "")
                    {
                        var sql1 = $@"Update [RentalContract] SET Validated = 0  WHERE RentalContractId IN ({t})";
                        await context.Database.ExecuteSqlCommandAsync(sql1);
                    }
                    


                    list1 = await context.RentalContracts.ToListAsync();
                    var roomListId = list1.Where(x => x.CheckOutDate + x.StartTime <= DateTime.Today + DateTime.Now.TimeOfDay 
                    && roomRentingList.Contains(x.RoomId) == false).Select(x => x.RoomId).ToList();
                    t = "";
                    for (int i = 0; i < roomListId.Count; i++)
                    {

                        t += $@"'{roomListId[i]}'";
                        if (i != roomListId.Count - 1)
                        {
                            t += ",";
                        }
                    }
                    string sql2 = "";
                    if (t != "")
                    {
                        sql2 = $@"Update [Room] SET RoomStatus = N'{ROOM_STATUS.READY}'  WHERE RoomId  IN ({t})";
                        await context.Database.ExecuteSqlCommandAsync(sql2);
                    }
                   

                    list1 = await context.RentalContracts.ToListAsync();
                    roomListId = list1.Where(x => x.CheckOutDate + x.StartTime > DateTime.Today + DateTime.Now.TimeOfDay && x.StartDate + x.StartTime <= DateTime.Today + DateTime.Now.TimeOfDay && roomRentingList.Contains(x.RoomId) == false && x.Validated == true).Select(x => x.RoomId).ToList();
                    t = "";
                    for (int i = 0; i < roomListId.Count; i++)
                    {

                        t += $@"'{roomListId[i]}'";
                        if (i != roomListId.Count - 1)
                        {
                            t += ",";
                        }
                    }
                    if (t != "")
                    {
                        sql2 = $@"Update [Room] SET RoomStatus = N'{ROOM_STATUS.BOOKED}'  WHERE RoomId  IN ({t})";
                        await context.Database.ExecuteSqlCommandAsync(sql2);
                    }
                   

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<List<RoomSettingDTO>> GetRoomsByRoomType(string RoomTypeId)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    RoomType rt = await context.RoomTypes.FindAsync(RoomTypeId);
                    int roomNumber = context.Rooms.Select(x => x.RoomTypeId == RoomTypeId).Count();


                    var roomList = await (from r in context.Rooms
                                          join c in context.RentalContracts
                                          on r.RoomId equals c.RoomId into ps
                                          from c in ps.DefaultIfEmpty()
                                          join cu in context.Customers
                                          on c.CustomerId equals cu.CustomerId into pi
                                          from cu in pi.DefaultIfEmpty()
                                          where r.RoomTypeId == RoomTypeId 
                                          orderby r.RoomId
                                          select new RoomSettingDTO
                                          {
                                              RoomId = r.RoomId,
                                              RoomNumber = r.RoomNumber,
                                              RoomTypeId = r.RoomTypeId,
                                              RoomTypeName = rt.RoomTypeName.Trim(),
                                              RoomStatus = r.RoomStatus.Trim(),
                                              RoomCleaningStatus = r.RoomCleaningStatus.Trim(),
                                              StartDate = c.StartDate,
                                              StartTime = c.StartTime,
                                              CheckOutDate = c.CheckOutDate,
                                              Validated = c.Validated,
                                              CustomerId = cu.CustomerId,
                                              CustomerName = cu.CustomerName,
                                              RentalContractId = c.RentalContractId,

                                          }

                                          ).ToListAsync();
                    List<RoomSettingDTO> roomList2 = new List<RoomSettingDTO>();
                    var t = DateTime.Today + DateTime.Now.TimeOfDay;
                    Dictionary<string, List<RoomSettingDTO>> dic = new Dictionary<string, List<RoomSettingDTO>>();
                    foreach (var item in roomList)
                    {
                        if (!dic.Keys.Contains(item.RoomId))
                        {
                            dic.Add(item.RoomId, new List<RoomSettingDTO>());
                        }
                    }
                    foreach (var item in dic.Keys)
                    {
                        foreach (var room in roomList)
                        {
                            if (room.RoomId == item)
                            {
                                dic[item].Add(room);
                            }
                        }
                    }
                    foreach (var item in dic.Values)
                    {
                        if (item.Count > 1)
                        {
                            item.Sort();
                        }
                    }
                    foreach (var item in dic.Values)
                    {
                        if (item.Count > 1)
                        {
                            bool flat = false;
                            foreach (var item2 in item)
                            {
                                RentalContract r = await context.RentalContracts.FindAsync(item2.RentalContractId);
                                if (item2.StartDate + item2.StartTime <= t && t < item2.CheckOutDate + item2.StartTime && r.Validated==true)
                                {
                                    roomList2.Add(item2);
                                    flat = true;
                                    break;
                                }
                                else if (r.Validated == true)
                                {
                                    roomList2.Add(item2);
                                    flat = true;
                                    break;
                                }

                            }
                            if (flat == false) roomList2.Add(item[0]);


                        }
                        else roomList2.Add(item[0]);
                    }
                    foreach (var item in roomList2)
                    {
                        if (item.RoomStatus == ROOM_STATUS.READY)
                        {

                            item.StartDate = null;
                            item.StartTime = null;
                            item.CheckOutDate = null;
                            item.CustomerId = null;
                            item.CustomerName = null;
                            item.RentalContractId= null;
                        }
                    }
                    return roomList2;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<List<RoomTypeDTO>> GetRoomTypes()
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    var roomTypeList = await (from r in context.RoomTypes
                                              select new RoomTypeDTO
                                              {
                                                  RoomTypeId = r.RoomTypeId,
                                                  RoomTypeName = r.RoomTypeName.Trim(),
                                                  RoomTypePrice = (double)r.Price,
                                              }
                                          ).ToListAsync();
                    return roomTypeList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<(bool, string)> ChangeRoomStatus(string roomId, string rentalContractId)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    Room room = await context.Rooms.FindAsync(roomId);
                    RentalContract rentalContract = await context.RentalContracts.FindAsync(rentalContractId);
                    string mess = "";
                    if (room.RoomStatus == ROOM_STATUS.BOOKED) 
                    {
                        room.RoomStatus = ROOM_STATUS.RENTING;
                        mess = "Nhận phòng thành công!";
                    }
                    else if (room.RoomStatus == ROOM_STATUS.RENTING)
                    {
                        room.RoomStatus = ROOM_STATUS.READY;
                        rentalContract.Validated = false;
                        mess = "Thanh toán thành công!";

                    }
                    await context.SaveChangesAsync();
                    return (true, mess);
                    
                }
            }
            catch(Exception ex)
            {
                return (false, "Lỗi hệ thống!");
            }
        }
        public async Task<(bool, string)> UpdateRoomInfo(string roomId, string roomCleaningStatus)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    Room room = await context.Rooms.FindAsync(roomId);
                    room.RoomCleaningStatus= roomCleaningStatus;
                    await context.SaveChangesAsync();
                    return (true, "Cập nhật thành công!");

                }
            }
            catch (Exception ex)
            {
                return (false, "Lỗi hệ thống!");
            }
        }

    }
}

        
