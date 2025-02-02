using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class TreatmentDTO
    {
        public int idTreatment { get; set; }
        public string Name { get; set; }
        public double? Price { get; set; }
        public string Description { get; set; }
        public decimal? DurationMinutes { get; set; }
        public string TreatmentGroupName { get; set; } 
        public string WorkPositionName { get; set; }
    }
}
