using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelManagement.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using HotelManagement.Model;
using System.Linq;
using System.Data.Entity;
using HotelManagement.DTOs;
using HotelManagement.Utilities;
using HotelManagement.Utils;
using System.Windows.Controls;

namespace HotelManagement.Model.Services.Tests
{
    [TestClass()]
    public class TroubleServiceTests
    {
        TroubleService service;

        Mock<HotelManagementEntities> mockEntities;
        Mock<DbSet<Trouble>> mockTrouble;
        
        List<Trouble> troubles;
        private List<Staff> staffDTOs;
        private Mock<DbSet<Staff>> mockStaff;

        [TestInitialize()]
        public void Setup()
        {
            troubles = new List<Trouble>()
            {
                new Trouble()
                {
                    TroubleId = "TB001",
                    Title = "Trouble 1",
                    Description = "Decription 1",
                    Reason = "Reason 1",
                    Level = LEVEL.NORMAL,
                }
            };
            var dataTrouble = troubles.AsQueryable();

            List<Room> rooms = new List<Room>()
            {
                new Room()
                {
                    RoomId = "Room001",
                    RoomStatus = ROOM_STATUS.BOOKED,
                },
                new Room()
                {
                    RoomId = "Room002",
                    RoomStatus = ROOM_STATUS.RENTING,
                },
                new Room()
                {
                    RoomId = "Room003",
                    RoomStatus = ROOM_STATUS.RENTING,
                }
            };
            var dataR = rooms.AsQueryable();

            var mockR = new Mock<DbSet<Room>>();
            mockR.As<IQueryable<Room>>().Setup(m => m.Provider).Returns(dataR.Provider);
            mockR.As<IQueryable<Room>>().Setup(m => m.Expression).Returns(dataR.Expression);
            mockR.As<IQueryable<Room>>().Setup(m => m.ElementType).Returns(dataR.ElementType);
            mockR.As<IQueryable<Room>>().Setup(m => m.GetEnumerator()).Returns(dataR.GetEnumerator());

            List<RentalContract> rentalContract = new List<RentalContract>()
            {
                new RentalContract()
                {
                    RentalContractId = "RC001",
                    CustomerId = "C001",
                    Validated = true,
                    Room = rooms.First(),
                },
                new RentalContract()
                {
                    RentalContractId = "RC002",
                    CustomerId = "C001",
                    Validated = true,
                    Room = rooms.ElementAt(1),
                }
            };

            var dataRental = rentalContract.AsQueryable();

            List<TroubleByCustomer> troubleByCustomers = new List<TroubleByCustomer>()
            {
                new TroubleByCustomer()
                {
                    TroubleId = "TB001",
                    RentalContractId = "RC001",
                    TroubleByCustomerId = 1,
                    PredictedPrice = 200000,
                    RentalContract = rentalContract.First(),
                    Trouble = troubles.First(),
                }
            };
            var dataTC = troubleByCustomers.AsQueryable();

            var mockTC = new Mock<DbSet<TroubleByCustomer>>();
            mockTC.As<IQueryable<TroubleByCustomer>>().Setup(m => m.Provider).Returns(dataTC.Provider);
            mockTC.As<IQueryable<TroubleByCustomer>>().Setup(m => m.Expression).Returns(dataTC.Expression);
            mockTC.As<IQueryable<TroubleByCustomer>>().Setup(m => m.ElementType).Returns(dataTC.ElementType);
            mockTC.As<IQueryable<TroubleByCustomer>>().Setup(m => m.GetEnumerator()).Returns(dataTC.GetEnumerator());


            var mockRental = new Mock<DbSet<RentalContract>>();
            mockRental.As<IQueryable<RentalContract>>().Setup(m => m.Provider).Returns(dataRental.Provider);
            mockRental.As<IQueryable<RentalContract>>().Setup(m => m.Expression).Returns(dataRental.Expression);
            mockRental.As<IQueryable<RentalContract>>().Setup(m => m.ElementType).Returns(dataRental.ElementType);
            mockRental.As<IQueryable<RentalContract>>().Setup(m => m.GetEnumerator()).Returns(dataRental.GetEnumerator());





            mockTrouble = new Mock<DbSet<Trouble>>();
            mockTrouble.As<IQueryable<Trouble>>().Setup(mockTrouble=>mockTrouble.Provider).Returns(dataTrouble.Provider);
            mockTrouble.As<IQueryable<Trouble>>().Setup(mockTrouble => mockTrouble.Expression).Returns(dataTrouble.Expression);
            mockTrouble.As<IQueryable<Trouble>>().Setup(mockTrouble => mockTrouble.ElementType).Returns(dataTrouble.ElementType);
            mockTrouble.As<IQueryable<Trouble>>().Setup(mockTrouble => mockTrouble.GetEnumerator()).Returns(dataTrouble.GetEnumerator());

            mockEntities = new Mock<HotelManagementEntities>();
            mockEntities.Setup(mockEntities => mockEntities.Troubles).Returns(mockTrouble.Object);
            mockEntities.Setup(mockEntities => mockEntities.TroubleByCustomers).Returns(mockTC.Object);
            mockEntities.Setup(mockEntities => mockEntities.RentalContracts).Returns(mockRental.Object);
            mockEntities.Setup(mockEntities => mockEntities.Rooms).Returns(mockR.Object);

            service = new TroubleService(mockEntities.Object);
            
        }

