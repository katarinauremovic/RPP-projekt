namespace EntityLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("WorkPosition")]
    public partial class WorkPosition
    {
        [Key]
        public int idWorkPosition { get; set; }

        [StringLength(45)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }
}
