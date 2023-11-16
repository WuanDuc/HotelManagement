﻿using HotelManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace HotelManagement.Model.Services
{
    public class CustomerService
    {
        public HotelManagementEntities entities;
        private static CustomerService _ins;
        public static CustomerService Ins
        {
            get {
                if (_ins == null)
                {
                    _ins = new CustomerService();
                }
                return _ins; }
            private set { _ins = value; }
        }
        private CustomerService() { }
        public CustomerService(HotelManagementEntities entities)
        {
            this.entities = entities;
        }

        public async Task<List<CustomerDTO>> GetAllCustomer()
        {
            List<CustomerDTO> customerList;
            try
            {
                if (entities == null)
                {
                    entities = new HotelManagementEntities();
                }
                if (entities.Customers == null)
                {
                    return null;
                }
                customerList =(from s in entities.Customers
                                  where s.IsDeleted == false
                                  select new CustomerDTO {
                                        CustomerId = s.CustomerId,
                                        CustomerName=s.CustomerName,
                                        PhoneNumber=s.PhoneNumber,
                                        Email=s.Email,
                                        CCCD=s.CCCD,
                                        DateOfBirth= (DateTime)s.DateOfBirth,
                                        Gender=s.Gender,
                                        CustomerAddress=s.CustomerAddress,
                                        CustomerType=s.CustomerType,
                                        IsDeleted= (bool)s.IsDeleted,

                                  }).ToList();
                
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return customerList;
        }
        public async Task<(bool, string, CustomerDTO)> AddCustomer(CustomerDTO newCus)
        {
            try
            {
                if (entities == null)
                {
                    entities = new HotelManagementEntities();
                }
                if (entities.Customers == null)
                {
                    return (false, "Mất kết nối cơ sở dữ liệu", null);
                }
                bool isCccdExist = await entities.Customers.AnyAsync(s => newCus.CCCD == s.CCCD);
                    if(isCccdExist) return (false, "CCCD đã tồn tại!", null);
                    var maxId = await entities.Customers.MaxAsync(s => s.CustomerId );
                    
                    Customer cus = new Customer();
                    cus.CustomerId = CreateNextCustomerId(maxId);
                    cus.CustomerName = newCus.CustomerName;
                    cus.DateOfBirth = newCus.DateOfBirth;
                    cus.PhoneNumber = newCus.PhoneNumber;
                    cus.Email = newCus.Email;
                    cus.CCCD=newCus.CCCD;
                    cus.CustomerAddress = newCus.CustomerAddress;
                    cus.CustomerType = newCus.CustomerType;
                    cus.Gender = newCus.Gender;
                    cus.IsDeleted=newCus.IsDeleted;

                    newCus.CustomerId = cus.CustomerId;
                    entities.Customers.Add(cus);
                    await entities.SaveChangesAsync();
                
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return (false, "Mất kết nối cơ sở dữ liệu", null);
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống", null);
            }
            return (true, "Thêm khách hàng mới thành công", newCus);
        }
        public async Task<CustomerDTO> GetCustomerByCCCD(string cccd)
        {
            try
            {
                if (entities == null)
                {
                    entities = new HotelManagementEntities();
                }
                if (entities.Customers == null)
                {
                    return null;
                }
                var cus = await entities.Customers.FirstOrDefaultAsync(x=> x.CCCD== cccd);
                    return new CustomerDTO
                    {
                        CustomerId = cus.CustomerId,
                        CustomerName = cus.CustomerName,
                        PhoneNumber = cus.PhoneNumber,
                        Email = cus.Email,
                        CCCD = cus.CCCD,
                        DateOfBirth = (DateTime)cus.DateOfBirth,
                        Gender = cus.Gender,
                        CustomerAddress = cus.CustomerAddress,
                        CustomerType = cus.CustomerType,
                    };
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public async Task<(bool , string)> UpdateCustomerInfo(CustomerDTO customer)
        {
            try
            {
                if (entities == null)
                {
                    entities = new HotelManagementEntities();
                }
                if (entities.Customers == null)
                {
                    return (false, "Mất kết nối cơ sở dữ liệu");
                }
                bool isCccdExist = await entities.Customers.AnyAsync(s => s.CustomerId!=customer.CustomerId && s.CCCD == customer.CCCD);
                    if (isCccdExist) return (false, "CCCD đã tồn tại!");
                    Customer selectedCus = await entities.Customers.FindAsync(customer.CustomerId);
                    selectedCus.CustomerName = customer.CustomerName;
                    selectedCus.DateOfBirth = customer.DateOfBirth;
                    selectedCus.PhoneNumber = customer.PhoneNumber;
                    selectedCus.Email = customer.Email;
                    selectedCus.CCCD = customer.CCCD;
                    selectedCus.CustomerAddress = customer.CustomerAddress;
                    selectedCus.CustomerType = customer.CustomerType;
                    selectedCus.Gender = customer.Gender;

                    await entities.SaveChangesAsync();
                
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return (false, "Mất kết nối cơ sở dữ liệu");
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống");
            }
            return (true, "Chỉnh sửa thông tin khách hàng thành công");
        }
        public async Task<(bool, string)> DeleteCustomer(string id)
        {
            try
            {
                if (entities == null)
                {
                    entities = new HotelManagementEntities();
                }
                if (entities.Customers == null)
                {
                    return (false, "Mất kết nối cơ sở dữ liệu");
                }
                Customer selectedCus = await ( from s in entities.Customers
                                                   where s.CustomerId==id && s.IsDeleted==false
                                                   select s).FirstOrDefaultAsync();
                    if(selectedCus is null || selectedCus.IsDeleted == true)
                    {
                        return (false, "Khách hàng không tồn tại" );

                    }
                    selectedCus.IsDeleted = true;

                    await entities.SaveChangesAsync();
                
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return (false, "Mất kết nối cơ sở dữ liệu");
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống");
            }
            return (true, "Xóa khách hàng thành công");
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

    }
}
