using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
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
    public class TreatmentService: ITreatmentService
    {
        public async Task<IEnumerable<TreatmentDTO>> GetAllTreatmentsAsync()
        {
            using (var repo = new TreatmentRepository())
            {
                var treatments = await repo.GetAllTreatmentsWithDetailsAsync();

                return treatments.Select(t => new TreatmentDTO
                {
                    idTreatment = t.idTreatment,
                    Name = t.Name,
                    Price = t.Price,
                    Description = t.Description,
                    DurationMinutes = t.DurationMinutes,
                    TreatmentGroupName = t.TreatmentGroupName,
                    WorkPositionName = t.WorkPositionName
                }).ToList();
            }
        }

        public async Task<IEnumerable<Treatment>> GetTreatmentByNameAsync(string namePattern)
        {
            using (var repo = new TreatmentRepository())
            {
                var treatments = await repo.GetTreatmentsByNamePattern(namePattern);
                return treatments;
            }
        }

        public async Task<IEnumerable<TreatmentGroup>> GetAllTreatmentGroupsAsync()
        {
            using (var repo = new TreatmentRepository())
            {
                return await repo.GetAllTreatmentGroupsAsync();
            }
        }

        public async Task<IEnumerable<WorkPosition>> GetAllWorkPositionsAsync()
        {
            using (var repo = new TreatmentRepository())
            {
                return await repo.GetAllWorkPositionsAsync();
            }
        }
        public async Task<IEnumerable<TreatmentDTO>> GetTreatmentsByGroupAsync(int groupId)
        {
            using (var repo = new TreatmentRepository())
            {
                return await repo.GetTreatmentsByGroupAsync(groupId); 
            }
        }
        public async Task<IEnumerable<TreatmentDTO>> GetTreatmentsByWorkPositionAsync(int workPositionId)
        {
            using (var repo = new TreatmentRepository())
            {
                return await repo.GetTreatmentsByWorkPositionAsync(workPositionId);
            }
        }

    }
}
