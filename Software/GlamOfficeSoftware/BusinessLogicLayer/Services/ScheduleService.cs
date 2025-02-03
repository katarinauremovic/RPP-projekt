using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Repositories;
using EntityLayer.DTOs;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class ScheduleService : IScheduleService
    {
        private ScheduleRepository _scheduleRepository;
        public ScheduleService()
        {
            _scheduleRepository = new ScheduleRepository();
        }
        public async Task AddDailyScheduleAsync(DailyScheduleDTO dailyScheduleDTO)
        {
            if (dailyScheduleDTO == null)
                throw new InvalidScheduleTimeException("Daily schedule cannot be null.");

            if (dailyScheduleDTO.WorkStartTime >= dailyScheduleDTO.WorkEndTime)
                throw new InvalidScheduleTimeException("Start time must be earlier than end time.");

            var day = await _scheduleRepository.GetDayByIdAsync(dailyScheduleDTO.DayId);
            if (day == null)
                throw new Exception("The specified day does not exist in the database!");

            bool hasConflict = day.DailySchedules.Any(ds =>
                ds.Employee_idEmployee == dailyScheduleDTO.EmployeeId &&
                ((ds.WorkStartTime < day.Date.Value.Add(dailyScheduleDTO.WorkEndTime.Value) &&
                  ds.WorkEndTime > day.Date.Value.Add(dailyScheduleDTO.WorkStartTime.Value))));

            if (hasConflict)
                throw new InvalidScheduleTimeException("The time overlaps with the selected employee's existing time");

            var newDailySchedule = new DailySchedule
            {
                Day_idDay = dailyScheduleDTO.DayId,
                Employee_idEmployee = dailyScheduleDTO.EmployeeId,
                WorkStartTime = day.Date.Value.Add(dailyScheduleDTO.WorkStartTime.Value),
                WorkEndTime = day.Date.Value.Add(dailyScheduleDTO.WorkEndTime.Value)
            };

            await _scheduleRepository.AddDailyScheduleAsync(dailyScheduleDTO.DayId, newDailySchedule);
        }


        public async Task DeleteDailyScheduleAsync(int dayId, int employeeId)
        {
            var schedule = await _scheduleRepository.GetDailyScheduleAsync(dayId, employeeId);

            if (schedule == null)
                throw new Exception("No schedule entry found to delete!");

            await _scheduleRepository.DeleteDailyScheduleAsync(dayId, employeeId);
        }

        public async Task<IEnumerable<DayDTO>> GetOrCreateDaysForNextWeekAsync()
        {
            DateTime nextMonday = GetNextMonday(DateTime.Today);

            var days = await _scheduleRepository.GetOrCreateDaysForWeekAsync(nextMonday);

            string[] dayNames = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            var formattedDays = days.Select((d, index) => new DayDTO
            {
                Id = d.idDay,
                Name = dayNames[index], 
                Date = d.Date
            }).ToList();

            return formattedDays;
        }


        public async Task UpdateDailyScheduleAsync(int dayId, int employeeId, TimeSpan newStartTime, TimeSpan newEndTime)
        {
            var schedule = await _scheduleRepository.GetDailyScheduleAsync(dayId, employeeId);

            if (schedule == null)
                throw new Exception("No schedule entry found to update!");

            if (newStartTime >= newEndTime)
                throw new InvalidScheduleTimeException("Start time must be earlier than end time.");

            var day = await _scheduleRepository.GetDayByIdAsync(dayId);
            if (day == null)
                throw new Exception("The specified day does not exist in the database!");

            bool hasConflict = day.DailySchedules.Any(ds =>
                ds.Employee_idEmployee == employeeId &&
                ds.Day_idDay != dayId && 
                ((ds.WorkStartTime < day.Date.Value.Add(newEndTime) &&
                  ds.WorkEndTime > day.Date.Value.Add(newStartTime))));

            if (hasConflict)
                throw new InvalidScheduleTimeException("The new work schedule conflicts with an existing one!");

   
            schedule.WorkStartTime = schedule.Day.Date.Value.Add(newStartTime);
            schedule.WorkEndTime = schedule.Day.Date.Value.Add(newEndTime);

            await _scheduleRepository.UpdateDailyScheduleAsync(schedule);
        }

        private DateTime GetNextMonday(DateTime date)
        {
            int daysUntilMonday = ((int)DayOfWeek.Monday - (int)date.DayOfWeek + 7) % 7;
            return date.AddDays(daysUntilMonday);
        }
        public async Task<IEnumerable<DailyScheduleDTO>> GetSchedulesForDayAsync(int dayId)
        {
            var schedules = await _scheduleRepository.GetSchedulesForDayAsync(dayId);

            return schedules.Select(s => new DailyScheduleDTO
            {
                DayId = s.Day_idDay,
                EmployeeId = s.Employee_idEmployee,
                WorkStartTime = s.WorkStartTime.HasValue ? s.WorkStartTime.Value.TimeOfDay : (TimeSpan?)null,
                WorkEndTime = s.WorkEndTime.HasValue ? s.WorkEndTime.Value.TimeOfDay : (TimeSpan?)null
            }).ToList();
        }

        private DateTime GetCurrentWeekMonday(DateTime date)
        {
            int daysSinceMonday = (int)date.DayOfWeek - (int)DayOfWeek.Monday;
            if (daysSinceMonday < 0) daysSinceMonday += 7;
            return date.AddDays(-daysSinceMonday);
        }

        public async Task<IEnumerable<DayDTO>> GetOrCreateDaysForWeekAsync(DateTime startDate)
        {
            var days = await _scheduleRepository.GetOrCreateDaysForWeekAsync(startDate);

            string[] dayNames = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            var formattedDays = days.Select((d, index) => new DayDTO
            {
                Id = d.idDay,
                Name = dayNames[index],
                Date = d.Date
            }).ToList();

            return formattedDays;
        }

    }
}
