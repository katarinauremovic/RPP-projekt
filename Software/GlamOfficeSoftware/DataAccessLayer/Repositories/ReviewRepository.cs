using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ReviewRepository : Repository<Review>
    {
        public async Task<Dictionary<int, int>> GetReviewDistributionAsync()
        {
            return await context.Reviews
                .GroupBy(r => r.Rating ?? 0)
                .Select(g => new { Rating = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.Rating, g => g.Count);
        }

        public async Task<Dictionary<int, double>> GetAverageRatingByEmployeeAsync()
        {
            return await context.Reviews
                .Where(r => r.Employee_idEmployee != null)
                .GroupBy(r => r.Employee_idEmployee.GetValueOrDefault(0))
                .Select(g => new
                {
                    EmployeeId = g.Key,
                    AvgRating = g.Average(r => r.Rating) ?? 0.0
                })
                .ToDictionaryAsync(g => g.EmployeeId, g => g.AvgRating);
        }

        public async Task<Dictionary<string, int>> GetReviewTrendsOverTimeAsync()
        {
            return await context.Reviews
                .GroupBy(r => (r.Date ?? DateTime.MinValue).Date)
                .Select(g => new { Date = g.Key.ToString("yyyy-MM-dd"), Count = g.Count() })
                .ToDictionaryAsync(g => g.Date, g => g.Count);
        }

        public async Task<IEnumerable<Review>> GetReviewsByEmployeeIdAsync(int employeeId)
        {
            return await context.Reviews
                .Where(r => r.Employee_idEmployee == employeeId)
                .ToListAsync();
        }
    }
}
