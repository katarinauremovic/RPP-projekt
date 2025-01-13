namespace EntityLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Review")]
    public partial class Review
    {
        [Key]
        public int idReview { get; set; }

        public int? Rating { get; set; }

        [Column(TypeName = "text")]
        public string Comment { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        public int? Client_idClient { get; set; }

        public int? Reservation_idReservation { get; set; }

        public int? Employee_idEmployee { get; set; }

        public int? Treatment_idTreatment { get; set; }

        public virtual Client Client { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Reservation Reservation { get; set; }

        public virtual Treatment Treatment { get; set; }
    }
}
