namespace EntityLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class RewardPoint
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Client_idClient { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Reward_idReward { get; set; }

        public int Points { get; set; }

        [Column(TypeName = "date")]
        public DateTime AssignmentDate { get; set; }

        [Required]
        [StringLength(45)]
        public string Status { get; set; }

        public virtual Client Client { get; set; }

        public virtual Reward Reward { get; set; }
    }
}
