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
        Task<Dictionary<int, double>> GetAverageRatingByEmployeeAsync();
        Task<Dictionary<string, int>> GetReviewTrendsOverTimeAsync();
        Task<IEnumerable<Review>> GetReviewsByEmployeeIdAsync(int employeeId);
    }
}
