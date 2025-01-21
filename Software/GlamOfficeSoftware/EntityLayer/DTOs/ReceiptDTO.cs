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

        public string TotalTreatmentAmount { get; set; }

        public string GiftCardDiscount { get; set; }

        public string RewardDiscount { get; set; }

        public string TotalPrice { get; set; }

        public DateTime ReceiptIssueDateTime { get; set; }

        public int idReservation { get; set; }
        
        public string ReservationDate { get; set; }

        public ICollection<TreatmentReceiptDTO> Treatments { get; set; }

        public string strTreatments { get; set; }

        public ClientReceiptDTO Client { get; set; }

        public string strClient { get; set; }
        
        public string Employee { get; set; }
    }
}
