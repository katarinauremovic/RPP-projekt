namespace EntityLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TreatmentGroup")]
    public partial class TreatmentGroup
    {
        [Key]
        public int idTreatmentGroup { get; set; }

        [StringLength(200)]
        public string Name { get; set; }
    }
}
