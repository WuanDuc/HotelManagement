using HotelManagement.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.DTOs
{
    public class RoomDTO
    {
        public string RoomId { get; set; }
        public Nullable<int> RoomNumber { get; set; }
        public string RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public string Note { get; set; }
        public string RoomStatus { get; set; }
        public string RoomCleaningStatus { get; set; }
        public double Price { get; set; }
        public string RoomPriceStr
        {
            get { return Helper.FormatVNMoney(Price); }
        }
        public string RoomReadyName
        {
            get { return "P" + RoomNumber.ToString(); }
        }
    }
}
