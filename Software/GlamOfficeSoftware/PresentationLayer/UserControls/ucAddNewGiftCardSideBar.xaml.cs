using BusinessLogicLayer.Services;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for ucAddNewGiftCardSideBar.xaml
    /// </summary>
    public partial class ucAddNewGiftCardSideBar : UserControl
    {
        public ucGiftCardAdministration Parent { get; set; }
        private GiftCardService _giftCardService;
        public ucAddNewGiftCardSideBar()
        {
            InitializeComponent();
            _giftCardService = new GiftCardService();
        }

        private void btnCloseSidebarGiftCard_Click(object sender, RoutedEventArgs e)
        {
            Parent.CloseSideBarMenu();
        }

        private void btnCancelAdding_Click(object sender, RoutedEventArgs e)
        {
            Parent.CloseSideBarMenu();
        }

        private async void btnAddNewGiftCard_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(txtValue.Text, out decimal value) || value <= 0)
            {
                MessageBox.Show("Value must be a number greater than 0.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dpActivationDate.SelectedDate == null)
            {
                MessageBox.Show("Please select an activation date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dpExpirationDate.SelectedDate == null)
            {
                MessageBox.Show("Please select an expiration date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dpActivationDate.SelectedDate >= dpExpirationDate.SelectedDate)
            {
                MessageBox.Show("Expiration date must be after activation date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            
            

            var newGiftCard = new GiftCard
            {
                Value = value,
                ToSpend = value,
                ActivationDate = dpActivationDate.SelectedDate,
                ExpirationDate = dpExpirationDate.SelectedDate,
                RedemptionDate = dpRedemptionDate.SelectedDate,
                Description = txtDescription.Text,
                PromoCode = txtPromoCode.Text,
                Status = "Waiting" 
            };

            try
            {
                await _giftCardService.AddNewGiftCardAsync(newGiftCard);
               

                
                Parent.RefreshGui();
                Parent.CloseSideBarMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while adding gift card: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnGeneratePromoCode_Click(object sender, RoutedEventArgs e)
        {
            txtPromoCode.Text = NStringGenerator.NStringGenerator.Generate();
             
        }
    }
}
