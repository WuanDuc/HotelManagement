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
    public class TroubleDTOTests
    {
        [TestMethod()]
        public void PriceFixStr()
        {
            TroubleDTO dto = new TroubleDTO()
            {
                TroubleId = "TB001",
                Title = "Title",
                Avatar = new byte[] {},
                Description = "Description",
                Reason = REASON.BYCUSTOMER,
                StartDate = DateTime.Now,
                Status = STATUS.WAITING,
                Level = LEVEL.NORMAL,
                StaffId = "NV001",
                Price = 200,
            };  

            Assert.AreEqual(Helper.FormatVNMoney((double)200), dto.PriceFixStr);    
        }
    }
}
