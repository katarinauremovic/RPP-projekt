using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Repositories;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class WeeklyScheduleForReservationService : IWeeklyScheduleForReservation
    {
        public async Task<IEnumerable<Day>> GetDaysForWeeklyScheduleAsync(int weeklyScheduleId)
        {
            using (var repo = new WeeklyScheduleRepositoryForReservation())
                return await repo.GetDaysForWeeklyScheduleAsync(weeklyScheduleId);
        }
    }
}
