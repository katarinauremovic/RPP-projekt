namespace EntityLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Reservation_has_Treatment
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Reservation_idReservation { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Treatment_idTreatment { get; set; }

        public int? Amount { get; set; }

        public virtual Reservation Reservation { get; set; }

        public virtual Treatment Treatment { get; set; }
    }
}
