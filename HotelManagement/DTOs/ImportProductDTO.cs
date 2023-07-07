using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.DTOs
{
    public class ImportProductDTO
    {
        public string ImportId { get; set; }
        public string ProductName { get; set; }
        public float ProductImportPrice { get; set; }
        public int ProductImportQuantity { get; set; }
        public string StaffName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int typeimport { get; set; }// 0: service, 1: funiture
    }
}
