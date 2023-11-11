using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelManagement.ViewModel.AdminVM.StaffManagementVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Utils;
using System.Threading;

namespace HotelManagement.ViewModel.AdminVM.StaffManagementVM.Tests
{
    [TestClass()]
    public class StaffManagementVMTests
    {
        [TestMethod()]
        public void IsValidAgeTest()
        {
            // Arrange
            var staffManagementVM = new StaffManagementVM();
            var eighteenYearsAgo = DateTime.Today.AddYears(-18);

            // Act
            var result = staffManagementVM.IsValidAge(eighteenYearsAgo);

            // Assert
            Assert.IsTrue(result.isv);
            Assert.IsNull(result.err);
        }
        [TestMethod()]
        public void IsValidAge_AgeExactly18_ReturnsTrue()
        {
            // Arrange
            var staffManagementVM = new StaffManagementVM();
            var exactlyEighteenYearsAgo = DateTime.Today.AddYears(-18).AddDays(-1);

            // Act
            var result = staffManagementVM.IsValidAge(exactlyEighteenYearsAgo);

            // Assert
            Assert.IsTrue(result.isv);
            Assert.IsNull(result.err);
        }

        [TestMethod()]
        public void IsValidAge_AgeUnder18_ReturnsFalse()
        {
            // Arrange
            var staffManagementVM = new StaffManagementVM();
            var underEighteenYearsAgo = DateTime.Today.AddYears(-17);

            // Act
            var result = staffManagementVM.IsValidAge(underEighteenYearsAgo);

            // Assert
            Assert.IsFalse(result.isv);
            Assert.AreEqual("Nhân viên chưa đủ 18 tuổi!", result.err);
        }

        [TestMethod()]
        public void ValidAge_AgeUnder18_LeapYear_ReturnsFalse()
        {
            // Arrange
            var staffManagementVM = new StaffManagementVM();
            var underEighteenYearsAgoLeapYear = DateTime.Today.AddYears(-17).AddDays(-1).AddYears(-1); // Leap year

            // Act
            var result = staffManagementVM.IsValidAge(underEighteenYearsAgoLeapYear);

            // Assert
            Assert.IsFalse(result.isv);
            Assert.AreEqual("Nhân viên chưa đủ 18 tuổi!", result.err);
        }

        private StaffManagementVM _viewModel;

        [TestInitialize]
        public void Setup()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            _viewModel = new StaffManagementVM();
            _viewModel.Password = "vanphat";
            _viewModel.Repass = "vanphat";
            _viewModel.FullName = "Van Phat";
            _viewModel.Address = "Dia chi";
            _viewModel.Email = "abc@gmail.com";
            _viewModel.Phonenumber = "0775897337";
            _viewModel.Username = "vanphat";
            _viewModel.Cccd = "099999999999";
            _viewModel.Gender = new System.Windows.Controls.ComboBoxItem();
            _viewModel.Startdate = new DateTime();
            _viewModel.Birthday = new DateTime();
        }

        [TestMethod]
        public void IsValidData_ValidDataOfTwoPassword_ReturnsTrue()
        {
            // Arrange
            var validOperation = Operation.CREATE;

            // Act
            var result = _viewModel.IsValidData(validOperation);

            // Assert
            Assert.IsTrue(result.isvalid);
            Assert.IsNull(result.mess);
        }

        [TestMethod]
        public void IsValidData_ValidDataOfTwoPassword_UPDATE_ReturnsTrue()
        {
            // Arrange
            var validOperation = Operation.UPDATE;

            // Act
            var result = _viewModel.IsValidData(validOperation);

            // Assert
            Assert.IsTrue(result.isvalid);
            Assert.IsNull(result.mess);
        }

        [TestMethod]
        public void IsValidData_MissingRequiredFields_ReturnsFalseWithErrorMessage()
        {
            // Arrange
            var invalidOperation = Operation.CREATE;
            _viewModel.Birthday = null;

            // Act
            var result = _viewModel.IsValidData(invalidOperation);

            // Assert
            Assert.IsFalse(result.isvalid);
            Assert.IsNotNull(result.mess);
            Assert.AreEqual("Vui lòng nhập đủ thông tin nhân viên!", result.mess);
        }

