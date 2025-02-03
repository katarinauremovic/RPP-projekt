using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IReviewService
    {
        Task<Dictionary<int, int>> GetReviewDistributionAsync();
        Task<Dictionary<string, double>> GetAverageRatingByEmployeeAsync();
        Task<Dictionary<string, int>> GetReviewTrendsOverTimeAsync();
        Task<List<Review>> GetReviewsByEmployeeIdAsync(int employeeId);
        Task<Dictionary<string, double>> GetAverageRatingByTreatmentAsync();
        Task SyncReviewsFromEmailAsync();
        Task<List<KeyValuePair<string, double>>> GetTopTreatmentsAsync(int topCount = 3);
        Task<List<KeyValuePair<string, double>>> GetTopEmployeesAsync(int topCount = 3);
    }
}
