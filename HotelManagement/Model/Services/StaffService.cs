using HotelManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Model.Services
{
    public class StaffService
    {
        private static StaffService _ins;
        public static StaffService Ins
        {
            get { return _ins ?? (_ins = new StaffService()); }
            private set { _ins = value; }
        }
        private StaffService(){}

        public async Task<List<StaffDTO>> GetAllStaff()
        {
            List<StaffDTO> staffList;
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    staffList = (from s in context.Staffs
                                    where s.IsDeleted == false
                                    select new StaffDTO
                                    {
                                        StaffId = s.StaffId,
                                        StaffName = s.StaffName,
                                        PhoneNumber = s.PhoneNumber,
                                        Email=s.Email,
                                        CCCD=s.CCCD,
                                        DateOfBirth=s.DateOfBirth,
                                        dateOfStart=s.dateOfStart,
                                        StaffAddress=s.StaffAddress,
                                        Gender = s.Gender,
                                        Position=s.Position,
                                        Username=s.Username,
                                        Password=s.Password,
                                        Avatar=s.Avatar,                                       
                                       
                                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return staffList;
        }
        public async Task<(bool, string, StaffDTO)> AddStaff(StaffDTO staff)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    bool isCccdExist = await context.Staffs.AnyAsync(s => staff.CCCD == s.CCCD);
                    if (isCccdExist) return (false, "CCCD đã tồn tại!", null);
                    bool IsphoneExist = await context.Staffs.AnyAsync(s => staff.PhoneNumber == s.PhoneNumber);
                    if (IsphoneExist) return (false, "Số điện thoại đã tồn tại!", null);
                    bool IsUserNameExist = await context.Staffs.AnyAsync(s => staff.Username == s.Username);
                    if (IsUserNameExist) return (false, "Tên đăng nhập đã tồn tại!", null);
                    if(staff.Email != null)
                    {
                        bool IsemailExist = await context.Staffs.AnyAsync(s => staff.Email == s.Email);
                        if (IsemailExist) return (false, "Email đã được đăng ký!", null);
                    }
                    var maxId = await context.Staffs.MaxAsync(s => s.StaffId);

                    Staff newstaff = new Staff();
                    newstaff.StaffId = CreateNextStaffId(maxId);
                    newstaff.StaffName = staff.StaffName;
                    newstaff.Email = staff.Email;
                    newstaff.StaffAddress = staff.StaffAddress;
                    newstaff.PhoneNumber = staff.PhoneNumber;
                    newstaff.CCCD=staff.CCCD;
                    newstaff.DateOfBirth = staff.DateOfBirth;
                    newstaff.dateOfStart = staff.dateOfStart;
                    newstaff.Gender = staff.Gender;
                    newstaff.Position = staff.Position;
                    newstaff.Username = staff.Username;
                    newstaff.Password=staff.Password;
                    newstaff.Avatar = staff.Avatar;
                    newstaff.IsDeleted = false;

                    staff.StaffId = newstaff.StaffId;
                    context.Staffs.Add(newstaff);
                    await context.SaveChangesAsync();
                }
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return (false, "Mất kết nối cơ sở dữ liệu", null);
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống", null);
            }
            return (true, "Thêm nhân viên mới thành công", staff);
        }
        public async Task<(bool, string)> UpdateStaffInfo(StaffDTO staff)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    bool isCccdExist = await context.Staffs.AnyAsync(s => staff.StaffId!=s.StaffId && staff.CCCD == s.CCCD);
                    if (isCccdExist) return (false, "CCCD đã tồn tại!");
                    bool IsphoneExist = await context.Staffs.AnyAsync(s => staff.StaffId != s.StaffId && staff.PhoneNumber == s.PhoneNumber);
                    if (IsphoneExist) return (false, "Số điện thoại đã tồn tại!");
                    bool IsUserNameExist = await context.Staffs.AnyAsync(s => staff.StaffId != s.StaffId && staff.Username == s.Username);
                    if (IsUserNameExist) return (false, "Tên đăng nhập đã tồn tại!");
                    if (staff.Email != null)
                    {
                        bool IsemailExist = await context.Staffs.AnyAsync(s => staff.StaffId != s.StaffId && staff.Email == s.Email);
                        if (IsemailExist) return (false, "Email đã được đăng ký!");
                    }
                    var selectStaff = await context.Staffs.FindAsync(staff.StaffId);
                    selectStaff.StaffName= staff.StaffName;
                    selectStaff.PhoneNumber= staff.PhoneNumber;
                    selectStaff.StaffAddress = staff.StaffAddress;
                    selectStaff.Email= staff.Email;
                    selectStaff.CCCD= staff.CCCD;
                    selectStaff.Username= staff.Username;
                    selectStaff.Position = staff.Position;
                    selectStaff.Gender = staff.Gender;
                    selectStaff.dateOfStart = staff.dateOfStart;
                    selectStaff.DateOfBirth = staff.DateOfBirth;
                    selectStaff.Avatar = staff.Avatar;
                                      
                    await context.SaveChangesAsync();
                }
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return (false, "Mất kết nối cơ sở dữ liệu");
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống");
            }
            return (true, "Cập nhật thông tin thành công");
        }
        public async Task<(bool, string)> DeleteStaff(string id)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    Staff selectedStaff = await (from s in context.Staffs
                                                  where s.StaffId == id && s.IsDeleted == false
                                                  select s).FirstOrDefaultAsync();
                    if (selectedStaff is null || selectedStaff.IsDeleted == true)
                    {
                        return (false, "Nhân viên không tồn tại");

                    }
                    selectedStaff.IsDeleted = true;

                    await context.SaveChangesAsync();
                }
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return (false, "Mất kết nối cơ sở dữ liệu");
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống");
            }
            return (true, "Xóa nhân viên thành công");
        }
        public async Task<(bool, string)> UpdatePassword(string staffid,string pass)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    var selectStaff = await context.Staffs.FindAsync(staffid);
                    selectStaff.Password=pass;

                    await context.SaveChangesAsync();
                }
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return (false, "Mất kết nối cơ sở dữ liệu");
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống");
            }
            return (true, "Cập nhật mật khẩu thành công");
        }

        public async Task<(bool, string, StaffDTO)> CheckLogin(string username, string password)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    var staff = await (from s in context.Staffs
                                       where (username == s.Username || username == s.Email) && password == s.Password
                                       select new StaffDTO
                                       {
                                           StaffId=s.StaffId,
                                           StaffName=s.StaffName,
                                           PhoneNumber=s.PhoneNumber,
                                           StaffAddress=s.StaffAddress,
                                           Email=s.Email,
                                           CCCD=s.CCCD,
                                           DateOfBirth=s.DateOfBirth,
                                           Position=s.Position,
                                           Gender=s.Gender,
                                           Username=s.Username,
                                           Password=s.Password,
                                           Avatar=s.Avatar,
                                           dateOfStart=s.dateOfStart
                                       }).FirstOrDefaultAsync();
                    if(staff == null)
                    {
                        return (false, "Sai tài khoản hoặc mật khẩu", null);
                    }
                    return (true, "", staff);
                }
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return (false, "Mất kết nối cơ sở dữ liệu", null);
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống", null);
            }
        }
        private string CreateNextStaffId(string maxId)
        {
            if (maxId is null)
            {
                return "NV001";
            }
            string newIdString = $"000{int.Parse(maxId.Substring(2)) + 1}";
            return "NV" + newIdString.Substring(newIdString.Length - 3, 3);
        }
    }
}
