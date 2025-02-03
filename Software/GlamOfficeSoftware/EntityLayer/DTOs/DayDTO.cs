using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class DayDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public int? WeeklyScheduleId { get; set; }
        public List<DailyScheduleDTO> DailySchedules { get; set; } = new List<DailyScheduleDTO>();
    }
}
