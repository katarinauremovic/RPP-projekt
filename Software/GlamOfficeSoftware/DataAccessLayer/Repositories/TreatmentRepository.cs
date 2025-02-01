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
    public class TreatmentRepository : Repository<Treatment>, ITreatmentRepository
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
        public async Task<IEnumerable<TreatmentGroup>> GetAllTreatmentGroupsAsync()
        {
            return await context.TreatmentGroups.ToListAsync();
        }

        public async Task<IEnumerable<WorkPosition>> GetAllWorkPositionsAsync()
        {
            return await context.WorkPositions.ToListAsync();
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



    }
}
