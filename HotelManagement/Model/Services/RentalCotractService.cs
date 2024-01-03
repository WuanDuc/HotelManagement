using HotelManagement.DTOs;
using HotelManagement.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Model.Services
{
    public class RentalContractService
    {
        public HotelManagementEntities context;
        private static RentalContractService _ins;
        public static RentalContractService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new RentalContractService();
                }
                return _ins;
            }
            private set => _ins = value;
        }
        private RentalContractService() { }
        public RentalContractService(HotelManagementEntities context)
        {
            this.context = context;
        }

        public async Task<List<RentalContractDTO>> GetAllRentalContracts()
        {
            try
            {
                if (context == null)
                {
                    context = new HotelManagementEntities();
                }
                if (context.RentalContracts == null)
                {
                    return null;
                }
                var rentalContractList = await (from r in context.RentalContracts

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

                    return rentalContractList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<RentalContractDTO>> GetRentalContractsNow()
        {
            try
            {
                if (context == null)
                {
                    context = new HotelManagementEntities();
                }
                if (context.RentalContracts == null)
                {
                    return null;
                }
                var rentalContractList = await (from r in context.RentalContracts

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
                    rentalContractList = rentalContractList.Where(x => x.CheckOutDate + x.StartTime > DateTime.Today + DateTime.Now.TimeOfDay && x.StartDate + x.StartTime <= DateTime.Today + DateTime.Now.TimeOfDay).ToList();

                    return rentalContractList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<RentalContractDTO> GetRentalContractById(string rentalContractId)
        {
            try
            {
                if (context == null)
                {
                    context = new HotelManagementEntities();
                }
                if (context.RentalContracts == null)
                {
                    return null;
                }
                var res = await context.RentalContracts.Select(x => new RentalContractDTO
                    {
                        RentalContractId = x.RentalContractId,
                        RoomId = x.RoomId,
                        RoomNumber = x.Room.RoomNumber,
                        CustomerName = x.Customer.CustomerName,
                        PersonNumber = x.PersonNumber == null ? 0 : (int)x.PersonNumber,
                        CheckOutDate = x.CheckOutDate,
                        CustomerId = x.CustomerId,
                        StartDate = x.StartDate,
                        StartTime = x.StartTime,
                        Validated = x.Validated,
                    }).FirstAsync(x => x.RentalContractId == rentalContractId);
                    return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<RentalContractDTO>> GetRentalContractByCustomer(string customerId)
        {
            try
            {
                if (context == null)
                {
                    context = new HotelManagementEntities();
                }
                if (context.RentalContracts == null)
                {
                    return null;
                }
                var list = await context.RentalContracts.Where(x => x.CustomerId == customerId && x.Room.RoomStatus == ROOM_STATUS.RENTING && x.Validated == true).Select(x => new RentalContractDTO
                    {
                        RentalContractId = x.RentalContractId,
                        RoomId = x.RoomId,
                        RoomTypeName = x.Room.RoomType.RoomTypeName,
                        RoomNumber = x.Room.RoomNumber,
                        RoomPrice = x.Room.RoomType.Price,
                        CustomerId = x.CustomerId,
                        CustomerName = x.Customer.CustomerName,
                        CustomerAddress = x.Customer.CustomerAddress,

                        StartDate = x.StartDate,
                        StartTime = x.StartTime,
                        CheckOutDate = x.CheckOutDate,
                        Validated = x.Validated,
                    }).ToListAsync();
                    return list;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public async Task<List<RoomCustomerDTO>> GetCustomersOfRoom(string RentalContractId)
        //{
        //    try
        //    {
        //        using (var context = new HotelManagementEntities())
        //        {
        //            var listCustomerId = await context.RoomCustomers.Where(x=> x.RentalContractId == RentalContractId).Select(x=> x.CustomerId).ToListAsync();
        //            var listCustomer = await context.Customers.Where(x => listCustomerId.Contains(x.CustomerId)).Select(x => new RoomCustomerDTO
        //            {
        //                CustomerName = x.CustomerName,
        //                CustomerType = x.CustomerType,
        //                CCCD = x.CCCD,
        //                CustomerAddress= x.CustomerAddress,
        //            }).ToListAsync();
        //            for (int i=0; i<listCustomer.Count; i++)
        //            {
        //                listCustomer[i].STT = i + 1;
        //            }

        //            return listCustomer;

        //return listCustomer;
        //return new List<RoomCustomerDTO>();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<int> GetPersonNumber(string rentalContractId)
        //{
        //    try
        //    {
        //        using (var context = new HotelManagementEntities())
        //        {
        //            var customerList = await context.RoomCustomers.Where(x => x.RentalContractId == rentalContractId).Select(x => x.CustomerId).CountAsync();
        //            return customerList;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
