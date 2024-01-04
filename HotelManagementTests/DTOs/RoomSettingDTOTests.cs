using HotelManagement.DTOs;
using HotelManagement.Model;
using HotelManagement.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementTests.DTOs
{
    [TestClass]
    public class RoomSettingDTOTests
    {
        [TestMethod()]
        public void DayNumber_Correct()
        {
            RoomSettingDTO bill = new RoomSettingDTO()
            {
                CheckOutDate = new DateTime(2023, 12, 20),
                StartDate = new DateTime(2023, 12, 18),
                Validated = true,
                CustomerId = "C001",
                RoomTypeId = "R001",
                RoomTypeName = "Name",
                Note = "Note",
                RoomStatus = ROOM_STATUS.BOOKED,
                RoomCleaningStatus =ROOM_CLEANING_STATUS.REPAIRING,
                Price = 1,
            };


            int expected = 2;

            Assert.AreEqual(expected, bill.DayNumber);
        }

        [TestMethod()]
        public void DayNumber_FalseValidate()
        {
            RoomSettingDTO bill = new RoomSettingDTO()
            {
                CheckOutDate = new DateTime(2023, 12, 20),
                StartDate = new DateTime(2023, 12, 18),
                Validated = false,
            };


            int expected = 0;

            Assert.AreEqual(expected, bill.DayNumber);
        }

        [TestMethod()]
        public void DayNumber_StartDateNull()
        {
            RoomSettingDTO bill = new RoomSettingDTO()
            {
                CheckOutDate = new DateTime(2023, 12, 20),
                StartDate = null,
                Validated = true,
            };

            Assert.AreEqual(0, bill.DayNumber);
        }

        [TestMethod()]
        public void DayNumber_StartDateNull_CheckoutDateNull()
        {
            RoomSettingDTO bill = new RoomSettingDTO()
            {
                CheckOutDate = null,
                StartDate = null,
                Validated = false,
            };

            Assert.AreEqual(0, bill.DayNumber);
        }

        [TestMethod()]
        public void DayNumber_CheckoutDateNull()
        {
            RoomSettingDTO bill = new RoomSettingDTO()
            {
                CheckOutDate = null,
                StartDate = new DateTime(2023, 12, 18),
            };

            Assert.AreEqual(0, bill.DayNumber);
        }

        [TestMethod()]
        public void RoomReadyName()
        {
            RoomSettingDTO r = new RoomSettingDTO()
            {
                RoomNumber = 202,
            };

            Assert.AreEqual("P202", r.RoomNameP);
        }

        [TestMethod()]
        public void RoomName()
        {
            RoomSettingDTO r = new RoomSettingDTO()
            {
                RoomNumber = 202,
            };

            Assert.AreEqual("Phòng 202", r.RoomName);
        }

        [TestMethod()]
        public void StartDateStr()
        {
            RoomSettingDTO r = new RoomSettingDTO()
            {
                StartDate = new DateTime(2023, 12, 20),
            };

            string expected = "20/12/2023";
            Assert.AreEqual(expected, r.StartDateSting);
        }

        [TestMethod()]
        public void StartDateStr_Null()
        {
            RoomSettingDTO r = new RoomSettingDTO()
            {
                StartDate = null,
            };

            Assert.AreEqual(null, r.StartDateSting);
        }

        [TestMethod()]
        public void CheckOutDateStr()
        {
            RoomSettingDTO r = new RoomSettingDTO()
            {
                CheckOutDate = null,
            };

            Assert.AreEqual(null, r.CheckOutDateSting);
        }

        [TestMethod()]
        public void CompareTo()
        {
            RoomSettingDTO r = new RoomSettingDTO()
            {
                StartTime = new TimeSpan(20, 22, 00),
                StartDate = new DateTime(2023, 12, 20),
            };
            RoomSettingDTO other = new RoomSettingDTO()
            {
                StartTime = new TimeSpan(22, 20, 20),
                StartDate = new DateTime(2023, 11, 20),
            };

            DateTime t1 = (DateTime)new DateTime(2023, 12, 20) + (TimeSpan)new TimeSpan(20, 22, 00);
            DateTime t2 = (DateTime)new DateTime(2023, 11, 20) + (TimeSpan)new TimeSpan(22, 20, 20);
            DateTime o = DateTime.Today + DateTime.Now.TimeOfDay;
            int expected = (t1 - o).CompareTo(t2 - o);

            Assert.AreEqual(expected, r.CompareTo(other));
        }
    }
}
