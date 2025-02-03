using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ScheduleRepository : Repository<WeeklySchedule>
    {
        public async Task<IEnumerable<WeeklySchedule>> GetAllSchedulesAsync()
        {
            return await items
                .Include(ws => ws.Days).Include("Days.DailySchedules")
                .ToListAsync();
        }

        public async Task AddDailyScheduleAsync(int dayInt, DailySchedule dailySchedule)
        {
            var day = await context.Days.FindAsync(dayInt);
            day.DailySchedules.Add(dailySchedule);
            await context.SaveChangesAsync();
        }

        public async Task UpdateDailyScheduleAsync(DailySchedule schedule)
        {
            context.Entry(schedule).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Day>> GetOrCreateDaysForWeekAsync(DateTime startDate)
        {
            DateTime endDate = startDate.AddDays(6);

            var existingSchedule = await context.WeeklySchedules
                .Include(ws => ws.Days)
                .FirstOrDefaultAsync(ws => ws.StartDate == startDate);

            if (existingSchedule != null)
                return existingSchedule.Days;

            var newSchedule = new WeeklySchedule
            {
                StartDate = startDate,
                EndDate = endDate,
                Days = new List<Day>()
            };

            for (int i = 0; i < 7; i++)
            {
                newSchedule.Days.Add(new Day
                {
                    Name = startDate.AddDays(i).ToString("dddd"),
                    Date = startDate.AddDays(i)
                });
            }

            context.WeeklySchedules.Add(newSchedule);
            await context.SaveChangesAsync();

            return newSchedule.Days;
        }

        public async Task DeleteDailyScheduleAsync(int dayId, int employeeId)
        {
            var dailySchedule = await GetDailyScheduleAsync(dayId, employeeId);

            if (dailySchedule != null)
            {
                context.DailySchedules.Remove(dailySchedule);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Day> GetDayByIdAsync(int dayId)
        {
            return await context.Days.FirstOrDefaultAsync(d => d.idDay == dayId);
        }

        public async Task<DailySchedule> GetDailyScheduleAsync(int dayId, int employeeId)
        {
            return await context.DailySchedules
                .Include(ds => ds.Day)
                .FirstOrDefaultAsync(ds => ds.Day_idDay == dayId && ds.Employee_idEmployee == employeeId);
        }

        public async Task<IEnumerable<DailySchedule>> GetSchedulesForDayAsync(int dayId)
        {
            return await context.DailySchedules
                .Where(ds => ds.Day_idDay == dayId) 
                .ToListAsync();
        }
        public async Task<DailySchedule> GetScheduleByIdAsync(int scheduleId)
        {
            return await context.DailySchedules
                .Include(ds => ds.Day)
                .FirstOrDefaultAsync(ds => ds.Day_idDay == scheduleId);
        }


    }
}
