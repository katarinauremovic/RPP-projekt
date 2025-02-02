using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class PromotionDTO
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

}
