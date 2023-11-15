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
                    Title = "Trouble 1",
                    Description = "Decription 1",
                    Reason = "Reason 1"
                }
            };
            var dataTrouble = troubles.AsQueryable();

            mockTrouble = new Mock<DbSet<Trouble>>();
            mockTrouble.As<IQueryable<Trouble>>().Setup(mockTrouble=>mockTrouble.Provider).Returns(dataTrouble.Provider);
            mockTrouble.As<IQueryable<Trouble>>().Setup(mockTrouble => mockTrouble.Expression).Returns(dataTrouble.Expression);
            mockTrouble.As<IQueryable<Trouble>>().Setup(mockTrouble => mockTrouble.ElementType).Returns(dataTrouble.ElementType);
            mockTrouble.As<IQueryable<Trouble>>().Setup(mockTrouble => mockTrouble.GetEnumerator()).Returns(dataTrouble.GetEnumerator());

            mockEntities = new Mock<HotelManagementEntities>();
            mockEntities.Setup(mockEntities => mockEntities.Troubles).Returns(mockTrouble.Object);

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
            mockEntities.Verify(m => m.SaveChangesAsync(), Times.Once);
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
        public void GetTroubleByCusTest()
        {

        }

        [TestMethod()]
        public void GetListTroubleByCustomerTest()
        {

        }

        [TestMethod()]
        public void GetCurrentListRentalContractIdTest()
        {

        }
    }
}