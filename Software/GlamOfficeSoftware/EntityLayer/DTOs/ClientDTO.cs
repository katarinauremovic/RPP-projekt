using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class ClientDTO
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int RewardPointsCount { get; set; }
        public string GiftCardDescription { get; set; }
        public string ReservationsDates { get; set; }
        public string ReviewsComments { get; set; }
    }
}
