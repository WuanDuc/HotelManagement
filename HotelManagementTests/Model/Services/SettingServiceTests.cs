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
using HotelManagement.ViewModel.StaffVM;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Contexts;

namespace HotelManagement.Model.Services.Tests
{
    [TestClass()]
    public class SettingServiceTests
    {
        SettingService service;

        Mock<HotelManagementEntities> mockEntities;
        Mock<DbSet<Staff>> mockStaff;

        List<Staff> staffDTOs;

        [TestInitialize()]
        public void Setup()
        {
            staffDTOs = new List<Staff>()
            {
                new Staff()
                {
                    StaffId = "NV001",
                                     StaffName = "Nhân viên 1",
                                     //PhoneNumber = "0775897337",
                                     Email = "21520129@gm.uit.edu.vn",
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
                                     Email = "21521665@gm.uit.edu.vn",
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

            mockEntities = new Mock<HotelManagementEntities>();
            mockEntities.Setup(m => m.Staffs).Returns(mockStaff.Object);

            service = new SettingService(mockEntities.Object);
        }


        [TestMethod]
        public async Task EditName_ShouldEditStaffNameSuccessfully()
        {
            service = new SettingService(mockEntities.Object);
            string staffId = "NV001";
            string staffName = "Nhân viên 2";
            // Act
            var result = await service.EditName(staffName, staffId);

            // Assert
            //Assert.IsTrue(result.Item1);
            Assert.AreEqual((true, "Lưu thông tin thành công"), result);

            // Verify that the SaveChangesAsync method was called
            mockEntities.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public async Task EditName_IdAndNameSwap_ReturnFalse()
        {
            service = new SettingService(mockEntities.Object);
            string staffId = "Nhân viên 1";
            string staffName = "NV001";
            // Act
            var result = await service.EditName(staffName, staffId);

            // Assert
            Assert.IsFalse(result.Item1);
            Assert.AreEqual("Lỗi không tìm thấy nhân viên", result.Item2);
        }

        [TestMethod]
        public async Task EditName_NullIdName_ReturnFalse()
        {
            service = new SettingService(mockEntities.Object);
            // Act
            var result = await service.EditName(null, null);

            // Assert
            Assert.IsFalse(result.Item1);
            Assert.AreEqual("Lỗi không tìm thấy nhân viên", result.Item2);
        }

        [TestMethod]
        public async Task EditEmail_ShouldEditStaffNameSuccessfully()
        {
            service = new SettingService(mockEntities.Object);
            string staffId = "NV001";
            string staffEmail = "abc@gmail.com";
            // Act
            var result = await service.EditEmail(staffEmail, staffId);

            // Assert
            //Assert.IsTrue(result.Item1);
            Assert.AreEqual((true, "Lưu thông tin thành công"), result);

            // Verify that the SaveChangesAsync method was called
            mockEntities.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public async Task EditEmail_IdAndNameSwap_ReturnFalse()
        {
            service = new SettingService(mockEntities.Object);
            string staffId = "abc@gmail.com";
            string staffEmail = "NV001";
            // Act
            var result = await service.EditEmail(staffEmail, staffId);

            // Assert
            Assert.IsFalse(result.Item1);
            Assert.AreEqual("lỗi hệ thống", result.Item2);
        }

        [TestMethod]
        public async Task EditEmail_NullIdName_ReturnFalse()
        {
            service = new SettingService(mockEntities.Object);
            // Act
            var result = await service.EditEmail(null, null);

            // Assert
            Assert.IsFalse(result.Item1);
            Assert.AreEqual("lỗi hệ thống", result.Item2);
        }
    }
}