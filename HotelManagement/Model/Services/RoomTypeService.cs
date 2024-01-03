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
        public RoomTypeService(HotelManagementEntities context) { 
            _context = context;
        }
        private static RoomTypeService _ins;
        public static RoomTypeService Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new RoomTypeService(new HotelManagementEntities());
                return _ins;
            }
            //private set { _ins = value; }
        }

        private HotelManagementEntities _context;
        // Take all RoomType 
        public async Task<List<RoomTypeDTO>> GetAllRoomType()
        {
            try
            {
                List<RoomTypeDTO> RoomTypeDTOs = (
                    from rt in _context.RoomTypes
                    select new RoomTypeDTO
                    {
                        // DTO = db
                        RoomTypeId = rt.RoomTypeId,
                        RoomTypeName = rt.RoomTypeName,
                        RoomTypePrice = (double)rt.Price,
                        RoomTypeNote = rt.Note,
                    }
                ).ToList();
                RoomTypeDTOs.Reverse();
                return RoomTypeDTOs;

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
                if (_context == null)
                {
                    _context = new HotelManagementEntities();
                }

                var item = _context.RoomTypes.Where(x => x.RoomTypeName == rtn).FirstOrDefault();
                return item.RoomTypeId;

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
                if (_context == null)
                {
                    _context = new HotelManagementEntities();
                }
                RoomType roomType = _context.RoomTypes.Where(r => r.RoomTypeId == updatedRoomType.RoomTypeId).First();

                if (roomType is null)
                {
                    return (false, "Loại phòng này không tồn tại!");
                }

                bool IsExistRoomTypeName = _context.RoomTypes.Any((RoomType rt) => rt.RoomTypeId != roomType.RoomTypeId && rt.RoomTypeName == updatedRoomType.RoomTypeName);
                if (IsExistRoomTypeName)
                {
                    return (false, "Tên loại phòng đã tồn tại!");
                }

                roomType.RoomTypeName = updatedRoomType.RoomTypeName;
                roomType.Price = updatedRoomType.RoomTypePrice;
                roomType.RoomTypeId = updatedRoomType.RoomTypeId;
                roomType.Note = updatedRoomType.RoomTypeNote;

                await _context.SaveChangesAsync();
                return (true, "Cập nhật thành công");

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
