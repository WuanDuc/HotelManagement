using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelManagement.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using System.Data.Entity;
using HotelManagement.DTOs;
using Microsoft.Office.Interop.Excel;

namespace HotelManagement.Model.Services.Tests
{
    [TestClass()]
    public class StaffServiceTests
    {
        StaffService service;
        Mock<HotelManagementEntities> mockEntities;
        Mock<DbSet<Staff>> mockStaff;

        List<Staff> mockStaffList;
        List<StaffDTO> mockStaffDTOList;

        [TestInitialize()]
        public void Setup()
        {
            mockStaffList = new List<Staff>()
            {
                new Staff()
                {
                    StaffId = "NV001",
                    StaffName = "Nhân viên 1",
                    Email = "21520129@gm.uit.edu.vn",
                    StaffAddress = "Địa chỉ nhân viên 1",
                    PhoneNumber = "0777777777",
                    CCCD = "1111111111",
                    DateOfBirth = new DateTime(),
                    dateOfStart = new DateTime(),
                    Gender = "Nam",
                    Position = "Quản lý",
                    Username = "nhanvien1",
                    Password = "password1",
                    Avatar = new byte[] {},
                    IsDeleted = false,
                }
            };
            var data = mockStaffList.AsQueryable();

            mockStaff = new Mock<DbSet<Staff>>();
            mockStaff.As<IQueryable<Staff>>().Setup(m => m.Provider).Returns(data.Provider);
            mockStaff.As<IQueryable<Staff>>().Setup(m => m.Expression).Returns(data.Expression);
            mockStaff.As<IQueryable<Staff>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockStaff.As<IQueryable<Staff>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockEntities = new Mock<HotelManagementEntities>();
            mockEntities.Setup(m => m.Staffs).Returns(mockStaff.Object);

            service = new StaffService(mockEntities.Object);
        }
        [TestMethod()]
        public async Task GetAllStaffTest_ReturnCorrectData()
        {
            List<StaffDTO> expected = new List<StaffDTO>()
            {
                new StaffDTO()
                {
                    StaffId = "NV001",
                    StaffName = "Nhân viên 1",
                    Email = "21520129@gm.uit.edu.vn",
                    StaffAddress = "Địa chỉ nhân viên 1",
                    PhoneNumber = "0777777777",
                    CCCD = "1111111111",
                    DateOfBirth = new DateTime(),
                    dateOfStart = new DateTime(),
                    Gender = "Nam",
                    Position = "Quản lý",
                    Username = "nhanvien1",
                    Password = "password1",
                    Avatar = new byte[] {},
                }
            };

            service = new StaffService(mockEntities.Object);

            List<StaffDTO> actual = await service.GetAllStaff();

            Assert.AreEqual(1, actual.Count);
            Assert.IsNotNull(actual[0]);
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetAllStaffTest_NullStaff_ThrowException()
        {
            mockStaffList = null;
            var data = mockStaffList.AsQueryable();

            mockStaff = new Mock<DbSet<Staff>>();
            mockStaff.As<IQueryable<Staff>>().Setup(m => m.Provider).Returns(data.Provider);
            mockStaff.As<IQueryable<Staff>>().Setup(m => m.Expression).Returns(data.Expression);
            mockStaff.As<IQueryable<Staff>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockStaff.As<IQueryable<Staff>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockEntities = new Mock<HotelManagementEntities>();
            mockEntities.Setup(m => m.Staffs).Returns(mockStaff.Object);

            service = new StaffService(mockEntities.Object);

            List<StaffDTO> actual = await service.GetAllStaff();
        }

        [TestMethod()]
        public async Task GetAllStaffTest_EmptyStaff_ReturnEmpty()
        {
            mockStaffList = new List<Staff>() { };
            var data = mockStaffList.AsQueryable();

            mockStaff = new Mock<DbSet<Staff>>();
            mockStaff.As<IQueryable<Staff>>().Setup(m => m.Provider).Returns(data.Provider);
            mockStaff.As<IQueryable<Staff>>().Setup(m => m.Expression).Returns(data.Expression);
            mockStaff.As<IQueryable<Staff>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockStaff.As<IQueryable<Staff>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockEntities = new Mock<HotelManagementEntities>();
            mockEntities.Setup(m => m.Staffs).Returns(mockStaff.Object);

            service = new StaffService(mockEntities.Object);

            List<StaffDTO> actual = await service.GetAllStaff();

            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod()]
        public async Task AddStaffTest_CorrectData()
        {

        }

        [TestMethod()]
        public void UpdateStaffInfoTest()
        {

        }

        [TestMethod()]
        public void CheckStaffTest()
        {

        }

        [TestMethod()]
        public void DeleteStaffTest()
        {

        }

        [TestMethod()]
        public void UpdatePasswordTest()
        {

        }

        [TestMethod()]
        public void CheckLoginTest()
        {

        }
    }
}