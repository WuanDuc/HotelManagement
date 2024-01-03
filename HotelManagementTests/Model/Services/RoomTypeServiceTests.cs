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
                    Price = 1000000,
                    Note = "Notes",
                },

                new RoomType()
                {
                    RoomTypeId = "RT002",
                    RoomTypeName = "Room Type 2",
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
                    RoomTypeId = "RT002",
                    RoomTypeName = "Room Type 2",
                    RoomTypePrice = 100000,
                    RoomTypeNote = "Note",
                }
            };
            service = new RoomTypeService(mockEntities.Object);
            var result = await service.GetAllRoomType();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(expected.First().RoomTypeId.ToString(), result.First().RoomTypeId.ToString());
            Assert.AreEqual(expected.First().RoomTypeName.ToString(), result.First().RoomTypeName.ToString());
            Assert.AreEqual(expected.First().RoomTypePrice.ToString(), result.First().RoomTypePrice.ToString());
            Assert.AreEqual(expected.First().RoomTypeNote.ToString(), result.First().RoomTypeNote.ToString());
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
        public async Task GetRoomTypeIDTest()
        {
            string rtname = "Room Type 1";
            string expected = "RT001";
            service = new RoomTypeService(mockEntities.Object);
            var result = await service.GetRoomTypeID(rtname);

            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        public async Task UpdateRoomTypeTest_UpdateSuccessfully()
        {
            service = new RoomTypeService(mockEntities.Object);

            RoomTypeDTO rt = new RoomTypeDTO()
            {
                RoomTypeId = "RT001",
                RoomTypeName = "Room Type 3",
                RoomTypePrice = 100000,
                RoomTypeNote = "Note",
            };

            var result = await service.UpdateRoomType(rt);

            Assert.AreEqual((true, "Cập nhật thành công"), result);

        }

        [TestMethod()]
        public async Task UpdateRoomTypeTest_RoomNameExist_ReturnFalse()
        {
            service = new RoomTypeService(mockEntities.Object);

            RoomTypeDTO rt = new RoomTypeDTO()
            {
                RoomTypeId = "RT001",
                RoomTypeName = "Room Type 2",
                RoomTypePrice = 100000,
                RoomTypeNote = "Note",
            };

            var result = await service.UpdateRoomType(rt);

            Assert.AreEqual((false, "Tên loại phòng đã tồn tại!"), result);
        }

        [TestMethod()]
        public async Task UpdateRoomTypeTest_RoomNotExist_ReturnFalse()
        {
            service = new RoomTypeService(mockEntities.Object);

            RoomTypeDTO rt = new RoomTypeDTO()
            {
                RoomTypeId = "RT004",
                RoomTypeName = "Room Type 2",
                RoomTypePrice = 100000,
                RoomTypeNote = "Note",
            };

            var result = await service.UpdateRoomType(rt);

            Assert.AreEqual((false, "Loại phòng này không tồn tại!"), result);
        }

        [TestMethod()]
        public void TestIns_NullValue_returnRTS()
        {
            service = new RoomTypeService(mockEntities.Object);

            Assert.AreNotEqual(RoomTypeService.Ins, null);
        }

        [TestMethod()]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public async Task Context_nullvalue_returnNotNull()
        {
            service = new RoomTypeService(null);
            await service.GetRoomTypeID("Room Type 2");
        }
    }
}