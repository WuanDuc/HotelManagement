using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.DTOs;
using HotelManagement.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotelManagementTests.DTOs
{
    [TestClass]
    public class TroubleByCustomerDTOTests
    {
        [TestMethod()]
        public void PredictedPriceStr()
        {
            TroubleByCustomerDTO dto = new TroubleByCustomerDTO()
            {
                PredictedPrice = 200,
            };

            RoomCustomerDTO r = new RoomCustomerDTO()
            {
                STT = 1,
                RoomCustomerId = 1,
                CustomerAddress = "AC",
                CustomerName = "Name",
                CCCD = "12",
                RentalContractId = "RI001",
            };

            Assert.AreEqual(Helper.FormatVNMoney(200), dto.PredictedPriceStr);
        }
    }
}
