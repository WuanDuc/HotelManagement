using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using HotelManagement.DTOs;
using HotelManagement.ViewModel.StaffVM;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Contexts;
using HotelManagement.Model.Services;
using System.Data.Entity;
using Castle.Core.Resource;

namespace HotelManagement.Model.Services.Tests
{
    [TestClass()]
    public class BillServiceTests
    {
        BillService service;

        Mock<HotelManagementEntities> mockEntities;
        Mock<DbSet<Bill>> mockBill;
        Mock<DbSet<Customer>> mockCustomer;
        Mock<DbSet<RentalContract>> mockRentalContract;

        List<Bill> billDTOs;
        List<Customer> customerDTOs;
        List<RentalContract> rentalContractDTOs;
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
                    CustomerId = "C001",
                    CustomerName = "John Doe",
                    PhoneNumber = "123456789",
                    Email = "john.doe@example.com",
                    CCCD = "123456789",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Gender = "Male",
                    CustomerType = "Regular",
                    CustomerAddress = "123 Main St",
                    IsDeleted = false
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
            rentalContractDTOs = new List<RentalContract>()
            {
                new RentalContract
                {
                    RentalContractId = "RC001",
                    StartDate = new DateTime(2023, 1, 1),
                    StartTime = new TimeSpan(10, 0, 0),
                    CheckOutDate = new DateTime(2023, 1, 5),
                    PersonNumber = 2,
                    StaffId = "S001",
                    CustomerId = "C001",
                    RoomId = "R001",
                    Validated = true
                }
            };
            billDTOs = new List<Bill>()
            {
                new Bill()
                {
                    BillId = "ID001",
                    RentalContractId = "RC001",
                    NumberOfRentalDays = 5,
                    ServicePrice = 20.0,
                    TroublePrice = 10.0,
                    DiscountPrice = 5.0,
                    Price = 50.0,
                    CreateDate = new DateTime(2023,1,1),
                },
                new Bill()
                {
                    BillId = "ID002",
                    RentalContractId = "RC001",
                    NumberOfRentalDays = 5,
                    ServicePrice = 20.0,
                    TroublePrice = 10.0,
                    DiscountPrice = 5.0,
                    Price = 50.0,
                    CreateDate = new DateTime(2023,1,1),
                }
            };
            var dataBill = billDTOs.AsQueryable();

            mockBill = new Mock<DbSet<Bill>>();
            mockBill.As<IQueryable<Bill>>().Setup(m => m.Provider).Returns(dataBill.Provider);
            mockBill.As<IQueryable<Bill>>().Setup(m => m.Expression).Returns(dataBill.Expression);
            mockBill.As<IQueryable<Bill>>().Setup(m => m.ElementType).Returns(dataBill.ElementType);
            mockBill.As<IQueryable<Bill>>().Setup(m => m.GetEnumerator()).Returns(dataBill.GetEnumerator());

            var dataRental = rentalContractDTOs.AsQueryable();

            mockRentalContract = new Mock<DbSet<RentalContract>>();
            mockRentalContract.As<IQueryable<RentalContract>>().Setup(m => m.Provider).Returns(dataRental.Provider);
            mockRentalContract.As<IQueryable<RentalContract>>().Setup(m => m.Expression).Returns(dataRental.Expression);
            mockRentalContract.As<IQueryable<RentalContract>>().Setup(m => m.ElementType).Returns(dataRental.ElementType);
            mockRentalContract.As<IQueryable<RentalContract>>().Setup(m => m.GetEnumerator()).Returns(dataRental.GetEnumerator());
            //// Thiết lập behavior cho phương thức Find của mockStaff
            //mockStaff.Setup(m => m.Find(It.IsAny<object[]>()))
            //    .Returns<object[]>(ids => staffDTOs.FirstOrDefault(s => s.StaffId == ids[0]));

            //// Thiết lập behavior cho phương thức Add của mockStaff
            //mockStaff.Setup(m => m.Add(It.IsAny<Staff>()))
            //    .Callback<Staff>(staff => staffDTOs.Add(staff));

            //mockEntities.Setup(m => m.Staffs.FindAsync(staffId)).ReturnsAsync(new Staff());
            mockEntities = new Mock<HotelManagementEntities>();
            mockEntities.Setup(m => m.Bills).Returns(mockBill.Object);
            mockEntities.Setup(m => m.RentalContracts).Returns(mockRentalContract.Object);

            service = new BillService(mockEntities.Object);
        }

