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
            Client_has_Reward = new HashSet<Client_has_Reward>();
            Reservations = new HashSet<Reservation>();
            Reviews = new HashSet<Review>();
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

        public int? Points { get; set; }

        public int? GiftCard_idGiftCard { get; set; }

        public int? LoyaltyLevel_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Client_has_Reward> Client_has_Reward { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reservation> Reservations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Review> Reviews { get; set; }

        public virtual GiftCard GiftCard { get; set; }

        public virtual LoyaltyLevel LoyaltyLevel { get; set; }
    }
}
