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

        public async Task<Dictionary<string, double>> GetAverageRatingByEmployeeAsync()
        {
            var employeeRatings = await context.Reviews
                .Where(r => r.Employee_idEmployee != null)
                .GroupBy(r => new { r.Employee.Firstname, r.Employee.Lastname })
                .Select(g => new
                {
                    EmployeeName = g.Key.Firstname + " " + g.Key.Lastname,
                    AvgRating = g.Average(r => r.Rating) ?? 0.0 
                })
                .ToListAsync();

            return employeeRatings.ToDictionary(g => g.EmployeeName, g => g.AvgRating);
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
        public async Task<Dictionary<string, double>> GetAverageRatingByTreatmentAsync()
        {
            return await context.Reviews
                .Where(r => r.Treatment_idTreatment != null) 
                .GroupBy(r => r.Treatment_idTreatment)
                .Select(g => new
                {
                    TreatmentName = context.Treatments
                                          .Where(t => t.idTreatment == g.Key)
                                          .Select(t => t.Name)
                                          .FirstOrDefault(),
                    AvgRating = g.Average(r => r.Rating) ?? 0.0
                })
                .ToDictionaryAsync(g => g.TreatmentName, g => g.AvgRating);
        }

    }
}
