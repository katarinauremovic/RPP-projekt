using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Repositories;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ReviewRepository _reviewRepository;

        public ReviewService()
        {
            _reviewRepository = new ReviewRepository();
        }

        public async Task<Dictionary<int, int>> GetReviewDistributionAsync()
        {
            return await _reviewRepository.GetReviewDistributionAsync();
        }

        public async Task<Dictionary<string, double>> GetAverageRatingByEmployeeAsync()
        {
            return await _reviewRepository.GetAverageRatingByEmployeeAsync();
        }


        public async Task<Dictionary<string, int>> GetReviewTrendsOverTimeAsync()
        {
            return await _reviewRepository.GetReviewTrendsOverTimeAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsByEmployeeIdAsync(int employeeId)
        {
            return await _reviewRepository.GetReviewsByEmployeeIdAsync(employeeId);
        }

        public async Task<Dictionary<string, double>> GetAverageRatingByTreatmentAsync()
        {
            return await _reviewRepository.GetAverageRatingByTreatmentAsync();
        }
        public async Task<List<KeyValuePair<string, double>>> GetTopTreatmentsAsync(int topCount = 3)
        {
            var treatmentRatings = await _reviewRepository.GetAverageRatingByTreatmentAsync();
            return treatmentRatings.OrderByDescending(t => t.Value).Take(topCount).ToList();
        }

        public async Task<List<KeyValuePair<string, double>>> GetTopEmployeesAsync(int topCount = 3)
        {
            var employeeRatings = await _reviewRepository.GetAverageRatingByEmployeeAsync();

            return employeeRatings
                .OrderByDescending(e => e.Value)
                .Take(topCount)
                .Select(e => new KeyValuePair<string, double>(e.Key.ToString(), e.Value)) 
                .ToList();
        }

    }
}
