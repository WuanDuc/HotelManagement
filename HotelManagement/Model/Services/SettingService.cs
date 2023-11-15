using HotelManagement.DTOs;
using HotelManagement.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Model.Services
{
    public class SettingService
    {
        private static SettingService _ins;
        public static SettingService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new SettingService(null);
                }
                return _ins;
            }
            private set => _ins = value;
        }

        private HotelManagementEntities _context;

        public SettingService(HotelManagementEntities context)
        {
            _context = context;
        }
        public async Task<(bool, string)> EditName(string StaffName, string Id)
        {
            try
            {
                if (_context == null)
                {
                    _context = new HotelManagementEntities();
                }
                if (_context.Staffs == null)
                {
                    return (false, "Không có nhân viên");
                }
                Staff staff = _context.Staffs.Find(Id);
                if (staff == null)
                    return (false, "Lỗi không tìm thấy nhân viên");
                staff.StaffName = StaffName;
                _context.SaveChanges();
                return (true, "Lưu thông tin thành công");
            }
            catch (EntityException)
            {
                return (false, "Mất kết nối cơ sở dữ liệu");
            }
            catch (Exception e)
            {
                return (false, "lỗi hệ thống");
            }
        }
        public async Task<(bool, string)> EditEmail(string StaffEmail, string Id)
        {
            try
            {
                if (_context == null)
                {
                    _context = new HotelManagementEntities();
                }

                Staff staff = _context.Staffs.Find(Id);
                if (staff == null)
                    return (false, "lỗi hệ thống");
                staff.Email = StaffEmail;
                _context.SaveChanges();
                return (true, "Lưu thông tin thành công");

            }
            catch (EntityException)
            {
                return (false, "Mất kết nối cơ sở dữ liệu");
            }
            catch (Exception e)
            {
                return (false, "lỗi hệ thống");
            }
        }
    }
}
