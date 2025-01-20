using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class ReceiptDTO
    {
        public int Id { get; set; }

        public string ReceiptNumber { get; set; }

        public decimal? TotalTreatmentAmount { get; set; }

        public decimal? GiftCardDiscount { get; set; }

        public decimal? RewardDiscount { get; set; }

        public decimal? TotalPrice { get; set; }

        public int idReservation { get; set; }
        
        public DateTime? ReservationDate { get; set; }

        public string Treatments { get; set; }
        
        public string Client { get; set; }
        
        public string Employee { get; set; }
    }
}
