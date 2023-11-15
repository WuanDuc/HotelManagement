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
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.Threading;
using HotelManagementTests.Model;

namespace HotelManagement.Model.Services.Tests
{
    [TestClass()]
    public class BillServiceTests
    {
        BillService service;

        Mock<HotelManagementEntities> mockEntities;
        Mock<DbSet<Staff>> mockStaff;
        Mock<DbSet<Bill>> mockBill;
        Mock<DbSet<Customer>> mockCustomer;
        Mock<DbSet<Room>> mockRoom;
        Mock<DbSet<RentalContract>> mockRentalContract;
        Mock<DbSet<RoomType>> mockRoomType;

        List<RoomType> roomTypes;
        List<Staff> staffDTOs;
        List<Bill> billDTOs;
        List<Customer> customerDTOs;
        List<RentalContract> rentalContractDTOs;
        List<Room> roomDTOs;
        [TestInitialize()]
        public void Setup()
        {
            roomTypes = new List<RoomType>()
            {
                new RoomType()
                {
                    RoomTypeId = "RT001",
                    RoomTypeName = "Room Type 1",
                    Price = 100000,
                    Note = "Note",
                }
            };
            var dataRoomType = roomTypes.AsQueryable();

            mockRoomType = new Mock<DbSet<RoomType>>();
            mockRoomType.As<IDbAsyncEnumerable<RoomType>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<RoomType>(dataRoomType.GetEnumerator()));
            mockRoomType.As<IQueryable<RoomType>>().Setup(m => m.Provider).Returns( new TestDbAsyncQueryProvider<RoomType>(dataRoomType.Provider));
            mockRoomType.As<IQueryable<RoomType>>().Setup(m => m.Expression).Returns(dataRoomType.Expression);
            mockRoomType.As<IQueryable<RoomType>>().Setup(m => m.ElementType).Returns(dataRoomType.ElementType);
            mockRoomType.As<IQueryable<RoomType>>().Setup(m => m.GetEnumerator()).Returns(dataRoomType.GetEnumerator());
            roomDTOs = new List<Room>()
            {
                new Room
                {
                    RoomId = "R001",
                    RoomNumber = 101,
                    RoomTypeId = "RT001", // Assuming you have a RoomType with this ID
                    Note = "This is a standard single room.",
                    RoomStatus = "Có sẵn",
                    RoomCleaningStatus = "Đã dọn dẹp",
                    RoomType = roomTypes[0]
                }
            };
            var dataRoom = roomDTOs.AsQueryable();

            mockRoom = new Mock<DbSet<Room>>();
            mockRoom.As<IDbAsyncEnumerable<Room>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Room>(dataRoom.GetEnumerator()));
            mockRoom.As<IQueryable<Room>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<Room>(dataRoom.Provider));
            mockRoom.As<IQueryable<Room>>().Setup(m => m.Expression).Returns(dataRoom.Expression);
            mockRoom.As<IQueryable<Room>>().Setup(m => m.ElementType).Returns(dataRoom.ElementType);
            mockRoom.As<IQueryable<Room>>().Setup(m => m.GetEnumerator()).Returns(dataRoom.GetEnumerator());

            staffDTOs = new List<Staff> {
                new Staff
                {
                    StaffId = "S001",
                    StaffName = "Alice Johnson",
                    PhoneNumber = "555-1234",
                    StaffAddress = "456 Oak St",
                    Email = "alice.johnson@example.com",
                    CCCD = "987654321",
                    DateOfBirth = new DateTime(1985, 5, 10),
                    Gender = "Female",
                    Position = "Receptionist",
                    Username = "alice123",
                    Password = "password123", // Note: Avoid hardcoding passwords in production
                    Avatar = null, // You can set the staff's avatar image here if needed
                    dateOfStart = new DateTime(2022, 1, 1),
                    IsDeleted = false
                }
            };
            var dataStaff = staffDTOs.AsQueryable();

            mockStaff = new Mock<DbSet<Staff>>();
            mockStaff.As<IDbAsyncEnumerable<Staff>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Staff>(dataStaff.GetEnumerator()));
            mockStaff.As<IQueryable<Staff>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<Staff>(dataStaff.Provider));
            mockStaff.As<IQueryable<Staff>>().Setup(m => m.Expression).Returns(dataStaff.Expression);
            mockStaff.As<IQueryable<Staff>>().Setup(m => m.ElementType).Returns(dataStaff.ElementType);
            mockStaff.As<IQueryable<Staff>>().Setup(m => m.GetEnumerator()).Returns(dataStaff.GetEnumerator());

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
                    Validated = true,
                    Customer = customerDTOs[0],
                    Staff = staffDTOs[0],
                    Room = roomDTOs[0]
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
                    RentalContract = rentalContractDTOs[0],
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
                    RentalContract = rentalContractDTOs[0],
                }
            };
            rentalContractDTOs[0].Bills = billDTOs;
            var dataBill = billDTOs.AsQueryable();

            mockBill = new Mock<DbSet<Bill>>();
            mockBill.As<IDbAsyncEnumerable<Bill>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Bill>(dataBill.GetEnumerator()));
            mockBill.As<IQueryable<Bill>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<Bill>(dataBill.Provider));
            mockBill.As<IQueryable<Bill>>().Setup(m => m.Expression).Returns(dataBill.Expression);
            mockBill.As<IQueryable<Bill>>().Setup(m => m.ElementType).Returns(dataBill.ElementType);
            mockBill.As<IQueryable<Bill>>().Setup(m => m.GetEnumerator()).Returns(dataBill.GetEnumerator());


            var dataRental = rentalContractDTOs.AsQueryable();

            mockRentalContract = new Mock<DbSet<RentalContract>>();
            mockRentalContract.As<IDbAsyncEnumerable<RentalContract>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<RentalContract>(dataRental.GetEnumerator()));
            mockRentalContract.As<IQueryable<RentalContract>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<RentalContract>(dataRental.Provider));
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
            mockEntities.Setup(m => m.RoomTypes).Returns(mockRoomType.Object);
            mockEntities.Setup(m => m.Rooms).Returns(mockRoom.Object);
            mockEntities.Setup(m => m.Customers).Returns(mockCustomer.Object);
            mockEntities.Setup(m => m.Staffs).Returns(mockStaff.Object);
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
