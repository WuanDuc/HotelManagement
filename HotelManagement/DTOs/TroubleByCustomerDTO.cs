using HotelManagement.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.DTOs
{
    public class TroubleByCustomerDTO
    {
        public string TroubleId { get; set; }
        public string RentalContractId { get; set; }
        public string Title { get; set; }
        public string Level { get; set; }
        public Nullable<double> PredictedPrice { get; set; }
        public string PredictedPriceStr
        {
            get
            {
                return Helper.FormatVNMoney((double)PredictedPrice);
            }
        }
    }
}
