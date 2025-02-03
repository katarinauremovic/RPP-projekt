using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class WeeklyScheduleRepositoryForReservation : Repository<WeeklySchedule>
    {
        public async Task<IEnumerable<Day>> GetDaysForWeeklyScheduleAsync(int weeklyScheduleId)
        {
            return await context.Days
                .Where(d => d.WeeklySchedule_idWeeklySchedule == weeklyScheduleId)
                .ToListAsync();
        }
    }
}
