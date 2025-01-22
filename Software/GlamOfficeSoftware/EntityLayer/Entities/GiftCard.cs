namespace EntityLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GiftCard")]
    public partial class GiftCard
    {
        [Key]
        public int idGiftCard { get; set; }

        public decimal? Value { get; set; }

        public decimal? ToSpend { get; set; }

        [StringLength(45)]
        public string Status { get; set; }
        
        public DateTime? ActivationDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime? RedemptionDate { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [StringLength(45)]
        public string PromoCode { get; set; }
    }
}
