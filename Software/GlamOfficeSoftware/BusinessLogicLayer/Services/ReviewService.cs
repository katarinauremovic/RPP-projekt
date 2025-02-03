using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Repositories;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessLogicLayer.Services.ReviewFormGmailService;

namespace BusinessLogicLayer.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ReviewRepository _reviewRepository;
        private readonly ReviewFormGmailService _emailService;

        public ReviewService()
        {
            _reviewRepository = new ReviewRepository();
            _emailService = new ReviewFormGmailService();
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

        public async Task<List<Review>> GetReviewsByEmployeeIdAsync(int employeeId)
        {
            return (await _reviewRepository.GetReviewsByEmployeeIdAsync(employeeId)).ToList();
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

        public async Task SyncReviewsFromEmailAsync()
        {
            var reviews = await _emailService.FetchReviewsFromEmailAsync();
            if (reviews.Any())
            {
                try
                {
                    await _reviewRepository.AddReviewsAsync(reviews);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error syncing reviews: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    }
                }
            }
        }
    }
}
