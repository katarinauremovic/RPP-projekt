using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class WeeklyScheduleDTO
    {
        public int Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<DayDTO> Days { get; set; } = new List<DayDTO>();
    }
}
