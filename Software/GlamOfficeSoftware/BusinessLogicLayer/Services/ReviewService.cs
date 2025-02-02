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

        public async Task<Dictionary<int, double>> GetAverageRatingByEmployeeAsync()
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
    }
}
