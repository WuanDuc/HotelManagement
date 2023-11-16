using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelManagement.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using System.Data.Entity;
using HotelManagementTests.Model;
using System.Data.Entity.Infrastructure;
using HotelManagement.DTOs;

namespace HotelManagement.Model.Services.Tests
{
    [TestClass()]
    public class CustomerServiceTests
    {
        CustomerService service;

        Mock<HotelManagementEntities> mockEntities;
        Mock<DbSet<Customer>> mockCustomer;

        List<Customer> customerDTOs;

        [TestInitialize()]
        public void Setup()
        {
            customerDTOs = new List<Customer> {
                new Customer()
                {
                    CustomerId = "C001",
                    CustomerName = "John Doe",
                    PhoneNumber = "123456789",
                    Email = "john.doe@example.com",
                    CCCD = "123456789",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Gender = "Male",
                    CustomerType = "Regular",
                    CustomerAddress = "123 Main St",
                    IsDeleted = false,
                },

                new Customer
                {
                    CustomerId = "C002",
                    CustomerName = "Jane Doe",
                    PhoneNumber = "987654321",
                    Email = "jane.doe@example.com",
                    CCCD = "987654321",
                    DateOfBirth = new DateTime(1985, 5, 10),
                    Gender = "Female",
                    CustomerType = "VIP",
                    CustomerAddress = "456 Oak St",
                    IsDeleted = false
                },
                new Customer
                {
                    CustomerId = "C003",
                    CustomerName = "Bob Smith",
                    PhoneNumber = "555555555",
                    Email = "bob.smith@example.com",
                    CCCD = "555555555",
                    DateOfBirth = new DateTime(1975, 8, 20),
                    Gender = "Male",
                    CustomerType = "Regular",
                    CustomerAddress = "789 Pine St",
                    IsDeleted = true
                }
            };
            var dataCustomers = customerDTOs.AsQueryable();

            mockCustomer = new Mock<DbSet<Customer>>();
            mockCustomer.As<IDbAsyncEnumerable<Customer>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Customer>(dataCustomers.GetEnumerator()));
            mockCustomer.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<Customer>(dataCustomers.Provider));
            mockCustomer.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(dataCustomers.Expression);
            mockCustomer.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(dataCustomers.ElementType);
            mockCustomer.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(dataCustomers.GetEnumerator());


            //mockEntities.Setup(m => m.Staffs.FindAsync(staffId)).ReturnsAsync(new Staff());
            mockEntities = new Mock<HotelManagementEntities>();
            mockEntities.Setup(m => m.Customers).Returns(mockCustomer.Object);

            service = new CustomerService(mockEntities.Object);
        }
        [TestMethod()]
        public async Task GetAllCustomerTestAsync()
        {
            var expected = new List<CustomerDTO>() {
                new CustomerDTO()
                {
                    CustomerId = "C001",
                    CustomerName = "John Doe",
                    PhoneNumber = "123456789",
                    Email = "john.doe@example.com",
                    CCCD = "123456789",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Gender = "Male",
                    CustomerType = "Regular",
                    CustomerAddress = "123 Main St",
                    IsDeleted = false,
                },

                new CustomerDTO
                {
                    CustomerId = "C002",
                    CustomerName = "Jane Doe",
                    PhoneNumber = "987654321",
                    Email = "jane.doe@example.com",
                    CCCD = "987654321",
                    DateOfBirth = new DateTime(1985, 5, 10),
                    Gender = "Female",
                    CustomerType = "VIP",
                    CustomerAddress = "456 Oak St",
                    IsDeleted = false
                },
                new CustomerDTO
                {
                    CustomerId = "C003",
                    CustomerName = "Bob Smith",
                    PhoneNumber = "555555555",
                    Email = "bob.smith@example.com",
                    CCCD = "555555555",
                    DateOfBirth = new DateTime(1975, 8, 20),
                    Gender = "Male",
                    CustomerType = "Regular",
                    CustomerAddress = "789 Pine St",
                    IsDeleted = true
                }
            };
            service = new CustomerService(mockEntities.Object);
            var result = await service.GetAllCustomer();
            //result.Reverse();
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(expected[0].CustomerId, result[0].CustomerId);
            Assert.AreEqual(expected[1].CustomerId, result[1].CustomerId);
        }

        [TestMethod()]
        public async Task AddCustomerTestAsync()
        {
            var cus = new CustomerDTO
            {
                CustomerName = "Bob Smith",
                PhoneNumber = "553355555",
                Email = "bob.smith@example.com",
                CCCD = "555554255",
                DateOfBirth = new DateTime(1975, 8, 20),
                Gender = "Male",
                CustomerType = "Regular",
                CustomerAddress = "789 Pine St",
                IsDeleted = false
            };
            var expected = (true, "Thêm khách hàng mới thành công", "KH004");
            service = new CustomerService(mockEntities.Object);
            var result = await service.AddCustomer(cus);
            Assert.AreEqual(result.Item1, expected.Item1);
            Assert.AreEqual(result.Item2, expected.Item2);
            Assert.AreEqual(result.Item3.CustomerId, expected.Item3);

        }

        [TestMethod()]
        public async Task GetCustomerByCCCDTest()
        {
            var expected = "C001";
            service = new CustomerService(mockEntities.Object);
            var result = await service.GetCustomerByCCCD("123456789");
            Assert.AreEqual(result.CustomerId, expected);
        }

        [TestMethod()]
        public async Task UpdateCustomerInfoTest()
        {
            var cus = new CustomerDTO()
            {
                CustomerId = "C001",
                CustomerName = "John Hoas",
                PhoneNumber = "123456789",
                Email = "john.doe@example.com",
                CCCD = "123456789",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = "Male",
                CustomerType = "Regular",
                CustomerAddress = "123 Main St",
                IsDeleted = false,
            };

            var expected = (true, "Chỉnh sửa thông tin khách hàng thành công");
            service = new CustomerService(mockEntities.Object);
            var result = await service.UpdateCustomerInfo(cus);
            Assert.AreEqual(result, expected);
        }

        [TestMethod()]
        public async Task DeleteCustomerTest()
        {
            var expected = (true, "Xóa khách hàng thành công");
            var cus = new CustomerDTO()
            {
                CustomerId = "C001",
                CustomerName = "John Hoas",
                PhoneNumber = "123456789",
                Email = "john.doe@example.com",
                CCCD = "123456789",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = "Male",
                CustomerType = "Regular",
                CustomerAddress = "123 Main St",
                IsDeleted = false,
            };

            service = new CustomerService(mockEntities.Object);
            var result = await service.DeleteCustomer(cus.CustomerId);
            Assert.AreEqual(result, expected);

        }
    }
}