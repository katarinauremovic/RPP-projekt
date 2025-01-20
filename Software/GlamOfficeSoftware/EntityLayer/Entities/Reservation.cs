namespace EntityLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Reservation")]
    public partial class Reservation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reservation()
        {
            Receipts = new HashSet<Receipt>();
            Reservation_has_Treatment = new HashSet<Reservation_has_Treatment>();
            Reviews = new HashSet<Review>();
        }

        [Key]
        public int idReservation { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [Column(TypeName = "text")]
        public string Remark { get; set; }

        [StringLength(45)]
        public string Status { get; set; }
        
        public decimal? TotalTreatmentAmount { get; set; }
        
        public decimal? GiftCardDiscount { get; set; }
        
        public decimal? RewardDiscount { get; set; }
        
        public decimal? TotalPrice { get; set; }
        
        public bool isPaid { get; set; } = false;

        public int? Client_idClient { get; set; }

        public int? Day_idDay { get; set; }

        public int? Employee_idEmployee { get; set; }

        public virtual Client Client { get; set; }

        public virtual Day Day { get; set; }

        public virtual Employee Employee { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Receipt> Receipts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reservation_has_Treatment> Reservation_has_Treatment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
