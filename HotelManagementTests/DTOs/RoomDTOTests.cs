using HotelManagement.DTOs;
using HotelManagement.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HotelManagementTests.DTOs.Tests
{
    [TestClass()]
    public class RoomDTOTests
    {
        [TestMethod()]
        public void RoomPriceStr()
        {
            RoomDTO roomDTO = new RoomDTO()
            {
                Price = 200,
            };

            Assert.AreEqual(Helper.FormatVNMoney(200), roomDTO.RoomPriceStr);
        }

        [TestMethod()]

        public void RoomReadyName()
        {
            RoomDTO roomDTO = new RoomDTO()
            {
                RoomNumber = 202,   
            };

            Assert.AreEqual("P202", roomDTO.RoomReadyName);
        }
    }
}