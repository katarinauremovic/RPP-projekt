namespace EntityLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Treatment")]
    public partial class Treatment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Treatment()
        {
            Reservation_has_Treatment = new HashSet<Reservation_has_Treatment>();
            Reviews = new HashSet<Review>();
        }

        [Key]
        public int idTreatment { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public double? Price { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public decimal? DurationMinutes { get; set; }

        public int? TreatmentGroup_idTreatmentGroup { get; set; }

        public int? WorkPosition_idWorkPosition { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reservation_has_Treatment> Reservation_has_Treatment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