        [TestMethod]
        public void IsValidData_InvalidPhoneNumber_ReturnsFalseWithErrorMessage()
        {
            // Arrange
            var invalidPhoneNumberOperation = Operation.CREATE;
            _viewModel.Phonenumber = "02";
            _viewModel.Birthday = new DateTime();

            // Act
            var result = _viewModel.IsValidData(invalidPhoneNumberOperation);

            // Assert
            Assert.IsFalse(result.isvalid);
            Assert.IsNotNull(result.mess);
            Assert.AreEqual("Số điện thoại không hợp lệ!", result.mess);
        }

        [TestMethod]
        public void IsValidData_InvalidAge_ReturnsFalseWithErrorMessage()
        {
            // Arrange
            var invalidAgeOperation = Operation.CREATE;
            _viewModel.Birthday = DateTime.Now.AddYears(-17); // Set a birthday that makes the age less than 18

            // Act
            var result = _viewModel.IsValidData(invalidAgeOperation);

            // Assert
            Assert.IsFalse(result.isvalid);
            Assert.IsNotNull(result.mess);
            Assert.AreEqual("Nhân viên chưa đủ 18 tuổi!", result.mess);
        }

        //    [Fact]
        //    public void Constructor_Initialization()
        //    {
        //        // Arrange
        //        var staffManagementVM = new StaffManagementVM();

        //        // Act & Assert
        //        Assert.NotNull(staffManagementVM.StaffList);
        //        Assert.Null(staffManagementVM.SelectedItem);
        //        Assert.False(staffManagementVM.IsSaving);
        //        // ... Kiểm tra các thuộc tính khác
        //    }

        //    [Fact]
        //    public void FirstLoadCommand_LoadsStaffList()
        //    {
        //        // Arrange
        //        var staffManagementVM = new StaffManagementVM();

        //        // Act
        //        staffManagementVM.FirstLoadCM.Execute(null);

        //        // Assert
        //        Assert.NotNull(staffManagementVM.StaffList);
        //        Assert.True(staffManagementVM.StaffList.Any()); // Kiểm tra xem danh sách nhân viên có phần tử không
        //                                                        // ... Kiểm tra các điều kiện khác
        //    }

        //    [Fact]
        //    public void AddStaffCommand_AddsNewStaff()
        //    {
        //        // Arrange
        //        var staffManagementVM = new StaffManagementVM();

        //        // Act
        //        // Simulate adding a new staff
        //        staffManagementVM.AddStaffCM.Execute(null);

        //        // Assert
        //        Assert.True(staffManagementVM.StaffList.Any(s => s.StaffId == staffManagementVM.StaffId));
        //        // ... Kiểm tra các điều kiện khác
        //    }

        //    [Fact]
        //    public void UploadImgCommand_LoadsImage()
        //    {
        //        // Arrange
        //        var staffManagementVM = new StaffManagementVM();

        //        // Act
        //        // Simulate uploading an image
        //        staffManagementVM.UploadImgCM.Execute(null);

        //        // Assert
        //        Assert.NotNull(staffManagementVM.ImageSource);
        //        // ... Kiểm tra các điều kiện khác
        //    }

        //    [Fact]
        //    public void EditStaffCommand_EditsExistingStaff()
        //    {
        //        // Arrange
        //        var staffManagementVM = new StaffManagementVM();

        //        // Act
        //        // Simulate editing an existing staff
        //        staffManagementVM.EditStaffCM.Execute(null);

        //        // Assert
        //        Assert.True(staffManagementVM.StaffList.Any(s => s.StaffId == staffManagementVM.StaffId && s.StaffName == staffManagementVM.FullName));
        //        // ... Kiểm tra các điều kiện khác
        //    }

        //    [Fact]
        //    public void ChangePasswordCommand_ChangesPassword()
        //    {
        //        // Arrange
        //        var staffManagementVM = new StaffManagementVM();

        //        // Act
        //        // Simulate changing the password
        //        staffManagementVM.ChangePasswordCM.Execute(null);

        //        // Assert
        //        // ... Kiểm tra xem mật khẩu đã thay đổi đúng không
        //    }
    }
}
