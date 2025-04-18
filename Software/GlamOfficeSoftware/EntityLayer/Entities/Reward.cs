namespace EntityLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Reward")]
    public partial class Reward
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reward()
        {
            Client_has_Reward = new HashSet<Client_has_Reward>();
        }

        [Key]
        public int idReward { get; set; }

        [StringLength(45)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public int? CostPoints { get; set; }

        public decimal? RewardAmount { get; set; }

        public int? LoyaltyLevel_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Client_has_Reward> Client_has_Reward { get; set; }

        public virtual LoyaltyLevel LoyaltyLevel { get; set; }
    }
}
