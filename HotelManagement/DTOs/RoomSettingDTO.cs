using HotelManagement.Model.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HotelManagement.DTOs
{
    public class RoomSettingDTO : IComparable<RoomSettingDTO>   
    {
        public string RoomId { get; set; }
        public Nullable<int> RoomNumber { get; set; }
        public string RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public string Note { get; set; }
        public string RoomStatus { get; set; }
        public string RoomCleaningStatus { get; set; }
        public Nullable<double> Price { get; set; }
        public string RentalContractId { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.TimeSpan> StartTime { get; set; }
        public Nullable<System.DateTime> CheckOutDate { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
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
        
        
     
        public Nullable<bool> Validated { get; set; }
        public string RoomNameP
        {
            get { return "P"+RoomNumber.ToString();  }
        }
        public string RoomName
        {
            get { return "Phòng " + RoomNumber.ToString(); }
        }
        public string StartDateSting
        {
            get 
            { 
                if (StartDate != null) return ((DateTime)StartDate).ToString("dd/MM/yyyy"); 
                else return null;
            }
        }
        public string CheckOutDateSting
        {
            get
            {
                if (CheckOutDate != null) return ((DateTime)CheckOutDate).ToString("dd/MM/yyyy");
                else return null;
            }
        }


        public int CompareTo(RoomSettingDTO other)
        {
            DateTime t1 = (DateTime)this.StartDate + (TimeSpan)this.StartTime;
            DateTime t2 = (DateTime)other.StartDate + (TimeSpan)other.StartTime;

            DateTime o = DateTime.Today + DateTime.Now.TimeOfDay;
            return (t1 - o).CompareTo(t2 - o);
        }

    }
}
