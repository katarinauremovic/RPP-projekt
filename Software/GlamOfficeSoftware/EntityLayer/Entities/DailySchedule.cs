namespace EntityLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("DailySchedule")]
    public partial class DailySchedule
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Day_idDay { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Employee_idEmployee { get; set; }

        public DateTime? WorkStartTime { get; set; }

        public DateTime? WorkEndTime { get; set; }

        public virtual Day Day { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
