namespace EntityLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Receipt")]
    public partial class Receipt
    {
        [Key]
        public int idReceipt { get; set; }

        [StringLength(45)]
        public string ReceiptNumber { get; set; }

        public decimal? TotalTreatmentAmount { get; set; }

        public decimal? GiftCardDiscount { get; set; }

        public decimal? RewardDiscount { get; set; }

        public decimal? TotalPrice { get; set; }

        public DateTime IssueDateTime { get; set; }

        [StringLength(45)]
        public string Status { get; set; }

        public int Reservation_idReservation { get; set; }

        public virtual Reservation Reservation { get; set; }
    }
}
