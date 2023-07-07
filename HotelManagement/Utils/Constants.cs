using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Utils
{
    public enum Operation
    {
        CREATE,
        READ,
        UPDATE,
        DELETE,
        UPDATE_PASSWORD,
        UPDATE_PROD_QUANTITY,
        UPDATECLEAN
    }
    public class ROOM_TYPE
    {
        public static readonly string ROOM_TYPE_A = "Phòng thường";
        public static readonly string ROOM_TYPE_B = "Phòng vip";
        public static readonly string ROOM_TYPE_C = "Phòng thương gia";
    }
    public class ROOM_STATUS
    {
        public static readonly string READY = "Phòng trống";
        public static readonly string BOOKED = "Phòng đã đặt";
        public static readonly string RENTING = "Phòng đang thuê";
    }
    public class ROOM_CLEANING_STATUS
    {
        public static readonly string CLEANED = "Đã dọn dẹp";
        public static readonly string NOT_CLEANING_YET = "Chưa dọn dẹp";
        public static readonly string REPAIRING = "Sửa chữa";
    }

    public class ROOM_NOTI
    {
        public static readonly int TIME_EXPIRED = 10;
    }
    public class ROOM_INFO
    {
        public static readonly int PERSON_NUMBER = 3;

    }
    public class HOTEL_INFO
    {
        public static readonly string PHONE = "0123456789";
    }

}
