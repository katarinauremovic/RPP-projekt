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
            RewardPoints = new HashSet<RewardPoint>();
        }

        [Key]
        public int idReward { get; set; }

        [Required]
        [StringLength(45)]
        public string Name { get; set; }

        [StringLength(45)]
        public string Threshold { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RewardPoint> RewardPoints { get; set; }
    }
}
