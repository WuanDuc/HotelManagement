using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.DTOs
{
    public class RoomCustomerDTO
    {
        public int STT { get; set; }
        public int RoomCustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerType { get; set; }
        public string CCCD { get; set; }
        public string CustomerAddress { get; set; }
        public string RentalContractId { get; set; }
    }
}
