namespace EntityLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Day")]
    public partial class Day
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Day()
        {
            DailySchedules = new HashSet<DailySchedule>();
            Reservations = new HashSet<Reservation>();
        }

        [Key]
        public int idDay { get; set; }

        [StringLength(45)]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        public int? WeeklySchedule_idWeeklySchedule { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DailySchedule> DailySchedules { get; set; }

        public virtual WeeklySchedule WeeklySchedule { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
