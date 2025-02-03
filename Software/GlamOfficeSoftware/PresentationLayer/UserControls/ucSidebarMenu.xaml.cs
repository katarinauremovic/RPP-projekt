using BusinessLogicLayer;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using EntityLayer.Entities;
using PresentationLayer.Windows;
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
    /// Interaction logic for ucSidebarMenu.xaml
    /// </summary>
    public partial class ucSidebarMenu : UserControl
    {
        public MainWindow Parent { get; set; }

        public ucSidebarMenu()
        {
            InitializeComponent();
        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            Parent.CloseSidebarMenu();
        }

        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnClientAdministration_Click(object sender, RoutedEventArgs e)
        {
            var ucClientAdministration = new ucClientAdministration();
            ucClientAdministration.Parent = Parent;
            Parent.ccContent.Content = ucClientAdministration;
        }

        private void btnReceipts_Click(object sender, RoutedEventArgs e)
        {
            var ucReceipts = new ucReceipts();
            ucReceipts.Parent = Parent;
            Parent.ccContent.Content = ucReceipts;
        }

        private void btnRewards_Click(object sender, RoutedEventArgs e)
        {
            var ucRewards = new ucRewards();
            ucRewards.Parent = Parent;
            Parent.ccContent.Content = ucRewards;
        }

        private void btnTreatmentManagement_Click(object sender, RoutedEventArgs e)
        {
            var ucTreatments = new ucTreatmentManagement();
            ucTreatments.Parent = Parent;
            Parent.ccContent.Content = ucTreatments;
        }

       

        private void btnEmployees_Click(object sender, RoutedEventArgs e)
        {
            var ucEmployees = new ucEmployeeAdministration();
            ucEmployees.Parent = Parent;
            Parent.ccContent.Content = ucEmployees;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
        "Jeste li sigurni da se želite odjaviti?",
        "Potvrda odjave",
        MessageBoxButton.YesNo,
        MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                LoggedInEmployee.Logout();
                var loginOptionsWindow = new LoginOptions();
                loginOptionsWindow.Show();
                Parent.Close();
            }
        }

        private void btnPromotionCreating_Click(object sender, RoutedEventArgs e)
        {
            var ucPromotions = new ucPromotionCreating();
            ucPromotions.Parent = Parent;
            Parent.ccContent.Content = ucPromotions;
        }

        private void btnReviewsManagement_Click(object sender, RoutedEventArgs e)
        {
            var ucReviews = new ucReviewManagement();
            ucReviews.Parent = Parent;
            Parent.ccContent.Content = ucReviews;
        }

        private void btnSchedule_Click(object sender, RoutedEventArgs e)
        {
            var ucSchedulee = new ucSchedule();
            ucSchedulee.Parent = Parent;
            Parent.ccContent.Content = ucSchedulee;
        }

        private void btnGiftCards_Click(object sender, RoutedEventArgs e)
        {
            var ucGiftCards = new ucGiftCardAdministration();
            ucGiftCards.Parent = Parent;
            Parent.ccContent.Content = ucGiftCards;
        }

        private void btnReservations_Click(object sender, RoutedEventArgs e)
        {
            var ucReservations = new ucReservationAdministration();
            ucReservations.Parent = Parent;
            Parent.ccContent.Content = ucReservations;
        }

       

        private void btnStatistics_Click_1(object sender, RoutedEventArgs e)
        {
            var ucTreatmentStatistics = new ucTreatmentStatistics();
            ucTreatmentStatistics.Parent = Parent;
            Parent.ccContent.Content = ucTreatmentStatistics;
        }
    }
}
