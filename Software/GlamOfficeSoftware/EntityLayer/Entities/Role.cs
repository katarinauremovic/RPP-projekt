namespace EntityLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Role")]
    public partial class Role
    {
        [Key]
        public int idRole { get; set; }

        [StringLength(45)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
    }
}
