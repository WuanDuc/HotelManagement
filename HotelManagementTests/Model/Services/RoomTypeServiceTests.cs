using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelManagement.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using System.Data.Entity;
using HotelManagement.DTOs;
using HotelManagement.Model;

namespace HotelManagement.Model.Services.Tests
{
    [TestClass()]
    public class RoomTypeServiceTests
    {
        Mock<HotelManagementEntities> mockEntities;
        RoomTypeService service;
        Mock<DbSet<RoomType>> mockRoomType;

        List<RoomType> roomTypes;

        [TestInitialize]
        public void Setup()
        {
            roomTypes = new List<RoomType>()
            {
                new RoomType()
                {
                    RoomTypeId = "RT001",
                    RoomTypeName = "Room Type 1",
                    Price = 100000,
                    Note = "Note",
                }
            };
            var data = roomTypes.AsQueryable();

            mockRoomType = new Mock<DbSet<RoomType>>();
            mockRoomType.As<IQueryable<RoomType>>().Setup(m => m.Provider).Returns(data.Provider);
            mockRoomType.As<IQueryable<RoomType>>().Setup(m => m.Expression).Returns(data.Expression);
            mockRoomType.As<IQueryable<RoomType>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockRoomType.As<IQueryable<RoomType>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockEntities = new Mock<HotelManagementEntities>();
            mockEntities.Setup(m => m.RoomTypes).Returns(mockRoomType.Object);

            service = new RoomTypeService(mockEntities.Object);

        }
        [TestMethod()]
        public async Task GetAllRoomTypeTest_CorrectTest()
        {
            List<RoomTypeDTO> expected = new List<RoomTypeDTO>()
            {
                new RoomTypeDTO()
                {
                    RoomTypeId = "RT001",
                    RoomTypeName = "Room Type 1",
                    RoomTypePrice = 100000,
                    RoomTypeNote = "Note",
                }
            };
            service = new RoomTypeService(mockEntities.Object);
            var result = await service.GetAllRoomType();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetAllRoomTypeTest_Null_ThrowEx()
        {
            List<RoomType> roomTypes = null;
            var data = roomTypes.AsQueryable();

            mockRoomType = new Mock<DbSet<RoomType>>();
            mockRoomType.As<IQueryable<RoomType>>().Setup(m => m.Provider).Returns(data.Provider);
            mockRoomType.As<IQueryable<RoomType>>().Setup(m => m.Expression).Returns(data.Expression);
            mockRoomType.As<IQueryable<RoomType>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockRoomType.As<IQueryable<RoomType>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockEntities = new Mock<HotelManagementEntities>();
            mockEntities.Setup(m => m.RoomTypes).Returns(mockRoomType.Object);

            service = new RoomTypeService(mockEntities.Object);
            var result = await service.GetAllRoomType();
        }

        [TestMethod()]
        public void GetAllRoomTypeTest()
        {

        }

        [TestMethod()]
        public void GetRoomTypeIDTest()
        {

        }

        [TestMethod()]
        public void UpdateRoomTypeTest()
        {

        }
    }
}