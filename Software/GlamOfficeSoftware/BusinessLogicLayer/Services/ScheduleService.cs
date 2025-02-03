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
        public async Task<IEnumerable<DailyScheduleDTO>> GetSchedulesForDayAsync(int dayId)
        {
            using (var repo = new ScheduleRepository())
            {
                var schedules = await repo.GetSchedulesForDayAsync(dayId);
                return schedules.Select(ConvertToDTO).ToList();
            }
        }

        public async Task UpdateDailyScheduleAsync(DailyScheduleDTO updatedSchedule)
        {
            using (var repo = new ScheduleRepository())
            {
                var schedule = await repo.GetDailyScheduleAsync(updatedSchedule.DayId, updatedSchedule.EmployeeId);

                if (schedule == null)
                    throw new Exception("No schedule entry found to update!");

                if (!schedule.Day.Date.HasValue)
                    throw new Exception("Schedule day does not have a valid date!");

                schedule.WorkStartTime = schedule.Day.Date.Value.Date.Add(updatedSchedule.WorkStartTime.Value);
                schedule.WorkEndTime = schedule.Day.Date.Value.Date.Add(updatedSchedule.WorkEndTime.Value);

                await repo.UpdateDailyScheduleAsync(schedule);
            }
        }


        public async Task AddDailyScheduleAsync(DailyScheduleDTO newSchedule)
        {
            using (var repo = new ScheduleRepository())
            {
                var dailySchedule = new DailySchedule
                {
                    Day_idDay = newSchedule.DayId,
                    Employee_idEmployee = newSchedule.EmployeeId,
                    WorkStartTime = newSchedule.WorkStartTime,
                    WorkEndTime = newSchedule.WorkEndTime
                };

                await repo.AddDailyScheduleAsync(newSchedule.DayId, dailySchedule);
            }
        }

        public async Task DeleteDailyScheduleAsync(int dayId, int employeeId)
        {
            using (var repo = new ScheduleRepository())
            {
                await repo.DeleteDailyScheduleAsync(dayId, employeeId);
            }
        }

        private DailyScheduleDTO ConvertToDTO(DailySchedule schedule)
        {
            return new DailyScheduleDTO
            {
                DayId = schedule.Day_idDay,
                EmployeeId = schedule.Employee_idEmployee,
                WorkStartTime = schedule.WorkStartTime?.TimeOfDay,
                WorkEndTime = schedule.WorkEndTime?.TimeOfDay
            };
        }
        public async Task<IEnumerable<DayDTO>> GetOrCreateDaysForWeekAsync(DateTime startDate)
        {
            using (var repo = new ScheduleRepository())
            {
                var days = await repo.GetOrCreateDaysForWeekAsync(startDate);

                return days.Select(d => new DayDTO
                {
                    Id = d.idDay,
                    Name = d.Name,
                    Date = d.Date
                }).ToList();
            }
        }

    }

}
