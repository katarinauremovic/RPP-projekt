using BusinessLogicLayer.Exceptions;
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
            try
            {
                if (!decimal.TryParse(txtValue.Text, out decimal value) || value <= 0)
                {
                    throw new InvalidInputException("Value must be a number greater than 0.");
                }

                if (dpActivationDate.SelectedDate == null)
                {
                    throw new InvalidDateFormatException("Please select an activation date.");
                }

                if (dpExpirationDate.SelectedDate == null)
                {
                    throw new InvalidDateFormatException("Please select an expiration date.");
                }

                if (dpActivationDate.SelectedDate >= dpExpirationDate.SelectedDate)
                {
                    throw new InvalidDateFormatException("Expiration date must be after activation date.");
                }

                var newGiftCard = new GiftCard
                {
                    Value = value,
                    ToSpend = value,
                    ActivationDate = dpActivationDate.SelectedDate,
                    ExpirationDate = dpExpirationDate.SelectedDate,
                    
                    Description = txtDescription.Text,
                    PromoCode = txtPromoCode.Text,
                    Status = "Active"
                };

                await _giftCardService.AddNewGiftCardAsync(newGiftCard);

                Parent.RefreshGui();
                Parent.CloseSideBarMenu();
            }
            catch (InvalidInputException ex)
            {
                MessageBox.Show($"Input Error: {ex.Message}", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (InvalidDateFormatException ex)
            {
                MessageBox.Show($"Date Error: {ex.Message}", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        
        }

        private void btnGeneratePromoCode_Click(object sender, RoutedEventArgs e)
        {
            txtPromoCode.Text = NStringGenerator.NStringGenerator.Generate();
             
        }
    }
}
