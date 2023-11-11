using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelManagement.ViewModel.AdminVM.StatisticalManagementVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.ViewModel.AdminVM.StatisticalManagementVM.Tests
{
    [TestClass()]
    public class IncomeTests
    {
        private StatisticalManagementVM statisticalVM;

        [TestInitialize]
        public void Setup()
        {
            statisticalVM = new StatisticalManagementVM();
        }
        
        [TestMethod()]
        public void CalculateTrueIncome_EmptyLists_ReturnsZero()
        {
            // Arrange
            
            var l1 = new List<double>();
            var l2 = new List<double>();

            // Act
            statisticalVM.CalculateTrueIncome(l1, l2);

            // Assert
            Assert.AreEqual("0 ₫", statisticalVM.TrueIncome);
            Assert.AreEqual("0 ₫", statisticalVM.TotalIn);
            Assert.AreEqual("0 ₫", statisticalVM.TotalOut);
        }

        [TestMethod()]
        public void CalculateTrueIncome_NegativeValues_CalculatesCorrectly()
        {
            // Arrange
            
            var l2 = new List<double> { -100, 200, 300 };
            var l1 = new List<double> { 50, -75, 100 };

            // Act
            statisticalVM.CalculateTrueIncome(l1, l2);

            // Assert
            Assert.AreEqual("-325,000 đ", statisticalVM.TrueIncome);
            Assert.AreEqual("75,000 đ", statisticalVM.TotalIn);
            Assert.AreEqual("400,000 đ", statisticalVM.TotalOut);
        }

        [TestMethod()]
        public void CalculateTrueIncome_LargeValues_CalculatesCorrectly()
        {
            // Arrange
            
            var l1 = new List<double> { 1000000, 2000000, 3000000 };
            var l2 = new List<double> { 500000, 750000, 1000000 };

            // Act
            statisticalVM.CalculateTrueIncome(l1, l2);

            // Assert
            Assert.AreEqual("3,750,000 đ", statisticalVM.TrueIncome);
            Assert.AreEqual("6,000,000 đ", statisticalVM.TotalIn);
            Assert.AreEqual("2,250,000 đ", statisticalVM.TotalOut);
        }

        [TestMethod()]
        public void CalculateTrueIncome_NullInput_ReturnsZero()
        {
            // Arrange
            

            // Act
            statisticalVM.CalculateTrueIncome(null, null);

            // Assert
            Assert.AreEqual("0 ₫", statisticalVM.TrueIncome);
            Assert.AreEqual("0 ₫", statisticalVM.TotalIn);
            Assert.AreEqual("0 ₫", statisticalVM.TotalOut);
        }

        [TestMethod]
        public void FindMaxPercentage_BothNonZeroInGreater_ReturnsCorrectPercentage()
        {
            // Arrange
            
            float inValue = 50;
            float outValue = 25;

            // Act
            statisticalVM.FindMaxPercentage(inValue, outValue);

            // Assert
            Assert.AreEqual(100, statisticalVM.TotalInPc);
            Assert.AreEqual(50, statisticalVM.TotalOutPc);
        }

        [TestMethod]
        public void FindMaxPercentage_BothZero_ReturnsDefaultPercentages()
        {
            // Arrange
            
            float inValue = 0;
            float outValue = 0;

            // Act
            statisticalVM.FindMaxPercentage(inValue, outValue);

            // Assert
            Assert.AreEqual(10, statisticalVM.TotalInPc);
            Assert.AreEqual(10, statisticalVM.TotalOutPc);
        }

        [TestMethod]
        public void FindMaxPercentage_InZero_ReturnsCorrectPercentage()
        {
            // Arrange
            
            float inValue = 0;
            float outValue = 25;

            // Act
            statisticalVM.FindMaxPercentage(inValue, outValue);

            // Assert
            Assert.AreEqual(10, statisticalVM.TotalInPc);
            Assert.AreEqual(90, statisticalVM.TotalOutPc);
        }

        [TestMethod]
        public void FindMaxPercentage_OutZero_ReturnsCorrectPercentage()
        {
            // Arrange
            
            float inValue = 25;
            float outValue = 0;

            // Act
            statisticalVM.FindMaxPercentage(inValue, outValue);

            // Assert
            Assert.AreEqual(90, statisticalVM.TotalInPc);
            Assert.AreEqual(10, statisticalVM.TotalOutPc);
        }

        [TestMethod]
        public void CalculateRevPercentage_BothZero_ReturnsZero()
        {
            // Arrange
            
            double a1 = 0;
            double a2 = 0;

            // Act
            statisticalVM.Calculate_RevPercentage(a1, a2);

            // Assert
            Assert.AreEqual("0%", statisticalVM.RentalPc);
            Assert.AreEqual("0%", statisticalVM.ServicePc);
        }

        [TestMethod]
        public void CalculateRevPercentage_RentalZero_ServiceNonZero_Returns100PercentService()
        {
            // Arrange
            
            double a1 = 0;
            double a2 = 10;

            // Act
            statisticalVM.Calculate_RevPercentage(a1, a2);

            // Assert
            Assert.AreEqual("0%", statisticalVM.RentalPc);
            Assert.AreEqual("100%", statisticalVM.ServicePc);
        }

        [TestMethod]
        public void CalculateRevPercentage_RentalNonZero_ServiceZero_Returns100PercentRental()
        {
            // Arrange
            
            double a1 = 15;
            double a2 = 0;

            // Act
            statisticalVM.Calculate_RevPercentage(a1, a2);

            // Assert
            Assert.AreEqual("100%", statisticalVM.RentalPc);
            Assert.AreEqual("0%", statisticalVM.ServicePc);
        }

        [TestMethod]
        public void CalculateRevPercentage_BothNonZero_ReturnsCorrectPercentages()
        {
            // Arrange
            
            double a1 = 20;
            double a2 = 30;

            // Act
            statisticalVM.Calculate_RevPercentage(a1, a2);

            // Assert
            Assert.AreEqual("40%", statisticalVM.RentalPc);
            Assert.AreEqual("60%", statisticalVM.ServicePc);
        }

        [TestMethod]
        public void CalculateExpPercentage_AllZeros_ReturnsZeroPercentForAll()
        {
            // Arrange
            
            double a3 = 0;
            double a4 = 0;
            double a5 = 0;

            // Act
            statisticalVM.Calculate_ExpPercentage(a3, a4, a5);

            // Assert
            Assert.AreEqual("0%", statisticalVM.ServiceExPc);
            Assert.AreEqual("0%", statisticalVM.RepairPc);
            Assert.AreEqual("0%", statisticalVM.FurniturePc);
        }

        [TestMethod]
        public void CalculateExpPercentage_ServiceZero_RepairAndFurnitureNonZero_ReturnsCorrectPercentages()
        {
            // Arrange
            double a3 = 0;
            double a4 = 15;
            double a5 = 25;

            // Act
            statisticalVM.Calculate_ExpPercentage(a3, a4, a5);

            // Assert
            Assert.AreEqual("0%", statisticalVM.ServiceExPc);
            Assert.AreEqual("37.5%", statisticalVM.RepairPc);
            Assert.AreEqual("62.5%", statisticalVM.FurniturePc);
        }

        [TestMethod]
        public void CalculateExpPercentage_AllNonZero_ReturnsCorrectPercentages()
        {
            // Arrange
            double a3 = 30;
            double a4 = 20;
            double a5 = 10;

            // Act
            statisticalVM.Calculate_ExpPercentage(a3, a4, a5);

            // Assert
            Assert.AreEqual("50%", statisticalVM.ServiceExPc);
            Assert.AreEqual("33.33%", statisticalVM.RepairPc);
            Assert.AreEqual("16.67%", statisticalVM.FurniturePc);
        }


        //[TestMethod()()]
        //public void LoadIncomeByMonthTest()
        //{

        //}
    }
}