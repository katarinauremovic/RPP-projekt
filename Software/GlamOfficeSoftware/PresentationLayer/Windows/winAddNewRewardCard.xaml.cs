using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using PresentationLayer.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;

namespace PresentationLayer.Windows
{
    /// <summary>
    /// Interaction logic for winAddNewRewardCard.xaml
    /// </summary>
    public partial class winAddNewRewardCard : Window
    {
        private ILoyaltyLevelService _loyaltyLevelService;

        public winAddNewRewardCard()
        {
            InitializeComponent();
            _loyaltyLevelService = new LoyaltyLevelService();
        }

        private void textName_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtName.Focus();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadLoyaltyLevels();
        }

        private async Task LoadLoyaltyLevels()
        {
            cmbLoyaltyLevels.ItemsSource = await Task.Run(() => _loyaltyLevelService.GetLoyaltyLevelsEnumsOrderingByLevels());
            cmbLoyaltyLevels.SelectedIndex = 0;
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var placeholderName = textName;
            var pattern = txtName.Text.ToLower();

            if (!string.IsNullOrEmpty(pattern) && pattern.Length >= 0)
            {
                placeholderName.Visibility = Visibility.Collapsed;
                //SearchingByFilter();
            } else
            {
                placeholderName.Visibility = Visibility.Visible;
                //await LoadClientsAsync();
            }
        }

        private void textDescription_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtDescription.Focus();
        }

        private void txtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            var placeholderDescription = textDescription;
            var pattern = txtDescription.Text.ToLower();

            if (!string.IsNullOrEmpty(pattern) && pattern.Length >= 0)
            {
                placeholderDescription.Visibility = Visibility.Collapsed;
                //SearchingByFilter();
            } else
            {
                placeholderDescription.Visibility = Visibility.Visible;
                //await LoadClientsAsync();
            }
        }

        private void textCostPoints_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtCostPoints.Focus();
        }

        private void txtCostPoints_TextChanged(object sender, TextChangedEventArgs e)
        {
            var placeholderCostPoints = textCostPoints;
            var pattern = txtCostPoints.Text.ToLower();

            if (!string.IsNullOrEmpty(pattern) && pattern.Length >= 0)
            {
                placeholderCostPoints.Visibility = Visibility.Collapsed;
                //SearchingByFilter();
            } else
            {
                placeholderCostPoints.Visibility = Visibility.Visible;
                //await LoadClientsAsync();
            }
        }

        private void textRewardAmount_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtRewardAmount.Focus();
        }

        private void txtRewardAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            var placeholderRewardAmount = textRewardAmount;
            var pattern = txtRewardAmount.Text.ToLower();

            if (!string.IsNullOrEmpty(pattern) && pattern.Length >= 0)
            {
                placeholderRewardAmount.Visibility = Visibility.Collapsed;
                //SearchingByFilter();
            } else
            {
                placeholderRewardAmount.Visibility = Visibility.Visible;
                //await LoadClientsAsync();
            }
        }

        private void cmbLoyaltyLevels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedLevel = cmbLoyaltyLevels.SelectedItem.ToString();
            this.DataContext = new { LoyaltyLevelName = selectedLevel };
        }

        private void btnDropdown_Click(object sender, RoutedEventArgs e)
        {
            cmbLoyaltyLevels.IsDropDownOpen = true;
        }

        private void btnAddNewRewardCard_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
