using HotelManagement.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HotelManagement.DTOs
{
    public class TroubleDTO
    {
        public string TroubleId { get; set; }
        public string Title { get; set; }
        public byte[] Avatar { get; set; }
        public string Description { get; set; }
        public string Reason { get; set; }
        public double Price { get; set; }
        public string PriceFixStr
        {
            get { return Helper.FormatVNMoney((double)Price); }
        }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> FixedDate { get; set; }
        public Nullable<System.DateTime> FinishDate { get; set; }
        public string Status { get; set; }
        public string StaffId { get; set; }
        public string Level { get; set; }
        public ImageSource ImagesourgeTrouble 
        { 
            get { return Helper.LoadBitmapImage(Avatar); }
            set { ImagesourgeTrouble = value; }
        }
    }
}
