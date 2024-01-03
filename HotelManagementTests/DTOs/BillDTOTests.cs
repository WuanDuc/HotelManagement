using HotelManagement.Model.Services;
using HotelManagement.Model;
using HotelManagement.DTOs;
using HotelManagementTests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Utilities;
using System.Runtime.InteropServices.ComTypes;

namespace HotelManagement.DTOs.Tests
{
    /// <summary>
    /// Summary description for BillDTOTests
    /// </summary>
    [TestClass]
    public class BillDTOTests
    {
        [TestMethod()]
        public void RoomName_Correct()
        {
            BillDTO bill = new BillDTO()
            {
                RoomNumber = 101,
            };

            string expected = "Phòng " + bill.RoomNumber;
            Assert.AreEqual(expected, bill.RoomName);
        }

        [TestMethod()] 
        public void RoomPriceStr_Correct()
        {
            BillDTO bill = new BillDTO()
            {
                RoomPrice = 200,
            };

            string expected = Helper.FormatVNMoney((double)200);

            Assert.AreEqual(expected, bill.RoomPriceStr);
        }

        [TestMethod()]
        public void DayNumber_Correct()
        {
            BillDTO bill = new BillDTO()
            {
                CheckOutDate = new DateTime(2023, 12, 20),
                StartDate = new DateTime(2023, 12,18),
            };

         
            int expected = 2;

            Assert.AreEqual(expected, bill.DayNumber);
        }

        [TestMethod()]
        public void DayNumber_StartDateNull()
        {
            BillDTO bill = new BillDTO()
            {
                CheckOutDate = new DateTime(2023, 12, 20),
                StartDate = null,
            };

            Assert.AreEqual(0, bill.DayNumber);
        }

        [TestMethod()]
        public void DayNumber_StartDateNull_CheckoutDateNull()
        {
            BillDTO bill = new BillDTO()
            {
                CheckOutDate = null,
                StartDate = null,
            };

            Assert.AreEqual(0, bill.DayNumber);
        }

        public void DayNumber_CheckoutDateNull()
        {
            BillDTO bill = new BillDTO()
            {
                CheckOutDate = null,
                StartDate = new DateTime(2023, 12, 18),
            };

            Assert.AreEqual(0, bill.DayNumber);
        }

        [TestMethod()]
        public void ServicePriceTemp_Correct()
        {
            double expected = 0;
            BillDTO bill = new BillDTO();

            Assert.AreEqual(expected, bill.ServicePriceTemp);
        }

        [TestMethod()]
        public void ServicePriceTempStr_Correct()
        {
            double expected = 0;
            BillDTO bill = new BillDTO();
            

            Assert.AreEqual(Helper.FormatVNMoney(expected), bill.ServicePriceTempStr);
        }

        [TestMethod()]
        public void TroublePriceTemp_Correct()
        {
            List<TroubleByCustomerDTO> list = new List<TroubleByCustomerDTO>()
            {
                new TroubleByCustomerDTO()
                {
                    PredictedPrice = 220,
                },
                new TroubleByCustomerDTO()
                {
                    PredictedPrice = 420,
                },
            };

            BillDTO bill = new BillDTO() {
                ListTroubleByCustomer = list,
            };

            double t = 640;
            Assert.AreEqual(t, bill.TroublePriceTemp);
        }

        [TestMethod()]
        public void TroublePriceTempStr_Correct()
        {
            List<TroubleByCustomerDTO> list = new List<TroubleByCustomerDTO>()
            {
                new TroubleByCustomerDTO()
                {
                    PredictedPrice = 220,
                },
                new TroubleByCustomerDTO()
                {
                    PredictedPrice = 420,
                },
            };

            BillDTO bill = new BillDTO()
            {
                ListTroubleByCustomer = list,
            };


            double t = 640;
            Assert.AreEqual(Helper.FormatVNMoney(t), bill.TroublePriceTempStr);
        }

        [TestMethod()]

        public void TotalPriceTemp_Correct()
        {
            List<TroubleByCustomerDTO> list = new List<TroubleByCustomerDTO>()
            {
                new TroubleByCustomerDTO()
                {
                    PredictedPrice = 220,
                },
                new TroubleByCustomerDTO()
                {
                    PredictedPrice = 420,
                },
            };

            BillDTO bill = new BillDTO()
            {
                ListTroubleByCustomer = list,
                CheckOutDate = new DateTime(2023, 12, 20),
                StartDate = new DateTime(2023, 12, 18),
                RoomPrice = 200,
            };


            double expected = 640 + 2 * (double)200;
            Assert.AreEqual(expected, bill.TotalPriceTemp);
        }

        [TestMethod()]
        public void TotalPriceTempStr_Correct()
        {
            List<TroubleByCustomerDTO> list = new List<TroubleByCustomerDTO>()
            {
                new TroubleByCustomerDTO()
                {
                    PredictedPrice = 220,
                },
                new TroubleByCustomerDTO()
                {
                    PredictedPrice = 420,
                },
            };

            BillDTO bill = new BillDTO()
            {
                ListTroubleByCustomer = list,
                CheckOutDate = new DateTime(2023, 12, 20),
                StartDate = new DateTime(2023, 12, 18),
                RoomPrice = 200,
            };


            double expected = 640 + 2 * (double)200;
            Assert.AreEqual(Helper.FormatVNMoney(expected), bill.TotalPriceTempStr);
        }
    }
}
