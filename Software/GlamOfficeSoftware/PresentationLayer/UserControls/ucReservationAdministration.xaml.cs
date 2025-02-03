using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using EntityLayer.DTOs;
using EntityLayer.Entities;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for ucReservationAdministration.xaml
    /// </summary>
    public partial class ucReservationAdministration : UserControl
    {
        private IReservationService _reservationService;

        public MainWindow Parent { get; set; }
        public ucReservationAdministration()
        {
            InitializeComponent();
            _reservationService = new ReservationService();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSortingList();
            ShowLoadingIndicator(true);
            await LoadReservationsAsync();
            ShowLoadingIndicator(false);
        }

        private async Task LoadReservationsAsync()
        {
            try
            {
                var reservations = await Task.Run(() => _reservationService.GetAllReservationsAsync());
                dgvReservations.ItemsSource = reservations;
            }
            catch (FailedToLoadClientsException ex)
            {
                throw new FailedToLoadClientsException("Error while loading reservations.");
            }
        }

        private void LoadSortingList()
        {
            cmbSortingList.Items.Add("Date ascending");
            cmbSortingList.Items.Add("Date descending");
            

            cmbSortingList.SelectedIndex = 0;
            cmbSortingList.IsDropDownOpen = false;
        }

        private void ShowLoadingIndicator(bool isLoading)
        {
            if (isLoading)
            {
                loadingIndicator.Visibility = Visibility.Visible;
                dgvReservations.Visibility = Visibility.Collapsed;
            }
            else
            {
                loadingIndicator.Visibility = Visibility.Collapsed;
                dgvReservations.Visibility = Visibility.Visible;
            }
        }
        private void btnRefreshData_Click(object sender, RoutedEventArgs e)
        {
            RefreshGui();
        }

        
            public async void RefreshGui()
        {
            ShowLoadingIndicator(true);
            await LoadReservationsAsync();
            ShowLoadingIndicator(false);
        }

        private void ShowSidebar()
        {
            var slideInAnimation = FindResource("SlideInAnimation") as Storyboard;
            var sidebarMenu = (FrameworkElement)ccSidebar.Content;

            if (sidebarMenu != null)
            {
                sidebarMenu.Visibility = Visibility.Visible;

                sidebarMenu.Margin = new Thickness(240, 0, 0, 0);

                var marginAnimation = new ThicknessAnimation
                {
                    From = new Thickness(240, 0, 0, 0),
                    To = new Thickness(0, 0, 0, 0),
                    Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
                };

                sidebarMenu.BeginAnimation(MarginProperty, marginAnimation);
            }
        }

        private void btnShowReservationDetails_Click(object sender, RoutedEventArgs e)
        {
            ShowSidebar();
        }

        private void btnAddNewReservation_Click(object sender, RoutedEventArgs e)
        {
            var ucAddnew = new ucAddNewReservation();
            ucAddnew.Parent = this;
            ccSidebar.Content = ucAddnew;
            ShowSidebar();
            
        }

        public async void CloseSideBarMenu()
        {
            var slideOutAnimation = FindResource("SlideOutAnimation") as Storyboard;
            var sidebarMenu = (FrameworkElement)ccSidebar.Content;

            if (sidebarMenu != null)
            {
                slideOutAnimation?.Begin(sidebarMenu);

                slideOutAnimation.Completed += (s, e) =>
                {
                    ccSidebar.Content = null;
                    sidebarMenu.Visibility = Visibility.Collapsed;
                };
            }

            await Task.Delay(500);
            ccSidebar.Content = null;
        }

        private void btnDropDownMenu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbFilters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbSortingList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvReservations.ItemsSource == null)
                return;

            var reservations = dgvReservations.ItemsSource.Cast<Reservation>().ToList();

            switch (cmbSortingList.SelectedIndex)
            {
                case 0:
                    reservations = reservations.OrderBy(res => res.Date).ToList();
                    break;
                case 1:
                    reservations = reservations.OrderByDescending(res => res.Date).ToList();
                    break;
                
                default:
                    return;
            }

            dgvReservations.ItemsSource = null;
            dgvReservations.ItemsSource = reservations;
        }

        private void btnDropDownList_Click(object sender, RoutedEventArgs e)
        {

        }

        private void textSearch_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void cmbFilterOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dgvReservations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
