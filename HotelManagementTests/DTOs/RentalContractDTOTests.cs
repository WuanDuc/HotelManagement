using HotelManagement.DTOs;
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
    public class RentalContractDTOTests
    {
        [TestMethod()]
        public void RoomName()
        {
            RentalContractDTO r = new RentalContractDTO()
            {
                RoomNumber = 100,
            };

            string expected = "Phòng 100";
            Assert.AreEqual(expected, r.RoomName);
        }

        [TestMethod()]
        public void PersonNumberStr()
        {
            RentalContractDTO r = new RentalContractDTO()
            {
                PersonNumber = 2,
            };

            Assert.AreEqual("2", r.PersonNumberStr);
        }

        [TestMethod()]
        public void StartDateStr()
        {
            RentalContractDTO r = new RentalContractDTO()
            {
                StartDate = new DateTime(2023, 12, 20),
            };

            string expected = "20/12/2023";
            Assert.AreEqual(expected, r.StartDateStr);
        }

        [TestMethod()]
        public void CheckOutDateStr()
        {
            RentalContractDTO r = new RentalContractDTO()
            {
                CheckOutDate = new DateTime(2023, 12, 20),
            };

            string expected = "20/12/2023";
            Assert.AreEqual(expected, r.CheckOutDateStr);
        }

        [TestMethod()]
        public void DayNumber_Correct()
        {
            RentalContractDTO bill = new RentalContractDTO()
            {
                CheckOutDate = new DateTime(2023, 12, 20),
                StartDate = new DateTime(2023, 12, 18),
                Validated = true,
            };


            int expected = 2;

            Assert.AreEqual(expected, bill.DayNumber);
        }

        [TestMethod()]
        public void DayNumber_FalseValidate()
        {
            RentalContractDTO bill = new RentalContractDTO()
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
            RentalContractDTO bill = new RentalContractDTO()
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
            RentalContractDTO bill = new RentalContractDTO()
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
            RentalContractDTO bill = new RentalContractDTO()
            {
                CheckOutDate = null,
                StartDate = new DateTime(2023, 12, 18),
            };

            Assert.AreEqual(0, bill.DayNumber);
        }
    }
}
