using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.DTOs;
using HotelManagement.Model;
using HotelManagement.Utilities;
using HotelManagement.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotelManagementTests.DTOs
{
    [TestClass]
    public class RoomTypeDTOTests
    {
        [TestMethod()]
        public void RoomTypePriceStr()
        {
            RoomTypeDTO roomTypeDTO = new RoomTypeDTO()
            {
                RoomTypePrice = 200,
                RoomTypeId = "R001",
                RoomTypeName = "Room001",
                RoomTypeNote = "note",
                Revenue = 20,
                STT = 1,

            };

            int a = ROOM_INFO.PERSON_NUMBER;

            int b = ROOM_NOTI.TIME_EXPIRED;


            Assert.AreEqual(Helper.FormatVNMoney(200), roomTypeDTO.RoomTypePriceStr);
        }
    }
}
