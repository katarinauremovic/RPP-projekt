namespace EntityLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Client")]
    public partial class Client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            Reservations = new HashSet<Reservation>();
            Reviews = new HashSet<Review>();
            RewardPoints = new HashSet<RewardPoint>();
        }

        [Key]
        public int idClient { get; set; }

        [StringLength(45)]
        public string Firstname { get; set; }

        [StringLength(45)]
        public string Lastname { get; set; }

        [StringLength(45)]
        public string Email { get; set; }

        [StringLength(45)]
        public string PhoneNumber { get; set; }

        public int? GiftCard_idGiftCard { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reservation> Reservations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Review> Reviews { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RewardPoint> RewardPoints { get; set; }
    }
}
