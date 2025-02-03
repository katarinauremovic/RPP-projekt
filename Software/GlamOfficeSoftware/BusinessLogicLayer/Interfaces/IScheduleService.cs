using EntityLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IScheduleService
    {
        Task<IEnumerable<DayDTO>> GetOrCreateDaysForWeekAsync(DateTime startDate);

        Task AddDailyScheduleAsync(DailyScheduleDTO dailyScheduleDTO);
        Task UpdateDailyScheduleAsync(DailyScheduleDTO updatedSchedule);
        Task DeleteDailyScheduleAsync(int dayId, int employeeId);
        Task<IEnumerable<DailyScheduleDTO>> GetSchedulesForDayAsync(int dayId);

    }
}
