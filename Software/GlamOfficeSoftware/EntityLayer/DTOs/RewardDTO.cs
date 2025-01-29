using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class RewardDTO
    {
        public int RewardId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CostPoints { get; set; }
        public decimal RewardAmount { get; set; }
        public string LoyaltyLevelName { get; set; }
        public string ReedemCode { get; set; }
        public string Status { get; set; }

    }
}
