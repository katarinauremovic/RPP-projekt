using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class TreatmentGroupStatisticsDTO
    {
        public string GroupName { get; set; }
        public string TreatmentName { get; set; }
        public int TotalTimesPerformed { get; set; }
    }

}
