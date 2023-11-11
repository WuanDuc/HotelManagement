using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelManagement.ViewModel.AdminVM.TroubleManagementVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Media;
using HotelManagement.Utilities; 

namespace HotelManagement.ViewModel.AdminVM.TroubleManagementVM.Tests
{
    [TestClass()]
    public class StatusBrushConverterTests
    {
        [TestMethod()]
        
        public void ConvertTest_WaitingValue_ReturnDF0404()
        {
            //Arrange
            string test = "Waiting";
            string test1 = STATUS.WAITING;
            string test2 = LEVEL.CRITICAL;
            StatusBrushConverter converter = new StatusBrushConverter();

            //Act
            SolidColorBrush result = (SolidColorBrush)converter.Convert(test, typeof(StatusBrushConverter), null, CultureInfo.CurrentCulture);
            SolidColorBrush result1= (SolidColorBrush)converter.Convert(test1, typeof(StatusBrushConverter), null, CultureInfo.CurrentCulture);
            SolidColorBrush result2= (SolidColorBrush)converter.Convert(test2, typeof(StatusBrushConverter), null, CultureInfo.CurrentCulture);

            //Assert
            Assert.AreEqual(result.ToString(), ((SolidColorBrush)new BrushConverter().ConvertFromString("#DF0404")).ToString());
            Assert.AreEqual(result1.ToString(), ((SolidColorBrush)new BrushConverter().ConvertFromString("#DF0404")).ToString());
            Assert.AreEqual(result2.ToString(), ((SolidColorBrush)new BrushConverter().ConvertFromString("#DF0404")).ToString());    

        }

        [TestMethod()]
        public void ConvertTest_SlovedValue_ReturnFFC5C54()
        {
            //Arrange
            string test = "Sloved";
            string test1 = STATUS.DONE;
            string test2 = LEVEL.NORMAL;
            StatusBrushConverter converter = new StatusBrushConverter();

            //Act
            SolidColorBrush result = (SolidColorBrush)converter.Convert(test, typeof(StatusBrushConverter), null, CultureInfo.CurrentCulture);
            SolidColorBrush result1 = (SolidColorBrush)converter.Convert(test1, typeof(StatusBrushConverter), null, CultureInfo.CurrentCulture);
            SolidColorBrush result2 = (SolidColorBrush)converter.Convert(test2, typeof(StatusBrushConverter), null, CultureInfo.CurrentCulture);
            //string actualResult = ((SolidColorBrush)new BrushConverter().ConvertFromString("#00B087")).ToString();
            //Assert
            Assert.AreEqual(result.ToString(), ((SolidColorBrush)new BrushConverter().ConvertFromString("#00B087")).ToString());
            Assert.AreEqual(result1.ToString(), ((SolidColorBrush)new BrushConverter().ConvertFromString("#00B087")).ToString());
            Assert.AreEqual(result2.ToString(), ((SolidColorBrush)new BrushConverter().ConvertFromString("#00B087")).ToString());
        }

        [TestMethod()]
        public void ConvertTest_SlovingValue_ReturnC0DAF1()
        {
            //Arrange
            string test = "Sloving";
            string test1 = STATUS.IN_PROGRESS;
            string test2 = STATUS.PREDIT;
            StatusBrushConverter converter = new StatusBrushConverter();

            //Act
            SolidColorBrush result = (SolidColorBrush)converter.Convert(test, typeof(StatusBrushConverter), null, CultureInfo.CurrentCulture);
            SolidColorBrush result1 = (SolidColorBrush)converter.Convert(test1, typeof(StatusBrushConverter), null, CultureInfo.CurrentCulture);
            SolidColorBrush result2 = (SolidColorBrush)converter.Convert(test2, typeof(StatusBrushConverter), null, CultureInfo.CurrentCulture);
            string actualResult = ((SolidColorBrush)new BrushConverter().ConvertFromString("#2233C5")).ToString();
            //Assert
            Assert.AreEqual(result.ToString(), actualResult);
            Assert.AreEqual(result1.ToString(), actualResult);
            Assert.AreEqual(result2.ToString(), actualResult);
        }

        [TestMethod()]
        public void ConvertTest_Cancle_ReturnGray()
        {
            //Arrange
            string test = STATUS.CANCLE;
            StatusBrushConverter converter = new StatusBrushConverter();

            //Act
            SolidColorBrush result = (SolidColorBrush)converter.Convert(test, typeof(StatusBrushConverter), null, CultureInfo.CurrentCulture);
            string actualResult = (new SolidColorBrush(Colors.Gray)).ToString();
            //Assert
            Assert.AreEqual(result.ToString(), actualResult);
        }


        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void ConvertBack_Always_ThrowsNotImplementedException()
        {
            // Arrange
            var converter = new StatusBrushConverter();

            // Act & Assert
            converter.ConvertBack(null, null, null, null);
        }
    }
}