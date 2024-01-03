using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotelManagementTests.Utils
{
    [TestClass()]
    public class ConstantTests
    {
        [TestMethod()]
        public void ROOM_TYPE_A()
        {
            Assert.AreEqual("Phòng thường", ROOM_TYPE.ROOM_TYPE_A);
        }

        [TestMethod()]
        public void ROOM_TYPE_B()
        {
            Assert.AreEqual("Phòng vip", ROOM_TYPE.ROOM_TYPE_B);
        }

        [TestMethod()]
        public void ROOM_TYPE_C()
        {
            Assert.AreEqual("Phòng thương gia", ROOM_TYPE.ROOM_TYPE_C);
        }

        [TestMethod()]
        public void ROOM_STATUS_R()
        {
            Assert.AreEqual("Phòng trống", ROOM_STATUS.READY);
        }

        [TestMethod()]
        public void ROOM_STATUS_B()
        {
            Assert.AreEqual("Phòng đã đặt", ROOM_STATUS.BOOKED);
        }

        [TestMethod()]
        public void ROOM_STATUS_Re()
        {
            Assert.AreEqual("Phòng đang thuê", ROOM_STATUS.RENTING);
        }

        [TestMethod()]
        public void ROOM_CLEANING_STATUS_C()
        {
            Assert.AreEqual("Đã dọn dẹp", ROOM_CLEANING_STATUS.CLEANED);
        }

        [TestMethod()]
        public void ROOM_CLEANING_STATUS_NC()
        {
            Assert.AreEqual("Chưa dọn dẹp", ROOM_CLEANING_STATUS.NOT_CLEANING_YET);
        }

        [TestMethod()]
        public void ROOM_CLEANING_STATUS_R()
        {
            Assert.AreEqual("Sửa chữa", ROOM_CLEANING_STATUS.REPAIRING);
        }

        [TestMethod()]
        public void HOTEL_INFO_Test()
        {
            Assert.AreEqual("0123456789", HOTEL_INFO.PHONE);
        }
    }
}
