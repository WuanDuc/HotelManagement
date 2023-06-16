using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HotelManagement.DTOs
{
    public class StaffDTO
    {
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string PhoneNumber { get; set; }
        public string StaffAddress { get; set; }
        public string Email { get; set; }
        public string CCCD { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public Nullable<System.DateTime> dateOfStart { get; set; }
        public string Gender { get; set; }
        public string Position { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public byte[] Avatar { get; set; }

        
    }
}
