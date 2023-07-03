using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Utilities
{
    
    public class LEVEL
    {
        public static readonly string NORMAL = "Bình thường";
        public static readonly string CRITICAL = "Nghiêm trọng";
    }
    public class STATUS
    {
        public static readonly string WAITING = "Chờ tiếp nhận";
        public static readonly string PREDIT = "Dự đoán giá";
        public static readonly string IN_PROGRESS = "Đang giải quyết";
        public static readonly string DONE = "Đã giải quyết";
        public static readonly string CANCLE = "Đã hủy";
    }
    public class REASON
    {
        public static readonly string BYCUSTOMER = "Bởi khách hàng";
        public static readonly string BYHOTEL = "Bởi khách sạn";
    }
}