        [TestMethod()]
        public async Task GetBillByListRentalContractTestAsync()
        {
            List<BillDTO> expected = new List<BillDTO>()
            {
                new BillDTO()
                {
                    BillId = "ID001",
                    RentalContractId = "RC001",
                    NumberOfRentalDays = 5,
                    ServicePrice = 20.0,
                    TroublePrice = 10.0,
                    DiscountPrice = 5.0,
                    Price = 50.0,
                    CreateDate = new DateTime(2023,1,1)
                },
                new BillDTO()
                {
                    BillId = "ID002",
                    RentalContractId = "RC001",
                    NumberOfRentalDays = 5,
                    ServicePrice = 20.0,
                    TroublePrice = 10.0,
                    DiscountPrice = 5.0,
                    Price = 50.0,
                    CreateDate = new DateTime(2023,1,1)
                }
            };
            List<RentalContractDTO> rentalContracts = new List<RentalContractDTO>()
            {
                new RentalContractDTO
                {
                    RentalContractId = "RC001",
                    StartDate = new DateTime(2023, 1, 1),
                    StartTime = new TimeSpan(10, 0, 0),
                    CheckOutDate = new DateTime(2023, 1, 5),
                    PersonNumber = 2,
                    StaffId = "S001",
                    CustomerId = "C001",
                    RoomId = "R001",
                    Validated = true
                }
            };
            service = new BillService(mockEntities.Object);
            var result = await service.GetBillByListRentalContract(rentalContracts);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        public async Task GetBillDetailsTestAsync()
        {
            BillDTO expected = new BillDTO()
            {
                BillId = "ID001",
                RentalContractId = "RC001",
                NumberOfRentalDays = 5,
                ServicePrice = 20.0,
                TroublePrice = 10.0,
                DiscountPrice = 5.0,
                Price = 50.0,
                CreateDate = new DateTime(2023, 1, 1)
            };
            service = new BillService(mockEntities.Object);
            var result = await service.GetBillDetails(expected.BillId);

            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        public async Task SaveBillTest()
        {
            (bool, string) expected = (true, "Thanh toán thành công!");
            BillDTO bill = new BillDTO()
            {
                BillId = "ID004",
                RentalContractId = "RC001",
                NumberOfRentalDays = 5,
                ServicePrice = 20.0,
                TroublePrice = 10.0,
                DiscountPrice = 5.0,
                Price = 50.0,
                CreateDate = new DateTime(2023, 1, 1)
            };
            service = new BillService(mockEntities.Object);
            var result = await service.SaveBill(bill);
            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        public async Task GetAllBillTestAsync()
        {
            List<BillDTO> expected = new List<BillDTO>()
            {
                new BillDTO()
                {
                    BillId = "ID001",
                    RentalContractId = "RC001",
                    NumberOfRentalDays = 5,
                    ServicePrice = 20.0,
                    TroublePrice = 10.0,
                    DiscountPrice = 5.0,
                    Price = 50.0,
                    CreateDate = new DateTime(2023,1,1)
                },
                new BillDTO()
                {
                    BillId = "ID002",
                    RentalContractId = "RC001",
                    NumberOfRentalDays = 5,
                    ServicePrice = 20.0,
                    TroublePrice = 10.0,
                    DiscountPrice = 5.0,
                    Price = 50.0,
                    CreateDate = new DateTime(2023,1,1)
                }
            };
            service = new BillService(mockEntities.Object);
            var result = await service.GetAllBill();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        public async Task GetAllBillByDateTestAsync()
        {
            List<BillDTO> expected = new List<BillDTO>()
            {
                new BillDTO()
                {
                    BillId = "ID001",
                    RentalContractId = "RC001",
                    NumberOfRentalDays = 5,
                    ServicePrice = 20.0,
                    TroublePrice = 10.0,
                    DiscountPrice = 5.0,
                    Price = 50.0,
                    CreateDate = new DateTime(2023,1,1)
                },
                new BillDTO()
                {
                    BillId = "ID002",
                    RentalContractId = "RC001",
                    NumberOfRentalDays = 5,
                    ServicePrice = 20.0,
                    TroublePrice = 10.0,
                    DiscountPrice = 5.0,
                    Price = 50.0,
                    CreateDate = new DateTime(2023,1,1)
                }
            };
            service = new BillService(mockEntities.Object);
            var result = await service.GetAllBillByDate(new DateTime(2023,1,1));

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(expected, result);

        }

        [TestMethod()]
        public async Task GetAllBillByMonthTestAsync()
        {
            List<BillDTO> expected = new List<BillDTO>()
            {
                new BillDTO()
                {
                    BillId = "ID001",
                    RentalContractId = "RC001",
                    NumberOfRentalDays = 5,
                    ServicePrice = 20.0,
                    TroublePrice = 10.0,
                    DiscountPrice = 5.0,
                    Price = 50.0,
                    CreateDate = new DateTime(2023,1,1)
                },
                new BillDTO()
                {
                    BillId = "ID002",
                    RentalContractId = "RC001",
                    NumberOfRentalDays = 5,
                    ServicePrice = 20.0,
                    TroublePrice = 10.0,
                    DiscountPrice = 5.0,
                    Price = 50.0,
                    CreateDate = new DateTime(2023,1,1)
                }
            };
            service = new BillService(mockEntities.Object);
            var result = await service.GetAllBillByMonth(1);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(expected, result);
        }
    }
}
