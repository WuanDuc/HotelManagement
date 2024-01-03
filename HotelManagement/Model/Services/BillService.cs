using HotelManagement.DTOs;
using HotelManagement.Utils;
//using HotelManagement.ViewModel.StaffVM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Model.Services
{
    public class BillService
    {
        public BillService() { }
        private static BillService _ins;
        public static BillService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new BillService(new HotelManagementEntities());
                }
                return _ins;
            }
            private set { _ins = value; }
        }
        public HotelManagementEntities _context;

        public BillService(HotelManagementEntities context)
        {
            _context = context;
        }
        public async Task<List<BillDTO>> GetBillByListRentalContract(List<RentalContractDTO> rentalContractDTOs)
        {
            try
            {
                    if (_context == null)
                    {
                        _context = new HotelManagementEntities();
                    }
                    if (_context.Bills == null)
                    {
                        return null;
                    }
                    var listRentalContractId = rentalContractDTOs.Select(x => x.RentalContractId).ToList();

                    var list = await _context.RentalContracts.Where(x => listRentalContractId.Contains(x.RentalContractId))
                         .Select(x => new BillDTO
                         {
                             RentalContractId = x.RentalContractId,
                             CustomerId = x.CustomerId,
                             CustomerName = x.Customer.CustomerName,
                             CustomerAddress = x.Customer.CustomerAddress,
                             RoomId = x.RoomId,
                             RoomNumber = (int)x.Room.RoomNumber,
                             RoomTypeName = x.Room.RoomType.RoomTypeName,
                             StartDate = x.StartDate,
                             StartTime = x.StartTime,
                             CheckOutDate = x.CheckOutDate,
                             PersonNumber = x.RoomCustomers.Count(),
                             IsHasForeignPerson = (x.RoomCustomers.Where(t => t.CustomerType == "Nước ngoài").Count() > 0),
                             RoomPrice = x.Room.RoomType.Price,
                             ListTroubleByCustomer = x.TroubleByCustomers.Select(t => new TroubleByCustomerDTO
                             {
                                 RentalContractId = t.RentalContractId,
                                 TroubleId = t.TroubleId,
                                 Title = t.Trouble.Title,
                                 PredictedPrice = t.PredictedPrice,
                                 Level = t.Trouble.Level,
                             }).ToList(),

                         }).ToListAsync();

                    return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<(bool, string)> SaveBill(BillDTO bill)
        {
            try
            {
                if (_context == null)
                {
                    _context = new HotelManagementEntities();
                }
                if (_context.Bills == null)
                {
                    return (false, "Lỗi hệ thống");
                }
                var maxBillId = await _context.Bills.MaxAsync(x => x.BillId);
                    Bill newBill = new Bill
                    {
                        BillId = CreateNextBillId(maxBillId),
                        RentalContractId = bill.RentalContractId,
                        StaffId = bill.StaffId,
                        NumberOfRentalDays = bill.NumberOfRentalDays,
                        ServicePrice = bill.ServicePrice,
                        TroublePrice = bill.TroublePrice,
                        TotalPrice = bill.TotalPrice,
                        CreateDate = bill.CreateDate,
                    };
                    _context.Bills.Add(newBill);
                    RentalContract rental = await _context.RentalContracts.FindAsync(bill.RentalContractId);
                    //rental.PersonNumber = rental.RoomCustomers.Count();
                    await _context.SaveChangesAsync();
                    return (true, "Thanh toán thành công!");
                }
            catch (Exception ex)
            {
                return (false, "Lỗi hệ thống");
            }
        }
        private string CreateNextBillId(string maxBillId)
        {
            if (maxBillId is null) return "HD001";
            int num = int.Parse(maxBillId.Substring(2));
            string nextNumString = (num + 1).ToString();
            while (nextNumString.Length < 3) nextNumString = "0" + nextNumString;
            return "HD" + nextNumString;

        }
        //public async Task<List<BillDTO>> GetBillsByRentalContracts(List<string> rentalContractIds)
        //{
        //    try
        //    {
        //        using (var context = new HotelManagementEntities())
        //        {
        //            List<BillDTO> listBillDTO = new List<BillDTO>();
        //            foreach (var item in rentalContractIds)
        //            {
        //                var itemBillDTO = await (from r in context.Rooms
        //                                         join c in context.RentalContracts
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public async Task<List<BillDTO>> GetAllBill()
        {
            try
            {
                if (_context == null)
                {
                    _context = new HotelManagementEntities();
                }
                if (_context.Bills == null)
                {
                    return null;
                }
                var billList = (from b in _context.Bills
                                    orderby b.CreateDate descending
                                    select new BillDTO
                                    {
                                        BillId = b.BillId,
                                        RentalContractId = b.RentalContractId,
                                        StaffId = b.StaffId,
                                        StaffName = b.Staff.StaffName,
                                        CustomerAddress = b.RentalContract.Customer.CustomerAddress,
                                        CustomerId = b.RentalContract.CustomerId,
                                        CustomerName = b.RentalContract.Customer.CustomerName,
                                        RoomId = b.RentalContract.RoomId,
                                        RoomNumber = (int)b.RentalContract.Room.RoomNumber,
                                        RoomTypeName = b.RentalContract.Room.RoomType.RoomTypeName,
                                        PersonNumber = (int)b.RentalContract.PersonNumber,
                                        RoomPrice = b.RentalContract.Room.RoomType.Price,
                                        NumberOfRentalDays = b.NumberOfRentalDays,
                                        ServicePrice = b.ServicePrice,
                                        TroublePrice = b.TroublePrice,
                                        TotalPrice = b.TotalPrice,
                                        DiscountPrice = b.DiscountPrice,
                                        Price = b.Price,
                                        StartDate = b.RentalContract.StartDate,
                                        CheckOutDate = b.RentalContract.CheckOutDate,
                                        StartTime = b.RentalContract.StartTime,
                                        CreateDate = b.CreateDate,
                                        ListTroubleByCustomer = b.RentalContract.TroubleByCustomers.Select(t => new TroubleByCustomerDTO
                                        {
                                            RentalContractId = t.RentalContractId,
                                            TroubleId = t.TroubleId,
                                            Title = t.Trouble.Title,
                                            PredictedPrice = t.PredictedPrice,
                                            Level = t.Trouble.Level,
                                        }).ToList()
                                    }).ToListAsync();
                    return await billList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<BillDTO>> GetAllBillByDate(DateTime date)
        {
            try
            {
                if (_context == null)
                {
                    _context = new HotelManagementEntities();
                }
                if (_context.Bills == null)
                {
                    return null;
                }
                var billList = (from b in _context.Bills
                                    where DbFunctions.TruncateTime(b.CreateDate) == date.Date
                                    orderby b.CreateDate descending
                                    select new BillDTO
                                    {
                                        BillId = b.BillId,
                                        RentalContractId = b.RentalContractId,
                                        StaffId = b.StaffId,
                                        StaffName = b.Staff.StaffName,
                                        CustomerAddress = b.RentalContract.Customer.CustomerAddress,
                                        CustomerId = b.RentalContract.CustomerId,
                                        CustomerName = b.RentalContract.Customer.CustomerName,
                                        RoomId = b.RentalContract.RoomId,
                                        RoomNumber = (int)b.RentalContract.Room.RoomNumber,
                                        RoomTypeName = b.RentalContract.Room.RoomType.RoomTypeName,
                                        PersonNumber = (int)b.RentalContract.PersonNumber,
                                        RoomPrice = b.RentalContract.Room.RoomType.Price,
                                        NumberOfRentalDays = b.NumberOfRentalDays,
                                        ServicePrice = b.ServicePrice,
                                        TroublePrice = b.TroublePrice,
                                        TotalPrice = b.TotalPrice,
                                        DiscountPrice = b.DiscountPrice,
                                        Price = b.Price,
                                        StartDate = b.RentalContract.StartDate,
                                        CheckOutDate = b.RentalContract.CheckOutDate,
                                        StartTime = b.RentalContract.StartTime,
                                        CreateDate = b.CreateDate,
                                        ListTroubleByCustomer = b.RentalContract.TroubleByCustomers.Select(t => new TroubleByCustomerDTO
                                        {
                                            RentalContractId = t.RentalContractId,
                                            TroubleId = t.TroubleId,
                                            Title = t.Trouble.Title,
                                            PredictedPrice = t.PredictedPrice,
                                            Level = t.Trouble.Level,
                                        }).ToList()
                                    }).ToListAsync();
                    return await billList;
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<List<BillDTO>> GetAllBillByMonth(int month)
        {
            try
            {
                if (_context == null)
                {
                    _context = new HotelManagementEntities();
                }
                if (_context.Bills == null)
                {
                    return null;
                }
                var billList = (from b in _context.Bills
                                    where ((DateTime)b.CreateDate).Year == DateTime.Now.Year && ((DateTime)b.CreateDate).Month == month
                                    orderby b.CreateDate descending
                                    select new BillDTO
                                    {
                                        BillId = b.BillId,
                                        RentalContractId = b.RentalContractId,
                                        StaffId = b.StaffId,
                                        StaffName = b.Staff.StaffName,
                                        CustomerAddress = b.RentalContract.Customer.CustomerAddress,
                                        CustomerId = b.RentalContract.CustomerId,
                                        CustomerName = b.RentalContract.Customer.CustomerName,
                                        RoomId = b.RentalContract.RoomId,
                                        RoomNumber = (int)b.RentalContract.Room.RoomNumber,
                                        RoomTypeName = b.RentalContract.Room.RoomType.RoomTypeName,
                                        PersonNumber = (int)b.RentalContract.PersonNumber,
                                        RoomPrice = b.RentalContract.Room.RoomType.Price,
                                        NumberOfRentalDays = b.NumberOfRentalDays,
                                        ServicePrice = b.ServicePrice,
                                        TroublePrice = b.TroublePrice,
                                        TotalPrice = b.TotalPrice,
                                        DiscountPrice = b.DiscountPrice,
                                        Price = b.Price,
                                        StartDate = b.RentalContract.StartDate,
                                        CheckOutDate = b.RentalContract.CheckOutDate,
                                        StartTime = b.RentalContract.StartTime,
                                        CreateDate = b.CreateDate,
                                        ListTroubleByCustomer = b.RentalContract.TroubleByCustomers.Select(t => new TroubleByCustomerDTO
                                        {
                                            RentalContractId = t.RentalContractId,
                                            TroubleId = t.TroubleId,
                                            Title = t.Trouble.Title,
                                            PredictedPrice = t.PredictedPrice,
                                            Level = t.Trouble.Level,
                                        }).ToList()
                                    }).ToListAsync();
                    return await billList;
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<BillDTO> GetBillDetails(string id)
        {
            try
            {
                if (_context == null)
                {
                    _context = new HotelManagementEntities();
                }
                if (_context.Bills == null)
                {
                    return null;
                }
                var b = await _context.Bills.FindAsync(id);

                    BillDTO billdetail = new BillDTO
                    {
                        BillId = b.BillId,
                        RentalContractId = b.RentalContractId,
                        StaffId = b.StaffId,
                        StaffName = b.Staff.StaffName,
                        CustomerAddress = b.RentalContract.Customer.CustomerAddress,
                        CustomerId = b.RentalContract.CustomerId,
                        CustomerName = b.RentalContract.Customer.CustomerName,
                        RoomId = b.RentalContract.RoomId,
                        RoomNumber = (int)b.RentalContract.Room.RoomNumber,
                        RoomTypeName = b.RentalContract.Room.RoomType.RoomTypeName,
                        PersonNumber = (int)b.RentalContract.PersonNumber,
                        RoomPrice = b.RentalContract.Room.RoomType.Price,
                        NumberOfRentalDays = b.NumberOfRentalDays,
                        ServicePrice = b.ServicePrice,
                        TroublePrice = b.TroublePrice,
                        TotalPrice = b.TotalPrice,
                        DiscountPrice = b.DiscountPrice,
                        Price = b.Price,
                        StartDate = b.RentalContract.StartDate,
                        CheckOutDate = b.RentalContract.CheckOutDate,
                        StartTime = b.RentalContract.StartTime,
                        CreateDate = b.CreateDate,
                        ListTroubleByCustomer = b.RentalContract.TroubleByCustomers.Select(t => new TroubleByCustomerDTO
                        {
                            RentalContractId = t.RentalContractId,
                            TroubleId = t.TroubleId,
                            Title = t.Trouble.Title,
                            PredictedPrice = t.PredictedPrice,
                            Level = t.Trouble.Level,
                        }).ToList()
                    };
                    return billdetail;
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
