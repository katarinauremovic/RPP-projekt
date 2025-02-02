using BusinessLogicLayer.Exceptions;
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
    /// Interaction logic for ucGiftCardAdministration.xaml
    /// </summary>
    public partial class ucGiftCardAdministration : UserControl
    {
        public MainWindow Parent { get; set; }

        private GiftCardService _giftCardService = new GiftCardService();
        public GiftCard _selectedGiftCard { get; set; }
        public ucGiftCardAdministration()
        {
            InitializeComponent();
        }

        private void btnRefreshData_Click(object sender, RoutedEventArgs e)
        {
            RefreshGui();
            textSearch.Text= "";
            cmbSortingListGiftCard.SelectedIndex = 0;
        }

        private void btnGiftCardSortingList_Click(object sender, RoutedEventArgs e)
        {
            cmbSortingListGiftCard.IsDropDownOpen = true;
        }

        private void btnShowGiftCardDetails_Click(object sender, RoutedEventArgs e)
        {
            var ucShowGiftCard = new ucShowGiftCardSideBar(_selectedGiftCard);
            ucShowGiftCard.Parent = this;
            ccSidebar.Content = ucShowGiftCard;
            ShowSidebar();
        }

        private void btnAddNewGiftCard_Click(object sender, RoutedEventArgs e)
        {
            var ucAddNewGiftCard = new ucAddNewGiftCardSideBar();
            ucAddNewGiftCard.Parent = this;
            ccSidebar.Content = ucAddNewGiftCard;
            ShowSidebar();
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


        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSortingCriteria();
            ShowLoadingIndicator(true);
            await LoadGiftCardsAsync();
            ShowLoadingIndicator(false);
        }

        private void LoadSortingCriteria()
        {
            cmbSortingListGiftCard.Items.Add("Date ascending");
            cmbSortingListGiftCard.Items.Add("Date descending");
            

            cmbSortingListGiftCard.SelectedIndex = 0;
            cmbSortingListGiftCard.IsDropDownOpen = false;
        }

        private void ShowLoadingIndicator(bool isLoading)
        {
            if (isLoading)
            {
                loadingIndicator.Visibility = Visibility.Visible;
                dgvGiftCards.Visibility = Visibility.Collapsed;
            }
            else
            {
                loadingIndicator.Visibility = Visibility.Collapsed;
                dgvGiftCards.Visibility = Visibility.Visible;
            }
        }
        public async void RefreshGui()
        {
            ShowLoadingIndicator(true);
           await LoadGiftCardsAsync();
            ShowLoadingIndicator(false);
            txtSearch.Text = "";
            cmbSortingListGiftCard.SelectedIndex = 0;
        }

        private async Task LoadGiftCardsAsync()
        {
            try
            {
                var giftCards = await Task.Run(() => _giftCardService.GetAllGiftCardsAsync());
                dgvGiftCards.ItemsSource = giftCards;
            }
            catch (FailedToLoadGiftCardsException ex)
            {
                throw new FailedToLoadGiftCardsException("Error while loading gift cards.");
            }
        }

        private void dgvGiftCards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvGiftCards.SelectedItem is GiftCard selected)
            {
                _selectedGiftCard = selected;
            }
        }

        private void textSearch_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtSearch.Focus();
        }

        private async void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            await SearchGiftCardsAsync();
        }

        private async Task SearchGiftCardsAsync()
        {
            string searchText = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                ShowLoadingIndicator(true);
                await LoadGiftCardsAsync(); 
                ShowLoadingIndicator(false);
                return;
            }

            try
            {
                ShowLoadingIndicator(true);
                IEnumerable<GiftCard> filtered = new List<GiftCard>();
                filtered = await _giftCardService.GetGiftCardsByPromoCodeAsync(searchText);
                dgvGiftCards.ItemsSource = filtered;
            }
            catch (FailedToLoadGiftCardsException ex)
            {
                MessageBox.Show($"Error while searching gift cards: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ShowLoadingIndicator(false);
            }
        }

        private void cmbSortingListGiftCard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSortingListGiftCard.SelectedItem == null)
                return;

            if (dgvGiftCards.ItemsSource == null)
                return;

            var giftCards = dgvGiftCards.ItemsSource.Cast<GiftCard>().ToList();

            switch (cmbSortingListGiftCard.SelectedIndex)
            {
                case 0: 
                    giftCards = giftCards.OrderBy(gc => gc.ActivationDate).ToList();
                    break;
                case 1: 
                    giftCards = giftCards.OrderByDescending(gc => gc.ActivationDate).ToList();
                    break;
                default:
                    return;
            }

            dgvGiftCards.ItemsSource = null; 
            dgvGiftCards.ItemsSource = giftCards;
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

        private void btnEditGiftCard_Click(object sender, RoutedEventArgs e)
        {
            var ucEditGiftCard = new ucEditGiftCardsSideBar(_selectedGiftCard);
            ucEditGiftCard.Parent = this;
            ccSidebar.Content = ucEditGiftCard;
            ShowSidebar();
        }
    }
}