        [TestMethod()]
        public async Task GetAllTrouble_ShouldReturnCorrectData()
        {
            service = new TroubleService(mockEntities.Object);
            var result = await service.GetAllTrouble();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Trouble 1", result[0].Title);
            Assert.AreEqual("Decription 1", result[0].Description);
            Assert.AreEqual("Reason 1", result[0].Reason);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetAllTrouble_NullData_ThrowException()
        {
            troubles = null;
            var dataTrouble = troubles.AsQueryable();

            mockTrouble = new Mock<DbSet<Trouble>>();
            mockTrouble.As<IQueryable<Trouble>>().Setup(mockTrouble => mockTrouble.Provider).Returns(dataTrouble.Provider);
            mockTrouble.As<IQueryable<Trouble>>().Setup(mockTrouble => mockTrouble.Expression).Returns(dataTrouble.Expression);
            mockTrouble.As<IQueryable<Trouble>>().Setup(mockTrouble => mockTrouble.ElementType).Returns(dataTrouble.ElementType);
            mockTrouble.As<IQueryable<Trouble>>().Setup(mockTrouble => mockTrouble.GetEnumerator()).Returns(dataTrouble.GetEnumerator());

            service = new TroubleService(mockEntities.Object);
            var result = await service.GetAllTrouble();
        }

        [TestMethod()]
        public async Task AddTroubleTest_CorrectTestCase()
        {
            TroubleDTO troubles =
                new TroubleDTO()
                {
                    Title = "Trouble 1",
                    Description = "Decription 1",
                    Reason = "Reason 1",
                    Avatar = new byte[] { },
                };
            
            service = new TroubleService(mockEntities.Object);
            var result = await service.AddTrouble(troubles, null);
            Assert.AreEqual((true, "Báo cáo sự cố mới thành công", troubles), result);
            mockEntities.Verify(m => m.SaveChangesAsync(), Times.Once);
        }

        [TestMethod()]
        public async Task AddTroubleTest_NullData()
        {
            TroubleDTO troubles = null;

            service = new TroubleService(mockEntities.Object);
            var result = await service.AddTrouble(troubles, null);
            Assert.AreEqual((false, "Lỗi hệ thống", null), result);
        }

        [TestMethod()]
        public async Task UpdateTroubleTest_PreditStatus()
        {
            TroubleDTO troubles =
                new TroubleDTO()
                {
                    Title = "Trouble 1",
                    Description = "Decription 1",
                    Reason = "Reason 1",
                    Avatar = new byte[] { },
                    Status = STATUS.PREDIT,
                };

            service = new TroubleService(mockEntities.Object);
            var result = await service.UpdateTrouble(troubles, 200);
            Assert.AreEqual((true, "Cập nhật sự cố thành công"), result);
            mockEntities.Verify(m => m.SaveChangesAsync(), Times.Once);
        }

        [TestMethod()]
        public async Task UpdateTroubleTest_NullStatus_ReturnFalse()
        {
            TroubleDTO troubles =
                new TroubleDTO()
                {
                    Title = "Trouble 1",
                    Description = "Decription 1",
                    Reason = "Reason 1",
                    Avatar = new byte[] { },
                    Status = null,
                };

            service = new TroubleService(mockEntities.Object);
            var result = await service.UpdateTrouble(troubles, 200);
            Assert.AreEqual((false, "Lỗi hệ thống"), result);
        }

        [TestMethod()]
        public async Task UpdateTroubleTest_NullData_ReturnFalse()
        {
            TroubleDTO troubles = null;

            service = new TroubleService(mockEntities.Object);
            var result = await service.UpdateTrouble(troubles, 200);
            Assert.AreEqual((false, "Lỗi hệ thống"), result);
        }

        [TestMethod()]
        public async Task UpdateTroubleTest_NoPrePrice_ReturnTrue()
        {
            TroubleDTO troubles =
                new TroubleDTO()
                {
                    Title = "Trouble 1",
                    Description = "Decription 1",
                    Reason = "Reason 1",
                    Avatar = new byte[] { },
                    Status = STATUS.PREDIT,
                };

            service = new TroubleService(mockEntities.Object);
            var result = await service.UpdateTrouble(troubles);
            Assert.AreEqual((true, "Cập nhật sự cố thành công"), result);
            mockEntities.Verify(m => m.SaveChangesAsync(), Times.Once);
        }

        [TestMethod()]
        public async Task EditTroubleTest_CorrectTest()
        {
            TroubleDTO troubles =
               new TroubleDTO()
               {
                   Title = "Trouble 1",
                   Description = "Decription 1",
                   Reason = "Reason 1",
                   Avatar = new byte[] { },
                   Status = STATUS.PREDIT,
                   StartDate = new DateTime(),
                   Level = LEVEL.CRITICAL,
                   StaffId = "NV001",

               };

            service = new TroubleService(mockEntities.Object);
            var result = await service.EditTrouble(troubles, null);
            Assert.AreEqual((true, "Chỉnh sửa sự cố thành công"), result);
            mockEntities.Verify(m => m.SaveChanges(), Times.Once);
        }


        [TestMethod()]
        public async Task EditTroubleTest_NullData_ReturnFalse()
        {
            TroubleDTO troubles = null;

            service = new TroubleService(mockEntities.Object);
            var result = await service.EditTrouble(troubles, null);
            Assert.AreEqual((false, "Lỗi hệ thống"), result);
        }

        [TestMethod()]
        public async Task GetStaffNameByIdTest()
        {
            staffDTOs = new List<Staff>()
            {
                new Staff()
                {
                    StaffId = "NV001",
                                     StaffName = "Nhân viên 1",
                                     //PhoneNumber = "0775897337",
                                     //Email = "21520129@gm.uit.edu.vn",
                                     //CCCD = "111111111111",
                                     ////DateOfBirth = new DateTime(),
                                     ////dateOfStart = s.dateOfStart,
                                     //StaffAddress = "KTX DHQG",
                                     //Gender = "Nam",
                                     //Position = "Nhân viên",
                                     //Username = "nhanvien1",
                                     //Password = "password1",
                                     //Avatar = s.Avatar,
                },
                new Staff()
                {
                    StaffId = "NV002",
                                     StaffName = "Nhân viên 2",
                                     //PhoneNumber = "0775897221",
                                     //Email = "21521665@gm.uit.edu.vn",
                                     //CCCD = "111111111117",
                                     ////DateOfBirth = new DateTime(),
                                     ////dateOfStart = s.dateOfStart,
                                     //StaffAddress = "KTX DHQG",
                                     //Gender = "Nam",
                                     //Position = "Nhân viên",
                                     //Username = "nhanvien2",
                                     //Password = "password2",
                                     //Avatar = s.Avatar,
                }
            };
            var dataStaff = staffDTOs.AsQueryable();

            mockStaff = new Mock<DbSet<Staff>>();
            mockStaff.As<IQueryable<Staff>>().Setup(m => m.Provider).Returns(dataStaff.Provider);
            mockStaff.As<IQueryable<Staff>>().Setup(m => m.Expression).Returns(dataStaff.Expression);
            mockStaff.As<IQueryable<Staff>>().Setup(m => m.ElementType).Returns(dataStaff.ElementType);
            mockStaff.As<IQueryable<Staff>>().Setup(m => m.GetEnumerator()).Returns(dataStaff.GetEnumerator());

            mockEntities.Setup(m => m.Staffs).Returns(mockStaff.Object);

            string staffID = "NV001";
            string name = "Nhân viên 1";
            service = new TroubleService(mockEntities.Object);
            var result = await service.GetStaffNameById(staffID);
            Assert.AreEqual(name, result);
        }

        [TestMethod()]
        public async Task GetStaffNameByIdTest_NullID()
        {
            staffDTOs = new List<Staff>()
            {
                new Staff()
                {
                    StaffId = "NV001",
                                     StaffName = "Nhân viên 1",
                                     //PhoneNumber = "0775897337",
                                     //Email = "21520129@gm.uit.edu.vn",
                                     //CCCD = "111111111111",
                                     ////DateOfBirth = new DateTime(),
                                     ////dateOfStart = s.dateOfStart,
                                     //StaffAddress = "KTX DHQG",
                                     //Gender = "Nam",
                                     //Position = "Nhân viên",
                                     //Username = "nhanvien1",
                                     //Password = "password1",
                                     //Avatar = s.Avatar,
                },
                new Staff()
                {
                    StaffId = "NV002",
                                     StaffName = "Nhân viên 2",
                                     //PhoneNumber = "0775897221",
                                     //Email = "21521665@gm.uit.edu.vn",
                                     //CCCD = "111111111117",
                                     ////DateOfBirth = new DateTime(),
                                     ////dateOfStart = s.dateOfStart,
                                     //StaffAddress = "KTX DHQG",
                                     //Gender = "Nam",
                                     //Position = "Nhân viên",
                                     //Username = "nhanvien2",
                                     //Password = "password2",
                                     //Avatar = s.Avatar,
                }
            };
            var dataStaff = staffDTOs.AsQueryable();

            mockStaff = new Mock<DbSet<Staff>>();
            mockStaff.As<IQueryable<Staff>>().Setup(m => m.Provider).Returns(dataStaff.Provider);
            mockStaff.As<IQueryable<Staff>>().Setup(m => m.Expression).Returns(dataStaff.Expression);
            mockStaff.As<IQueryable<Staff>>().Setup(m => m.ElementType).Returns(dataStaff.ElementType);
            mockStaff.As<IQueryable<Staff>>().Setup(m => m.GetEnumerator()).Returns(dataStaff.GetEnumerator());

            mockEntities.Setup(m => m.Staffs).Returns(mockStaff.Object);
            string staffID = null;
            string name = "";
            service = new TroubleService(mockEntities.Object);
            var result = await service.GetStaffNameById(staffID);
            Assert.AreEqual(name, "");
        }

        [TestMethod()]
        public async Task GetTroubleByCusTest_CorrectTestAsync()
        {
            service = new TroubleService(mockEntities.Object);
            string rentalContractId = "RC001";
            List<RentalContract> rentalContract = new List<RentalContract>()
            {
                new RentalContract()
                {
                    RentalContractId = "RC001",
                    CustomerId = "C001",
                },
                new RentalContract()
                {
                    RentalContractId = "RC002",
                    CustomerId = "C001",
                }
            };
            List<TroubleByCustomerDTO> expected = new List<TroubleByCustomerDTO>()
            {
                new TroubleByCustomerDTO()
                {
                    TroubleId = "TB001",
                    RentalContractId = "RC001",
                    PredictedPrice = 200000,
                }
            };
            List<TroubleByCustomerDTO> actual = await service.GetListTroubleByCustomer(rentalContractId);

            Assert.AreEqual(expected.First().TroubleId, actual.First().TroubleId);
            Assert.AreEqual(expected.First().RentalContractId, actual.First().RentalContractId);
            Assert.AreEqual(expected.First().PredictedPriceStr, actual.First().PredictedPriceStr);
        }

        [TestMethod()]
        public async Task GetCurrentListRentalContractId()
        {
            service = new TroubleService(mockEntities.Object);

            List<string> expected = new List<string>()
            {
                "RC002",
            };

            List<string> actual = await service.GetCurrentListRentalContractId();
            Assert.AreEqual(expected.First(), actual.First());
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetTroubleByCusTest_ThrowException()
        {
            List<TroubleByCustomer> troubleByCustomers = null;
            var dataTC = troubleByCustomers.AsQueryable();

            var mockTC = new Mock<DbSet<TroubleByCustomer>>();
            mockTC.As<IQueryable<TroubleByCustomer>>().Setup(m => m.Provider).Returns(dataTC.Provider);
            mockTC.As<IQueryable<TroubleByCustomer>>().Setup(m => m.Expression).Returns(dataTC.Expression);
            mockTC.As<IQueryable<TroubleByCustomer>>().Setup(m => m.ElementType).Returns(dataTC.ElementType);
            mockTC.As<IQueryable<TroubleByCustomer>>().Setup(m => m.GetEnumerator()).Returns(dataTC.GetEnumerator());

            mockEntities.Setup(m => m.TroubleByCustomers).Returns(mockTC.Object);

            service = new TroubleService(mockEntities.Object);

            List<TroubleByCustomerDTO> actual = await service.GetListTroubleByCustomer("RC001");
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetTroubleByCusTest_NoTroubleByCustomer_ReturnEmpty()
        {
            var dataTrouble = troubles.AsQueryable();

            List<Room> rooms = new List<Room>()
            {
                new Room()
                {
                    RoomId = "Room001",
                    RoomStatus = ROOM_STATUS.BOOKED,
                },
                new Room()
                {
                    RoomId = "Room002",
                    RoomStatus = ROOM_STATUS.RENTING,
                },
                new Room()
                {
                    RoomId = "Room003",
                    RoomStatus = ROOM_STATUS.RENTING,
                }
            };
            var dataR = rooms.AsQueryable();

            var mockR = new Mock<DbSet<Room>>();
            mockR.As<IQueryable<Room>>().Setup(m => m.Provider).Returns(dataR.Provider);
            mockR.As<IQueryable<Room>>().Setup(m => m.Expression).Returns(dataR.Expression);
            mockR.As<IQueryable<Room>>().Setup(m => m.ElementType).Returns(dataR.ElementType);

            List<RentalContract> rentalContract = new List<RentalContract>()
            {
                new RentalContract()
                {
                    RentalContractId = "RC001",
                    CustomerId = "C001",
                    Validated = true,
                    Room = rooms.First(),
                },
                new RentalContract()
                {
                    RentalContractId = "RC002",
                    CustomerId = "C001",
                    Validated = true,
                    Room = rooms.ElementAt(1),
                }
            };

            var dataRental = rentalContract.AsQueryable();

            List<TroubleByCustomer> troubleByCustomers = new List<TroubleByCustomer>()
            {
                new TroubleByCustomer()
                {
                    TroubleId = "TB001",
                    RentalContractId = "RC002",
                    TroubleByCustomerId = 1,
                    PredictedPrice = 200000,
                    RentalContract = rentalContract.ElementAt(1),
                    Trouble = troubles.First(),
                }
            };
            var dataTC = troubleByCustomers.AsQueryable();

            var mockTC = new Mock<DbSet<TroubleByCustomer>>();
            mockTC.As<IQueryable<TroubleByCustomer>>().Setup(m => m.Provider).Returns(dataTC.Provider);
            mockTC.As<IQueryable<TroubleByCustomer>>().Setup(m => m.Expression).Returns(dataTC.Expression);
            mockTC.As<IQueryable<TroubleByCustomer>>().Setup(m => m.ElementType).Returns(dataTC.ElementType);
            mockTC.As<IQueryable<TroubleByCustomer>>().Setup(m => m.GetEnumerator()).Returns(dataTC.GetEnumerator());

            var mockRental = new Mock<DbSet<RentalContract>>();
            mockRental.As<IQueryable<RentalContract>>().Setup(m => m.Provider).Returns(dataRental.Provider);
            mockRental.As<IQueryable<RentalContract>>().Setup(m => m.Expression).Returns(dataRental.Expression);
            mockRental.As<IQueryable<RentalContract>>().Setup(m => m.ElementType).Returns(dataRental.ElementType);
            mockRental.As<IQueryable<RentalContract>>().Setup(m => m.GetEnumerator()).Returns(dataRental.GetEnumerator());

            Mock<HotelManagementEntities> entities = new Mock<HotelManagementEntities>();
            entities.Setup(mockEntities => mockEntities.TroubleByCustomers).Returns(mockTC.Object);
            entities.Setup(mockEntities => mockEntities.RentalContracts).Returns(mockRental.Object);
            entities.Setup(mockEntities => mockEntities.Rooms).Returns(mockR.Object);
            service = new TroubleService(mockEntities.Object);

            List<TroubleByCustomerDTO> actual = await service.GetListTroubleByCustomer("RC003");
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetCurrentListRentalContractIdTest_NullRentalConTract_ThrowNullException()
        {
            List<RentalContract> rentalContract = null;

            var dataRental = rentalContract.AsQueryable();
            var mockRental = new Mock<DbSet<RentalContract>>();
            mockRental.As<IQueryable<RentalContract>>().Setup(m => m.Provider).Returns(dataRental.Provider);
            mockRental.As<IQueryable<RentalContract>>().Setup(m => m.Expression).Returns(dataRental.Expression);
            mockRental.As<IQueryable<RentalContract>>().Setup(m => m.ElementType).Returns(dataRental.ElementType);
            mockRental.As<IQueryable<RentalContract>>().Setup(m => m.GetEnumerator()).Returns(dataRental.GetEnumerator());

            Mock<HotelManagementEntities> nullContext = new Mock<HotelManagementEntities>();
            mockEntities = new Mock<HotelManagementEntities>();
            mockEntities.Setup(mockEntities => mockEntities.RentalContracts).Returns(mockRental.Object);

            service = new TroubleService(mockEntities.Object);

            List<string> actual = await service.GetCurrentListRentalContractId();
        }

        [TestMethod()]
        public async Task GetCurrentListRentalContractIdTest_NoRentingRoom_ReturnEmpty()
        {
            List<Room> rooms = new List<Room>()
            {
                new Room()
                {
                    RoomId = "Room001",
                    RoomStatus = ROOM_STATUS.BOOKED,
                },
                new Room()
                {
                    RoomId = "Room002",
                    RoomStatus = ROOM_STATUS.READY,
                },
                new Room()
                {
                    RoomId = "Room003",
                    RoomStatus = ROOM_STATUS.READY,
                }
            };
            var dataR = rooms.AsQueryable();

            List<RentalContract> rentalContract = new List<RentalContract>()
            {
                new RentalContract()
                {
                    RentalContractId = "RC001",
                    CustomerId = "C001",
                    Validated = true,
                    Room = rooms.First(),
                },
                new RentalContract()
                {
                    RentalContractId = "RC002",
                    CustomerId = "C001",
                    Validated = true,
                    Room = rooms.ElementAt(1),
                }
            };

            var dataRental = rentalContract.AsQueryable();
            var mockRental = new Mock<DbSet<RentalContract>>();
            mockRental.As<IQueryable<RentalContract>>().Setup(m => m.Provider).Returns(dataRental.Provider);
            mockRental.As<IQueryable<RentalContract>>().Setup(m => m.Expression).Returns(dataRental.Expression);
            mockRental.As<IQueryable<RentalContract>>().Setup(m => m.ElementType).Returns(dataRental.ElementType);
            mockRental.As<IQueryable<RentalContract>>().Setup(m => m.GetEnumerator()).Returns(dataRental.GetEnumerator());

            var mockR = new Mock<DbSet<Room>>();
            mockR.As<IQueryable<Room>>().Setup(m => m.Provider).Returns(dataR.Provider);
            mockR.As<IQueryable<Room>>().Setup(m => m.Expression).Returns(dataR.Expression);
            mockR.As<IQueryable<Room>>().Setup(m => m.ElementType).Returns(dataR.ElementType);
            mockR.As<IQueryable<Room>>().Setup(m => m.GetEnumerator()).Returns(dataR.GetEnumerator());

            mockEntities.Setup(mockEntities => mockEntities.Rooms).Returns(mockR.Object);
            mockEntities.Setup(mockEntities => mockEntities.RentalContracts).Returns(mockRental.Object);

            service = new TroubleService(mockEntities.Object);

            List<string> actual = await service.GetCurrentListRentalContractId();
            Assert.AreEqual(0, actual.Count);
        }
    }
}