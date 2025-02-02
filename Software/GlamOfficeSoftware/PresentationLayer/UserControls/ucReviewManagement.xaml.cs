using BusinessLogicLayer.Services;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for ucReviewManagement.xaml
    /// </summary>
    public partial class ucReviewManagement : UserControl
    {
        private readonly ReviewService _reviewService;
        public MainWindow Parent { get; set; }
        public ucReviewManagement()
        {
            InitializeComponent();
            _reviewService = new ReviewService();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadReviewStatistics();
        }
        private async Task LoadReviewStatistics()
        {
            var reviewDistribution = await _reviewService.GetReviewDistributionAsync();
            var avgRatingByEmployee = await _reviewService.GetAverageRatingByEmployeeAsync();
            var reviewTrends = await _reviewService.GetReviewTrendsOverTimeAsync();

            // Prikaz prosječne ocjene
            if (reviewDistribution.Any())
            {
                double averageRating = reviewDistribution
                    .Select(x => x.Key * x.Value)
                    .Sum() / (double)reviewDistribution.Values.Sum();

                txtAverageRating.Text = $"{averageRating:F2}";
            }

            // Prikaz grafova
            chartRatingDistribution.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Ratings",
                    Values = new ChartValues<int>(reviewDistribution.Values)
                }
            };
            chartRatingDistribution.AxisX.Clear();
            chartRatingDistribution.AxisX.Add(new Axis
            {
                Title = "Ratings",
                Labels = reviewDistribution.Keys.Select(x => x.ToString()).ToList()
            });

            chartEmployeeRatings.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Avg Rating",
                    Values = new ChartValues<double>(avgRatingByEmployee.Values)
                }
            };
            chartEmployeeRatings.AxisX.Clear();
            chartEmployeeRatings.AxisX.Add(new Axis
            {
                Title = "Employees",
                Labels = avgRatingByEmployee.Keys.Select(x => x.ToString()).ToList()
            });

            chartReviewTrends.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Reviews Over Time",
                    Values = new ChartValues<int>(reviewTrends.Values)
                }
            };
            chartReviewTrends.AxisX.Clear();
            chartReviewTrends.AxisX.Add(new Axis
            {
                Title = "Date",
                Labels = reviewTrends.Keys.ToList()
            });
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnDropdownSearch_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
