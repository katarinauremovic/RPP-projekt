using DataAccessLayer.Interfaces;
using EntityLayer.DTOs;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class TreatmentRepository : Repository<Treatment>
    {

        public async Task<IEnumerable<TreatmentDTO>> GetAllTreatmentsWithDetailsAsync()
        {
            var result = await (from t in items
                                join tg in context.TreatmentGroups on t.TreatmentGroup_idTreatmentGroup equals tg.idTreatmentGroup into tgJoin
                                from tg in tgJoin.DefaultIfEmpty()
                                join wp in context.WorkPositions on t.WorkPosition_idWorkPosition equals wp.idWorkPosition into wpJoin
                                from wp in wpJoin.DefaultIfEmpty()
                                select new TreatmentDTO
                                {
                                    idTreatment = t.idTreatment,
                                    Name = t.Name,
                                    Price = t.Price,
                                    Description = t.Description,
                                    DurationMinutes = t.DurationMinutes,
                                    TreatmentGroupName = tg != null ? tg.Name : "No Group",
                                    WorkPositionName = wp != null ? wp.Name : "No Position"
                                })
                        .ToListAsync();

            return result;
        }


        public async Task<IEnumerable<Treatment>> GetTreatmentsByNamePattern(string namePattern)
        {
            return await items
                .Where(t => t.Name.Contains(namePattern)) 
                .ToListAsync();
        }
       

       
        public async Task<IEnumerable<TreatmentDTO>> GetTreatmentsByGroupAsync(int groupId)
        {
            var result = await (from t in items
                                join tg in context.TreatmentGroups on t.TreatmentGroup_idTreatmentGroup equals tg.idTreatmentGroup into tgJoin
                                from tg in tgJoin.DefaultIfEmpty()
                                join wp in context.WorkPositions on t.WorkPosition_idWorkPosition equals wp.idWorkPosition into wpJoin
                                from wp in wpJoin.DefaultIfEmpty()
                                where t.TreatmentGroup_idTreatmentGroup == groupId
                                select new TreatmentDTO
                                {
                                    idTreatment = t.idTreatment,
                                    Name = t.Name,
                                    Price = t.Price,
                                    Description = t.Description,
                                    DurationMinutes = t.DurationMinutes,
                                    TreatmentGroupName = tg != null ? tg.Name : "No Group",
                                    WorkPositionName = wp != null ? wp.Name : "No Position"
                                })
                          .ToListAsync();

            return result;
        }
        public async Task<IEnumerable<TreatmentDTO>> GetTreatmentsByWorkPositionAsync(int workPositionId)
        {
            var result = await (from t in items
                                join tg in context.TreatmentGroups on t.TreatmentGroup_idTreatmentGroup equals tg.idTreatmentGroup into tgJoin
                                from tg in tgJoin.DefaultIfEmpty()
                                join wp in context.WorkPositions on t.WorkPosition_idWorkPosition equals wp.idWorkPosition into wpJoin
                                from wp in wpJoin.DefaultIfEmpty()
                                where t.WorkPosition_idWorkPosition == workPositionId
                                select new TreatmentDTO
                                {
                                    idTreatment = t.idTreatment,
                                    Name = t.Name,
                                    Price = t.Price,
                                    Description = t.Description,
                                    DurationMinutes = t.DurationMinutes,
                                    TreatmentGroupName = tg != null ? tg.Name : "No Group",
                                    WorkPositionName = wp != null ? wp.Name : "No Position"
                                })
                                .ToListAsync();

            return result;
        }

        public async Task AddTreatmentAsync(Treatment treatment)
        {
            items.Add(treatment);
            await SaveChangesAsync();
        }
        public async Task UpdateTreatmentAsync(Treatment treatment)
        {
            var existingTreatment = await context.Treatments.FindAsync(treatment.idTreatment);
            if (existingTreatment != null)
            {
                existingTreatment.Name = treatment.Name;
                existingTreatment.Price = treatment.Price;
                existingTreatment.Description = treatment.Description;
                existingTreatment.DurationMinutes = treatment.DurationMinutes;
                existingTreatment.TreatmentGroup_idTreatmentGroup = treatment.TreatmentGroup_idTreatmentGroup;
                existingTreatment.WorkPosition_idWorkPosition = treatment.WorkPosition_idWorkPosition;

                await context.SaveChangesAsync();
            }
        }
        public async Task DeleteTreatmentAsync(int treatmentId)
        {
            var treatment = await items.FirstOrDefaultAsync(t => t.idTreatment == treatmentId);
            if (treatment != null)
            {
                items.Remove(treatment);
                await SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TreatmentGroupStatisticsDTO>> GetTreatmentStatisticsByGroupAsync()
        {
            var statistics = await (from rht in context.Reservation_has_Treatment
                                    join t in context.Treatments on rht.Treatment_idTreatment equals t.idTreatment
                                    join tg in context.TreatmentGroups on t.TreatmentGroup_idTreatmentGroup equals tg.idTreatmentGroup
                                    group rht by new { TreatmentGroupName = tg.Name, TreatmentName = t.Name } into grouped
                                    select new TreatmentGroupStatisticsDTO
                                    {
                                        GroupName = grouped.Key.TreatmentGroupName,
                                        TreatmentName = grouped.Key.TreatmentName,
                                        TotalTimesPerformed = grouped.Sum(rht => rht.Amount ?? 0)
                                    })
                                     .OrderByDescending(t => t.TotalTimesPerformed)
                                     .ToListAsync();

            return statistics;
        }

    }
}
