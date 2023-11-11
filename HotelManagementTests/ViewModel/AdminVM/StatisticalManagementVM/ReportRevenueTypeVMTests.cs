using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelManagement.ViewModel.AdminVM.StatisticalManagementVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Model.Services;
using Moq;

namespace HotelManagement.ViewModel.AdminVM.StatisticalManagementVM.Tests
{
    [TestClass()]
    public class ReportRevenueTypeVMTests
    {
        [TestMethod]
        public async Task ChangeServiceTypeRevenue_SuccessfulExecution()
        {
            // Arrange
            StatisticalManagementVM vm = new StatisticalManagementVM();

            // Act
            await vm.ChangeServiceTypeRevenue();

            // Assert
            Assert.IsNotNull(vm.ListServiceTypeRevenue);
            Assert.IsNotNull(vm.ServiceTypeRevenuePieChart);
        }
        //[TestMethod()]
        //public void ChangeRoomTypeRevenueTest()
        //{

        //}

        //[TestMethod()]
        //public void ChangeServiceTypeRevenueTest()
        //{

        //}
    }
}