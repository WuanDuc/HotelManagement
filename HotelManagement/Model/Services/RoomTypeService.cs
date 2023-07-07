using HotelManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Model.Services
{
    public class RoomTypeService
    {
        public RoomTypeService() { }
        private static RoomTypeService _ins;
        public static RoomTypeService Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new RoomTypeService();
                return _ins;
            }
            private set { _ins = value; }
        }
        // Take all RoomType 
        public async Task<List<RoomTypeDTO>> GetAllRoomType()
        {
            try
            {
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    List<RoomTypeDTO> RoomTypeDTOs = await (
                        from rt in db.RoomTypes
                        select new RoomTypeDTO
                        {
                            // DTO = db
                            RoomTypeId = rt.RoomTypeId,
                            RoomTypeName = rt.RoomTypeName,
                            RoomTypePrice = (double)rt.Price,
                            RoomTypeNote = rt.Note,
                        }
                    ).ToListAsync();
                    RoomTypeDTOs.Reverse();
                    return RoomTypeDTOs;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<string> GetRoomTypeID(string rtn)
        {
            try
            {
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    var item = await db.RoomTypes.Where(x => x.RoomTypeName == rtn).FirstOrDefaultAsync();
                    return item.RoomTypeId;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<(bool, string)> UpdateRoomType(RoomTypeDTO updatedRoomType)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    RoomType roomType = context.RoomTypes.Find(updatedRoomType.RoomTypeId);

                    if (roomType is null)
                    {
                        return (false, "Loại phòng này không tồn tại!");
                    }

                    bool IsExistRoomTypeName = context.RoomTypes.Any((RoomType rt) => rt.RoomTypeId != roomType.RoomTypeId && rt.RoomTypeName == updatedRoomType.RoomTypeName);
                    if (IsExistRoomTypeName)
                    {
                        return (false, "Tên loại phòng đã tồn tại!");
                    }

                    roomType.RoomTypeName = updatedRoomType.RoomTypeName;
                    roomType.Price = updatedRoomType.RoomTypePrice;
                    roomType.RoomTypeId = updatedRoomType.RoomTypeId;
                    roomType.Note = updatedRoomType.RoomTypeNote;

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
    }
}
