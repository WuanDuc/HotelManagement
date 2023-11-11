using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelManagement.ViewModel.AdminVM.TroubleManagementVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Utilities;
using System.Globalization;
using System.Windows;

namespace HotelManagement.ViewModel.AdminVM.TroubleManagementVM.Tests
{
    [TestClass()]
    public class EditConverterTests
    {
        [TestMethod()]
        public void Convert_CancleStatus_ReturnsCollapsed()
        {
            // Arrange
            var converter = new EditConverter();

            // Act
            var result = converter.Convert(STATUS.CANCLE, typeof(Visibility), null, CultureInfo.CurrentCulture);

            // Assert
            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod()]
        public void Convert_NotCancleStatus_ReturnsVisible()
        {
            // Arrange
            var converter = new EditConverter();

            // Act
            var result = converter.Convert(STATUS.DONE, typeof(Visibility), null, CultureInfo.CurrentCulture);

            // Assert
            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void ConvertBack_Always_ThrowsNotImplementedException()
        {
            // Arrange
            var converter = new EditConverter();

            // Act & Assert
            converter.ConvertBack(null, null, null, null);
        }
        //[TestMethod()]
        //public void ConvertTest()
        //{

        //}

        //[TestMethod()]
        //public void ConvertBackTest()
        //{

        //}
    }
}