using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class TreatmentReceiptDTO
    {
        public string Name { get; set; }
        public int? Quantity { get; set; }
        public string UnitPrice { get; set; }
        public string TotalPrice { get; set; }
    }
}
