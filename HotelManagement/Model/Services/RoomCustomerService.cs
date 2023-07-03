using HotelManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Model.Services
{
    public class RoomCustomerService
    {
        private static RoomCustomerService _ins;
        public static RoomCustomerService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new RoomCustomerService();
                }
                return _ins;
            }
            private set => _ins = value;
        }
        private RoomCustomerService() { }

        public async Task<List<RoomCustomerDTO>> GetCustomersOfRoom(string RentalContractId)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    
                    var listCustomer = await context.RoomCustomers.Where(x=> x.RentalContractId == RentalContractId).Select(x => new RoomCustomerDTO
                    {
                        CustomerName = x.CustomerName,
                        CustomerType = x.CustomerType, 
                        CCCD = x.CCCD,
                        CustomerAddress = x.CustomerAddress,
                        RentalContractId= x.RentalContractId,
                        RoomCustomerId= x.RoomCustomerId,   
                    }).ToListAsync();
                    for (int i = 0; i < listCustomer.Count; i++)
                    {
                        listCustomer[i].STT = i + 1;
                    }

                    return listCustomer;


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> GetPersonNumberOfRoom(string rentalContractId)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    var customerList = await context.RoomCustomers.Where(x => x.RentalContractId == rentalContractId).CountAsync();

                    return customerList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(bool, string, List<RoomCustomerDTO>)> AddRoomCustomer(RoomCustomerDTO roomCustomer)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    var listCCCD = await context.RoomCustomers.Where(x=> x.RentalContractId == roomCustomer.RentalContractId).Select(x=> x.CCCD).ToListAsync(); 
                    if (listCCCD != null)
                    {
                        if (listCCCD.Contains(roomCustomer.CCCD))
                        {
                            return (false, "Thêm thất bại! Mã CCCD bị trùng!", null);
                        }
                    }
                    RoomCustomer rc = new RoomCustomer
                    {
                        CustomerName = roomCustomer.CustomerName,
                        CustomerAddress = roomCustomer.CustomerAddress,
                        CustomerType = roomCustomer.CustomerType,
                        CCCD = roomCustomer.CCCD,
                        RentalContractId= roomCustomer.RentalContractId,
                    };
                    context.RoomCustomers.Add(rc);
                    await context.SaveChangesAsync();
                    
                    
                    var listCustomer = await context.RoomCustomers.Where(x=> x.RentalContractId == roomCustomer.RentalContractId).Select(x=> new RoomCustomerDTO
                    {
                        CustomerName = x.CustomerName,
                        CustomerType = x.CustomerType,
                        CCCD = x.CCCD,
                        CustomerAddress = x.CustomerAddress,
                        RentalContractId = x.RentalContractId,
                        RoomCustomerId = x.RoomCustomerId,
                    }).ToListAsync();
                    for (int i = 0; i < listCustomer.Count; i++)
                    {
                        listCustomer[i].STT = i + 1;
                    }

                    return (true, "Thêm khách ở thành công!", listCustomer);
                }
            }
            catch(Exception ex)
            {
                return (false, "Lỗi hệ thống", null);
            }
        }
        public async Task<(bool, string, List<RoomCustomerDTO>)> UpdateRoomCustomer(RoomCustomerDTO roomCustomer)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    
                    RoomCustomer cus = await context.RoomCustomers.Where(x => x.RoomCustomerId == roomCustomer.RoomCustomerId).FirstOrDefaultAsync();
                    var listCCCD = await context.RoomCustomers.Where(x => x.RentalContractId == roomCustomer.RentalContractId && x.RoomCustomerId != cus.RoomCustomerId).Select(x => x.CCCD).ToListAsync();
                    if (listCCCD != null)
                    {
                        if (listCCCD.Contains(roomCustomer.CCCD))
                        {
                            return (false, "Cập nhật thất bại! Mã CCCD bị trùng!", null);
                        }
                    }
                    cus.CustomerName = roomCustomer.CustomerName;
                    cus.CustomerAddress = roomCustomer.CustomerAddress;
                    cus.CustomerType = roomCustomer.CustomerType;
                    cus.CCCD = roomCustomer.CCCD;
                 
                    await context.SaveChangesAsync();

                    var listCustomer = await context.RoomCustomers.Where(x => x.RentalContractId == roomCustomer.RentalContractId).Select(x => new RoomCustomerDTO
                    {
                        CustomerName = x.CustomerName,
                        CustomerType = x.CustomerType,
                        CCCD = x.CCCD,
                        CustomerAddress = x.CustomerAddress,
                        RentalContractId = x.RentalContractId,
                        RoomCustomerId = x.RoomCustomerId,
                    }).ToListAsync();
                    for (int i = 0; i < listCustomer.Count; i++)
                    {
                        listCustomer[i].STT = i + 1;
                    }
                    return (true, "Cập nhật thông tin khách ở thành công!", listCustomer);
                }
            }
            catch (Exception ex)
            {
                return (false, "Lỗi hệ thống", null);
            }
        }
        public async Task<(bool, string, List<RoomCustomerDTO>)> DeleteRoomCustomer(RoomCustomerDTO roomCustomer)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    RoomCustomer cus = await context.RoomCustomers.Where(x => x.RoomCustomerId == roomCustomer.RoomCustomerId).FirstOrDefaultAsync();
                    context.RoomCustomers.Remove(cus);
                    await context.SaveChangesAsync();

                    var listCustomer = await context.RoomCustomers.Where(x => x.RentalContractId == roomCustomer.RentalContractId).Select(x => new RoomCustomerDTO
                    {
                        CustomerName = x.CustomerName,
                        CustomerType = x.CustomerType,
                        CCCD = x.CCCD,
                        CustomerAddress = x.CustomerAddress,
                        RentalContractId = x.RentalContractId,
                        RoomCustomerId = x.RoomCustomerId,
                    }).ToListAsync();
                    for (int i = 0; i < listCustomer.Count; i++)
                    {
                        listCustomer[i].STT = i + 1;
                    }
                    return (true, "Cập nhật thông tin khách ở thành công!", listCustomer);
                }
            }
            catch (Exception ex)
            {
                return (false, "Lỗi hệ thống", null);
            }
        }

    }
}
