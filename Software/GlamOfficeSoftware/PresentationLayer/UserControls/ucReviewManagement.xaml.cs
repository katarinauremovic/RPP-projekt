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
            await LoadAverageRatingByTreatmentChart();
            await LoadTopEmployeesAndTreatments();

        }
        private async Task LoadReviewStatistics()
        {
            var reviewDistribution = await _reviewService.GetReviewDistributionAsync();
            var avgRatingByEmployee = await _reviewService.GetAverageRatingByEmployeeAsync();
            var reviewTrends = await _reviewService.GetReviewTrendsOverTimeAsync();

            if (reviewDistribution.Any())
            {
                double averageRating = reviewDistribution
                    .Select(x => x.Key * x.Value)
                    .Sum() / (double)reviewDistribution.Values.Sum();

                txtAverageRating.Text = $"{averageRating:F2}";
            }

            var customColor = new SolidColorBrush(Color.FromRgb(184, 148, 172)); 

            chartRatingDistribution.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Ratings",
                    Values = new ChartValues<int>(reviewDistribution.Values),
                    Fill = customColor
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
                    Values = new ChartValues<double>(avgRatingByEmployee.Values),
                    Fill = customColor
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
                    Values = new ChartValues<int>(reviewTrends.Values),
                    Stroke = customColor,
                    Fill = new SolidColorBrush(Color.FromArgb(50, 184, 148, 172)) 
                }
            };
            chartReviewTrends.AxisX.Clear();
            chartReviewTrends.AxisX.Add(new Axis
            {
                Title = "Date",
                Labels = reviewTrends.Keys.ToList()
            });
        }

        private async Task LoadAverageRatingByTreatmentChart()
        {
            var treatmentRatings = await _reviewService.GetAverageRatingByTreatmentAsync();
            var customColor = new SolidColorBrush(Color.FromRgb(184, 148, 172));

            if (treatmentRatings != null && treatmentRatings.Any())
            {
                chartTreatmentRatings.Series.Clear();
                chartTreatmentRatings.Series.Add(new ColumnSeries
                {
                    Title = "Rating",
                    Values = new ChartValues<double>(treatmentRatings.Values),
                    Fill = customColor
                });

                chartTreatmentRatings.AxisX.Clear();
                chartTreatmentRatings.AxisX.Add(new Axis
                {
                    Labels = treatmentRatings.Keys.ToList(),
                    Separator = new LiveCharts.Wpf.Separator { Step = 1, IsEnabled = false }
                });
            }
        }
        private async Task LoadTopEmployeesAndTreatments()
        {
            var topEmployees = await _reviewService.GetTopEmployeesAsync();
            var topTreatments = await _reviewService.GetTopTreatmentsAsync();

            if (topEmployees != null)
            {
                topEmployeesList.ItemsSource = topEmployees
                    .Select(e => new { Name = e.Key, AvgRating = e.Value.ToString("F2") });
            }

            if (topTreatments != null)
            {
                topTreatmentsList.ItemsSource = topTreatments
                    .Select(t => new { Name = t.Key, AvgRating = t.Value.ToString("F2") });
            }
        }



        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnDropdownSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void btnRefreshData_Click(object sender, RoutedEventArgs e)
        {
            await LoadReviewStatistics();
            await LoadAverageRatingByTreatmentChart();
            await LoadTopEmployeesAndTreatments();
        }

        private async void btnSyncReviews_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSyncReviews.IsEnabled = false;
                btnSyncReviews.Content = "Syncing...";

                await _reviewService.SyncReviewsFromEmailAsync();

                MessageBox.Show("Reviews successfully synced from email!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                await LoadReviewStatistics(); 
                await LoadAverageRatingByTreatmentChart();
                await LoadTopEmployeesAndTreatments();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error syncing reviews: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                btnSyncReviews.IsEnabled = true;
                btnSyncReviews.Content = "Sync Reviews from Email";
            }
        }
    }
}
