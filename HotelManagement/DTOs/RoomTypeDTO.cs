using HotelManagement.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.DTOs
{
    public class RoomTypeDTO
    {
        public string RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public double RoomTypePrice { get; set; }
        public string RoomTypeNote { get; set; }
        public bool IsDeleted { get; set; }
        public string RoomTypePriceStr
        {
            get { return Helper.FormatVNMoney(RoomTypePrice); }
        }
        public IList<RoomDTO> Rooms { get; set; }

        public double Revenue { get; set; }
        public int STT { get; set; }
    }
}
