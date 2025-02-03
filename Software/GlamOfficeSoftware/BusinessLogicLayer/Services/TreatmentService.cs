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
            using (var repo = new TreatmentGroupRepository())
            {
                return await repo.GetAllTreatmentGroupsAsync();
            }
        }

        public async Task<IEnumerable<WorkPosition>> GetAllWorkPositionsAsync()
        {
            using (var repo = new WorkPositionRepository())
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
        public async Task AddTreatmentAsync(TreatmentDTO treatmentDTO)
        {
            using (var repo = new TreatmentRepository())
            {
                int? groupId = await GetTreatmentGroupIdByName(treatmentDTO.TreatmentGroupName);
                int? positionId = await GetWorkPositionIdByName(treatmentDTO.WorkPositionName);

                var treatment = new Treatment
                {
                    Name = treatmentDTO.Name,
                    Price = treatmentDTO.Price,
                    Description = treatmentDTO.Description,
                    DurationMinutes = treatmentDTO.DurationMinutes,
                    TreatmentGroup_idTreatmentGroup = groupId,
                    WorkPosition_idWorkPosition = positionId 
                };

                await repo.AddTreatmentAsync(treatment);
            }
        }

        private async Task<int?> GetTreatmentGroupIdByName(string groupName)
        {
            using (var repo = new TreatmentGroupRepository())
            {
                var group = await repo.GetByNameAsync(groupName);
                return group?.idTreatmentGroup;
            }
        }

        private async Task<int?> GetWorkPositionIdByName(string positionName)
        {
            using (var repo = new WorkPositionRepository())
            {
                var position = await repo.GetByNameAsync(positionName);
                return position?.idWorkPosition;
            }
        }

        public async Task UpdateTreatmentAsync(TreatmentDTO treatmentDTO)
        {
            using (var repo = new TreatmentRepository())
            {
                var treatment = await repo.GetByIdAsync(treatmentDTO.idTreatment);
                if (treatment == null) return;

                treatment.Name = treatmentDTO.Name;
                treatment.Price = treatmentDTO.Price;
                treatment.Description = treatmentDTO.Description;
                treatment.DurationMinutes = treatmentDTO.DurationMinutes;
                treatment.TreatmentGroup_idTreatmentGroup = await GetTreatmentGroupIdByName(treatmentDTO.TreatmentGroupName);
                treatment.WorkPosition_idWorkPosition = await GetWorkPositionIdByName(treatmentDTO.WorkPositionName);

                await repo.UpdateTreatmentAsync(treatment);
            }
        }
        public async Task DeleteTreatmentAsync(int treatmentId)
        {
            using (var repo = new TreatmentRepository())
            {
                await repo.DeleteTreatmentAsync(treatmentId);
            }
        }

        public async Task AddTreatmentGroupAsync(string groupName)
        {
            using (var repo = new TreatmentGroupRepository())
            {
                var newGroup = new TreatmentGroup { Name = groupName };
                await repo.AddAsync(newGroup);
            }
        }

        public async Task DeleteTreatmentGroupAsync(int groupId)
        {
            using (var repo = new TreatmentGroupRepository())
            {
                var group = await repo.GetByIdAsync(groupId);
                if (group != null)
                {
                    await repo.RemoveAsync(group);
                }
            }
        }

        public async Task<IEnumerable<TreatmentGroupStatisticsDTO>> GetTreatmentStatisticsByGroupAsync()
        {
            using (var repo = new TreatmentRepository())
            {
                return await repo.GetTreatmentStatisticsByGroupAsync();
            }
        }
    }
}

