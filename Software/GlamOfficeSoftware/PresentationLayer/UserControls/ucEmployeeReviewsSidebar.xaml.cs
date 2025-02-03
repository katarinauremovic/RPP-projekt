using BusinessLogicLayer;
using BusinessLogicLayer.Services;
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
    /// Interaction logic for ucEmployeeReviewsSidebar.xaml
    /// </summary>
    public partial class ucEmployeeReviewsSidebar : UserControl
    {
        private readonly ReviewService _reviewService = new ReviewService();

        public ucReviewManagement ParentControl { get; internal set; }

        public ucEmployeeReviewsSidebar()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadReviews();
        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            ParentControl.CloseSidebar();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            ParentControl.CloseSidebar();
        }
        public async Task LoadReviews()
        {
            if (!LoggedInEmployee.IsLoggedIn || LoggedInEmployee.LoggedEmployee == null)
            {
                MessageBox.Show("Error: No employee logged in.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int employeeId = LoggedInEmployee.LoggedEmployee.idEmployee;

            var reviews = await _reviewService.GetReviewsByEmployeeIdAsync(employeeId);

            if (reviews != null && reviews.Count > 0)
            {
                reviewsList.ItemsSource = reviews;
            }
            else
            {
                MessageBox.Show("No reviews found for you.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
