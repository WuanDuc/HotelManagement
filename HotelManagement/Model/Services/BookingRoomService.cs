using HotelManagement.DTOs;
using HotelManagement.Utils;
using IronXL.Formatting;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Model.Services
{
    public class BookingRoomService
    {
        private static BookingRoomService _ins;
        public static BookingRoomService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new BookingRoomService();
                }
                return _ins;
            }
            private set { _ins = value; }
        }

        public BookingRoomService() { }

        public async Task<List<RentalContractDTO>> GetBookingList()
        {
            try
            {
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    List<RentalContractDTO> RentalContractDTOs = await (
                        from r in db.RentalContracts
                        join s in db.Staffs
                        on r.StaffId equals s.StaffId
                        join c in db.Customers
                        on r.CustomerId equals c.CustomerId
                        join room in db.Rooms
                        on r.RoomId equals room.RoomId
                        join rt in db.RoomTypes
                        on room.RoomTypeId equals rt.RoomTypeId
                        select new RentalContractDTO
                        {
                            RentalContractId = r.RentalContractId,
                            StaffName = s.StaffName,
                            CustomerName = c.CustomerName,
                            StartDate = r.StartDate,
                            CheckOutDate = r.CheckOutDate,
                            StartTime = r.StartTime,
                            Validated = r.Validated,
                        }
                    ).ToListAsync();
                    RentalContractDTOs.Reverse();
                  
                    return RentalContractDTOs;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<List<RoomDTO>> GetListReadyRoom(DateTime StartDate, DateTime StartTime, DateTime CheckOutDate)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    var listRentalContract = await (from r in context.RentalContracts

                                                    select new RentalContractDTO
                                                    {
                                                        RentalContractId = r.RentalContractId,
                                                        StartDate = r.StartDate,
                                                        StartTime = r.StartTime,
                                                        CheckOutDate = r.CheckOutDate,
                                                        CustomerId = r.CustomerId,
                                                        RoomId = r.RoomId,
                                                        Validated = r.Validated,
                                                    }
                                          ).ToListAsync();
                    DateTime start = StartDate + StartTime.TimeOfDay;
                    DateTime end = CheckOutDate + StartTime.TimeOfDay;

                    var listBusyRoomId = listRentalContract.Where(x =>
                    (x.CheckOutDate + x.StartTime >= start && x.CheckOutDate + x.StartTime < end && x.Validated==true) ||
                    (x.StartDate + x.StartTime >= start && x.StartDate + x.StartTime < end && x.Validated == true) ||
                    (x.StartDate + x.StartTime <= start && x.CheckOutDate + x.StartTime >= end && x.Validated == true)).Select(x => x.RoomId).ToList();
                    var listReadyRoom = await (
                        from r in context.Rooms
                        join temp in context.RoomTypes
                        on r.RoomTypeId equals temp.RoomTypeId 
                        where listBusyRoomId.Contains(r.RoomId)==false
                        select new RoomDTO
                        {
                            // DTO = db
                            RoomId = r.RoomId,
                            RoomNumber = (int)r.RoomNumber,
                            RoomTypeName = temp.RoomTypeName,
                            RoomTypeId = temp.RoomTypeId,
                            Price = (double)temp.Price,
                        }
                    ).ToListAsync();
                   return listReadyRoom;

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<(bool,string)> SaveBooking(RentalContractDTO rentalContract)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    var maxId = await context.RentalContracts.MaxAsync(s => s.RentalContractId);
                    string rentalId = CreateNextRentalContractId(maxId);
                    RentalContract rc = new RentalContract
                    {
                        RentalContractId = rentalId,
                        RoomId= rentalContract.RoomId,
                        StaffId = rentalContract.StaffId,
                        StartDate = rentalContract.StartDate,
                        StartTime = rentalContract.StartTime,
                        CheckOutDate = rentalContract.CheckOutDate,
                        CustomerId = rentalContract.CustomerId,
                        Validated = rentalContract.Validated,
                    };

                    context.RentalContracts.Add(rc);
                    await context.SaveChangesAsync();
                    rentalContract.RentalContractId = rc.RentalContractId;
                    return (true, "Đặt phòng thành công!");
                }
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống");
            }
        }
        public async Task<(bool, string, string)> SaveCustomer(CustomerDTO customer)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    var c = await context.Customers.FirstOrDefaultAsync(x => x.CCCD == customer.CCCD);
                    var maxId = await context.Customers.MaxAsync(s => s.CustomerId);
                    if (c != null) return (true, null,c.CustomerId);
                    else
                    {
                        string newCusId = CreateNextCustomerId(maxId);
                        Customer newCus = new Customer
                        {
                            CustomerName = customer.CustomerName,
                            CCCD = customer.CCCD,
                            CustomerAddress = customer.CustomerAddress,
                            PhoneNumber = customer.PhoneNumber,
                            Email = customer.Email,
                            CustomerId = newCusId,
                            CustomerType = customer.CustomerType,
                            DateOfBirth = customer.DateOfBirth,
                            Gender = customer.Gender,
                            IsDeleted = customer.IsDeleted,
                        };
                        context.Customers.Add(newCus);
                        await context.SaveChangesAsync();
                        string cusId = (await context.Customers.FirstOrDefaultAsync(x => x.CCCD == customer.CCCD)).CustomerId;
                        return (true, "", cusId);
                    }
                }
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return (false, "Mất kết nối cơ sở dữ liệu",null);
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống",null);
            }
        }

        public async Task<(bool, string)> DeleteRentalContractBooked(string Id)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    RentalContract rental = await (from p in context.RentalContracts
                                       where p.RentalContractId == Id
                                       select p).FirstOrDefaultAsync();
                    if (rental == null)
                    {
                        return (false, "Phiếu thuê phòng này không tồn tại!");
                    }

                    rental.Room.RoomStatus = ROOM_STATUS.READY;

                    context.RentalContracts.Remove(rental);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống");
            }
            return (true, "Xóa phiếu thuê phòng thành công");
        }

        public async Task<(bool, string)> DeleteRentalContractOutDate(string Id)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    RentalContract rental = await (from p in context.RentalContracts
                                                   where p.RentalContractId == Id
                                                   select p).FirstOrDefaultAsync();
                    if (rental == null)
                    {
                        return (false, "Phiếu thuê phòng này không tồn tại!");
                    }

                    List<ServiceUsing> serviceUsing = await context.ServiceUsings.Where(s => s.RentalContractId == rental.RentalContractId).ToListAsync();

                    if(serviceUsing != null)
                        context.ServiceUsings.RemoveRange(serviceUsing);

                    List<Bill> bill = await context.Bills.Where(s => s.RentalContractId == rental.RentalContractId).ToListAsync();

                    if (bill != null)
                        context.Bills.RemoveRange(bill);

                    List<TroubleByCustomer> troubleByCus = await context.TroubleByCustomers.Where(s => s.RentalContractId == rental.RentalContractId).ToListAsync();

                    if (troubleByCus != null)
                        context.TroubleByCustomers.RemoveRange(troubleByCus);

                    List<RoomCustomer> roomCus = await context.RoomCustomers.Where(s => s.RentalContractId == rental.RentalContractId).ToListAsync();

                    if (roomCus != null)
                        context.RoomCustomers.RemoveRange(roomCus);

                    context.RentalContracts.Remove(rental);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống!");
            }
            return (true, "Xóa phiếu thuê phòng thành công");
        }
        public async Task<string> GetRoomStatusBy(string rtID)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    return (await context.RentalContracts.FindAsync(rtID)).Room.RoomStatus;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<(bool, CustomerDTO)> CheckCCCD(string cccd)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    Customer cus = await context.Customers.FirstOrDefaultAsync(x => x.CCCD == cccd);
                    if (cus != null)
                    {
                        CustomerDTO customerDTO = new CustomerDTO
                        {
                            CustomerId = cus.CustomerId,
                            CustomerName = cus.CustomerName,
                            CCCD = cus.CCCD,
                            CustomerAddress = cus.CustomerAddress,
                            DateOfBirth = cus.DateOfBirth,
                            CustomerType = cus.CustomerType,
                            Email = cus.Email,
                            Gender = cus.Gender,
                            PhoneNumber = cus.PhoneNumber,
                        };

                        return (true, customerDTO);
                    }
                    else
                    {
                        return (false, null);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private string CreateNextCustomerId(string maxId)
        {
            //KHxxx
            if (maxId is null)
            {
                return "KH001";
            }
            string newIdString = $"000{int.Parse(maxId.Substring(2)) + 1}";
            return "KH" + newIdString.Substring(newIdString.Length - 3, 3);
        }
        private string CreateNextRentalContractId(string maxId)
        {
            //KHxxx
            if (maxId is null)
            {
                return "PT001";
            }
            string newIdString = $"000{int.Parse(maxId.Substring(2)) + 1}";
            return "PT" + newIdString.Substring(newIdString.Length - 3, 3);
        }

    }
}
