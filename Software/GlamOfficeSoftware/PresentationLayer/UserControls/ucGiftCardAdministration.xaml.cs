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
    /// Interaction logic for ucGiftCardAdministration.xaml
    /// </summary>
    public partial class ucGiftCardAdministration : UserControl
    {
        public MainWindow Parent { get; set; }
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

        }

        private void btnAddNewGiftCard_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSortingCriteria();
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
           //await LoadGiftCardsAsync();
            ShowLoadingIndicator(false);
        }

        private void dgvGiftCards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void textSearch_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtSearch.Focus();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void cmbSortingListGiftCard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSortingListGiftCard.SelectedItem == null)
                return;
        }
    }
}
