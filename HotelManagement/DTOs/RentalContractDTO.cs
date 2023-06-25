using HotelManagement.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.DTOs
{
    public class RentalContractDTO
    {
        public string RentalContractId { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.TimeSpan> StartTime { get; set; }
        public Nullable<System.DateTime> CheckOutDate { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string RoomId { get; set; }
        public string RoomTypeName { get; set; }
        public Nullable<int> RoomNumber { get; set; }
        public Nullable<double> RoomPrice { get; set; }

        public string RoomName
        {
            get { return "Phòng " + RoomNumber.ToString(); }
        }
        public Nullable<bool> Validated { get; set; }
        public int PersonNumber { get; set; }
        public string PersonNumberStr
        {
            get { return PersonNumber.ToString(); }
        }
        public IList<CustomerDTO> CustomersOfRoom { get; set; }
        public string StartDateStr
        {
            get { return ((DateTime)StartDate).ToString("dd/MM/yyyy"); }
        }
        public string CheckOutDateStr
        {
            get { return ((DateTime)CheckOutDate).ToString("dd/MM/yyyy"); }
        }
        public int DayNumber
        {
            get
            {
                if (CheckOutDate == null || StartDate == null)
                {
                    return 0;
                }
                else
                {
                    if (!(bool)Validated) return 0;
                }
                TimeSpan t = (TimeSpan)(CheckOutDate - StartDate);
                int res = (int)t.TotalDays;
                return res;
            }
        }
    }
}
