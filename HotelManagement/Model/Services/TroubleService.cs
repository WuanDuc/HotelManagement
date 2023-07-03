using HotelManagement.DTOs;
using HotelManagement.Utilities;
using HotelManagement.Utils;
using HotelManagement.ViewModel.StaffVM;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace HotelManagement.Model.Services
{
    public class TroubleService
    {
        private static TroubleService _ins;
        public static TroubleService Ins
        {
            get { return _ins ?? (_ins = new TroubleService()); }
            private set { _ins = value; }
        }
        private TroubleService(){}
        public async Task<List<TroubleDTO>> GetAllTrouble()
        {
            List<TroubleDTO> Troublelist;
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    Troublelist = (from s in context.Troubles
                                   select new TroubleDTO {
                                       TroubleId = s.TroubleId,
                                       Title = s.Title,
                                       Avatar = s.Avatar,
                                       Description = s.Description,
                                       Reason = s.Reason,
                                       Price = (double)s.Price,
                                       StartDate = s.StartDate,
                                       FixedDate = s.FixedDate,
                                       FinishDate = s.FinishDate,
                                       Status = s.Status,
                                       StaffId=s.StaffId,
                                       Level = s.Level,
                                   }) .ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Troublelist.Reverse();
            return Troublelist;
        }
        public async Task<(bool, string, TroubleDTO)> AddTrouble(TroubleDTO s, string id  )
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {                    
                    var maxId = await context.Troubles.MaxAsync(p => p.TroubleId);

                    Trouble newtrouble = new Trouble { 
                        TroubleId = CreateNextTroubleId(maxId),
                        Title = s.Title,
                        Avatar = s.Avatar,
                        Description = s.Description,
                        Reason = s.Reason,
                        Price = 0,
                        StartDate = s.StartDate,
                        Status = STATUS.WAITING,
                        StaffId = s.StaffId,
                        Level = s.Level ?? LEVEL.NORMAL,
                    };
                    if (id != null)
                    {
                        TroubleByCustomer tb = new TroubleByCustomer
                        {
                            TroubleId=newtrouble.TroubleId,
                            RentalContractId=id
                        };
                        context.TroubleByCustomers.Add(tb);
                    }
                    s.TroubleId = newtrouble.TroubleId;
                    context.Troubles.Add(newtrouble);
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
            return (true, "Báo cáo sự cố mới thành công", s);

        }
        public async Task<(bool,string)> UpdateTrouble(TroubleDTO s, double preprice=0)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    if (s.Status == STATUS.PREDIT)
                    {
                        var troublebycus = await context.TroubleByCustomers.FirstOrDefaultAsync(x => x.TroubleId == s.TroubleId);
                        troublebycus.PredictedPrice=preprice;
                        var trouble = await context.Troubles.FindAsync(s.TroubleId);
                        trouble.Status=STATUS.PREDIT;
                    }
                    else if(s.Status == STATUS.IN_PROGRESS)
                    {
                        var trouble = await context.Troubles.FindAsync(s.TroubleId);
                        trouble.Status = STATUS.IN_PROGRESS;
                        trouble.FixedDate = s.FixedDate;
                        if (trouble.Reason == REASON.BYCUSTOMER)
                        {
                            var troublebycus = await context.TroubleByCustomers.FindAsync(s.TroubleId);
                            troublebycus.PredictedPrice = preprice;
                        }
                    }
                    else if(s.Status == STATUS.DONE)
                    {
                        var trouble = await context.Troubles.FindAsync(s.TroubleId);
                        trouble.Status = STATUS.DONE;
                        trouble.FixedDate = s.FixedDate;
                        trouble.FinishDate = s.FinishDate;
                        trouble.Price = s.Price;
                        if (trouble.Reason == REASON.BYCUSTOMER)
                        {
                            var troublebycus = await context.TroubleByCustomers.FindAsync(s.TroubleId);
                            troublebycus.PredictedPrice = s.Price;
                        }
                        
                    }
                    else if (s.Status == STATUS.CANCLE)
                    {
                        var trouble = await context.Troubles.FindAsync(s.TroubleId);
                        trouble.Status = STATUS.CANCLE;
                        if (trouble.Reason == REASON.BYCUSTOMER)
                        {
                            var troublebycus = await context.TroubleByCustomers.FindAsync(s.TroubleId);
                            troublebycus.PredictedPrice = 0;
                        }
                    }
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
            return (true, "Cập nhật sự cố thành công");
        }
        public async Task<(bool, string)> EditTrouble(TroubleDTO s, string id)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    var trouble = await context.Troubles.FindAsync(s.TroubleId);
                    trouble.Title = s.Title;
                    trouble.Avatar = s.Avatar;
                    trouble.Description = s.Description;
                    trouble.Reason = s.Reason;
                    trouble.StartDate = DateTime.Now;
                    trouble.StaffId = s.StaffId;
                    trouble.Level = s.Level ;
                    var tb = await context.TroubleByCustomers.FindAsync(s.TroubleId);
                    if (id != null)
                    {
                        if(tb != null)
                        {
                            tb.RentalContractId = id;
                        }
                        else
                        {
                            TroubleByCustomer tbb = new TroubleByCustomer
                            {
                                TroubleId = trouble.TroubleId,
                                RentalContractId = id
                            };
                            context.TroubleByCustomers.Add(tbb);
                        }               
                    }
                    else
                    {
                        if (tb != null)
                        {
                            tb.TroubleId = null;
                            tb.RentalContractId = null;
                        }
                    }
                    
                   
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
            return (true, "Chỉnh sửa sự cố thành công");

        }
        public async Task<string> GetStaffNameById(string id)
        {
            string staffname;
            using ( var context = new HotelManagementEntities())
            {
                var staff = await context.Staffs.FindAsync(id);
                staffname = staff.StaffName;
            }
            return staffname;
        }
        public async Task<TroubleByCustomerDTO> GetTroubleByCus(string id)
        {
            TroubleByCustomerDTO tb;
            using(var context = new HotelManagementEntities())
            {
                var trouble =  context.TroubleByCustomers.FirstOrDefault(x=> x.TroubleId == id);
                tb = new TroubleByCustomerDTO
                {
                    TroubleId=trouble.TroubleId,
                    RentalContractId=trouble.RentalContractId,
                    PredictedPrice=trouble.PredictedPrice,
                };
            }
            return tb;
        }
        public async Task<List<TroubleByCustomerDTO>> GetListTroubleByCustomer(string rentalContractId)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    var listTroubleByCustomer = await context.TroubleByCustomers.Where(x=> x.RentalContractId ==rentalContractId)
                        .Select(x=> new TroubleByCustomerDTO
                        {
                           RentalContractId = x.RentalContractId,
                           TroubleId=x.TroubleId,
                           Title = x.Trouble.Title,
                           PredictedPrice=x.PredictedPrice,
                           Level = x.Trouble.Level,
                        }).ToListAsync();
                    return listTroubleByCustomer;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
        public async Task<List<string>> GetCurrentListRentalContractId()
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {

                   var list = await context.RentalContracts.Where(x=> x.Room.RoomStatus == ROOM_STATUS.RENTING && x.Validated==true).Select(x=> x.RentalContractId).ToListAsync();
                    return list;
                }
            }
           
            catch (Exception)
            {
                return (null);
            }

        }
        private string CreateNextTroubleId(string maxId)
        {
            if (maxId is null)
            {
                return "SC001";
            }
            string newIdString = $"000{int.Parse(maxId.Substring(2)) + 1}";
            return "SC" + newIdString.Substring(newIdString.Length - 3, 3);
        }
    }
}
