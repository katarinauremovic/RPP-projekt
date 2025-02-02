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
            var reviews = await context.Reviews
                .Where(r => r.Employee_idEmployee != null)
                .ToListAsync();  

            return reviews
                .GroupBy(r => r.Employee_idEmployee ?? 0) 
                .ToDictionary(
                    g => g.Key,
                    g => g.Average(r => r.Rating ?? 0.0) 
                );
        }


        public async Task<Dictionary<string, int>> GetReviewTrendsOverTimeAsync()
        {
            var reviews = await context.Reviews
                .Where(r => r.Date != null) 
                .ToListAsync(); 

            return reviews
                .GroupBy(r => r.Date.Value.ToString("yyyy-MM-dd")) 
                .ToDictionary(
                    g => g.Key,
                    g => g.Count()
                );
        }


        public async Task<IEnumerable<Review>> GetReviewsByEmployeeIdAsync(int employeeId)
        {
            return await context.Reviews
                .Where(r => r.Employee_idEmployee == employeeId)
                .ToListAsync();
        }
    }
}
