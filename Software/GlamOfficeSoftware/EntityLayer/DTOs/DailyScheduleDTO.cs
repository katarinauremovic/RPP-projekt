using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class DailyScheduleDTO
    {
        public int DayId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? WorkStartTime { get; set; }
        public DateTime? WorkEndTime { get; set; }
    }
}
