using EntityLayer.DTOs;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface ITreatmentService
    {
        Task<IEnumerable<TreatmentDTO>> GetAllTreatmentsAsync();
        Task<IEnumerable<Treatment>> GetTreatmentByNameAsync(string namePattern);
        Task<IEnumerable<TreatmentGroup>> GetAllTreatmentGroupsAsync();
        Task<IEnumerable<WorkPosition>> GetAllWorkPositionsAsync();
        Task<IEnumerable<TreatmentDTO>> GetTreatmentsByGroupAsync(int groupId);
        Task<IEnumerable<TreatmentDTO>> GetTreatmentsByWorkPositionAsync(int workPositionId);
        Task AddTreatmentAsync(TreatmentDTO treatmentDTO);
        Task UpdateTreatmentAsync(TreatmentDTO treatmentDTO);
        Task DeleteTreatmentAsync(int treatmentId);
        Task DeleteTreatmentGroupAsync(int groupId);
        Task AddTreatmentGroupAsync(string groupName);
        Task<IEnumerable<TreatmentGroupStatisticsDTO>> GetTreatmentStatisticsByGroupAsync();
    }
}
