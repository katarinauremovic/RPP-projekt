namespace EntityLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Client_has_Reward
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Client_idClient { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Reward_idReward { get; set; }

        public int SpentPoints { get; set; }

        [Column(TypeName = "date")]
        public DateTime PurchaseDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime RedeemDate { get; set; }

        [StringLength(45)]
        public string ReedemCode { get; set; }

        [StringLength(45)]
        public string Status { get; set; }

        public virtual Client Client { get; set; }

        public virtual Reward Reward { get; set; }
    }
}
