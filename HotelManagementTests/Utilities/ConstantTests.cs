using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotelManagementTests.Utilities
{
    [TestClass]
    public class ConstantTests
    {
        [TestMethod()]
        public void LEVEL_Normal()
        {
            Assert.AreEqual("Bình thường", LEVEL.NORMAL);
        }

        [TestMethod()]
        public void LEVEL_Cri()
        {
            Assert.AreEqual("Nghiêm trọng", LEVEL.CRITICAL);
        }

        [TestMethod()]
        public void STATUS_Wait()
        {
            Assert.AreEqual("Chờ tiếp nhận", STATUS.WAITING);
        }

        [TestMethod()]
        public void STATUS_Pre()
        {
            Assert.AreEqual("Dự đoán giá", STATUS.PREDIT);
        }

        [TestMethod()]
        public void STATUS_Pro()
        {
            Assert.AreEqual("Đang giải quyết", STATUS.IN_PROGRESS);
        }

        [TestMethod()]
        public void STATUS_Done()
        {
            Assert.AreEqual("Đã giải quyết", STATUS.DONE);
        }

        [TestMethod()]
        public void STATUS_Cancle()
        {
            Assert.AreEqual("Đã hủy", STATUS.CANCLE);
        }

        [TestMethod()]
        public void REASON_ByCus()
        {
            Assert.AreEqual("Bởi khách hàng", REASON.BYCUSTOMER);
        }

        [TestMethod()]
        public void REASON_ByHotel()
        {
            Assert.AreEqual("Bởi khách sạn", REASON.BYHOTEL);
        }
    }
}
