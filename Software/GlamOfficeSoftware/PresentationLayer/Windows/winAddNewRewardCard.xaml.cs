using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using EntityLayer.Entities;
using EntityLayer.Enums;
using PresentationLayer.Converters;
using PresentationLayer.UserControls;
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
using System.Xml.Linq;

namespace PresentationLayer.Windows
{
    /// <summary>
    /// Interaction logic for winAddNewRewardCard.xaml
    /// </summary>
    public partial class winAddNewRewardCard : Window
    {
        private ILoyaltyLevelService _loyaltyLevelService;
        private IRewardService _rewardService;

        public ucRewards Parent { get; set; }

        public winAddNewRewardCard()
        {
            InitializeComponent();
            _loyaltyLevelService = new LoyaltyLevelService();
            _rewardService = new RewardService();
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
            var name = txtName.Text;

            if (!string.IsNullOrEmpty(name) && name.Length >= 0)
            {
                placeholderName.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderName.Visibility = Visibility.Visible;
            }

            if (!IsLettersOnly(name))
            {
                if (string.IsNullOrEmpty(name)) return;
                txtName.Clear();
                MessageBox.Show("Reward name must contain only letters.");
                return;
            }
        }

        private void textDescription_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtDescription.Focus();
        }

        private void txtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            var placeholderDescription = textDescription;
            var description = txtDescription.Text;

            if (!string.IsNullOrEmpty(description) && description.Length >= 0)
            {
                placeholderDescription.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderDescription.Visibility = Visibility.Visible;
            }

            if (!IsLettersOnly(description))
            {
                if (string.IsNullOrEmpty(description)) return;
                txtDescription.Clear();
                MessageBox.Show("Description must contain only letters.");
                return;
            }
        }

        private void textCostPoints_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtCostPoints.Focus();
        }

        private void txtCostPoints_TextChanged(object sender, TextChangedEventArgs e)
        {
            var placeholderCostPoints = textCostPoints;
            var costPoints = txtCostPoints.Text;

            if (!string.IsNullOrEmpty(costPoints) && costPoints.Length >= 0)
            {
                placeholderCostPoints.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderCostPoints.Visibility = Visibility.Visible;
            }

            if (!IsNumbersOnly(costPoints))
            {
                if (string.IsNullOrEmpty(costPoints)) return;
                txtCostPoints.Clear();
                MessageBox.Show("Reward cost point must contain only numbers.");
                return;
            }
        }

        private void textRewardAmount_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtRewardAmount.Focus();
        }

        private void txtRewardAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            var placeholderRewardAmount = textRewardAmount;
            var rewardAmount = txtRewardAmount.Text;

            if (!string.IsNullOrEmpty(rewardAmount) && rewardAmount.Length >= 0)
            {
                placeholderRewardAmount.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderRewardAmount.Visibility = Visibility.Visible;
            }

            if (!IsNumbersOnly(rewardAmount))
            {
                if (string.IsNullOrEmpty(rewardAmount)) return;
                txtRewardAmount.Clear();
                MessageBox.Show("Reward amount must contain only numbers.");
                return;
            }
        }

        private void cmbLoyaltyLevels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedLevel = cmbLoyaltyLevels.SelectedItem.ToString();
            DataContext = new { LoyaltyLevelName = selectedLevel };
        }

        private void btnDropdown_Click(object sender, RoutedEventArgs e)
        {
            cmbLoyaltyLevels.IsDropDownOpen = true;
        }

        private async void btnAddNewRewardCard_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtDescription.Text) ||
                string.IsNullOrWhiteSpace(txtCostPoints.Text) ||
                string.IsNullOrWhiteSpace(txtRewardAmount.Text))
            {
                MessageBox.Show("All fields are required!");
                return;
            }

            if (!IsLettersOnly(txtName.Text) || !IsLettersOnly(txtDescription.Text))
            {
                MessageBox.Show("Reward name and description must contain only letters.");
                return;
            }

            if (!IsNumbersOnly(txtCostPoints.Text) || !IsLettersOnly(txtRewardAmount.Text))
            {
                MessageBox.Show("Reward cost points and amount must contain only numbers.");
                return;
            }

            var loyaltyLevelName = (LoyaltyLevels)Enum.Parse(typeof(LoyaltyLevels), cmbLoyaltyLevels.SelectedItem.ToString());
            var loyaltyLevel = await _loyaltyLevelService.GetLoyaltyLevelByNameAsync(loyaltyLevelName);

            var reward = new Reward()
            {
                Name = txtName.Text,
                Description = txtDescription.Text,
                CostPoints = int.Parse(txtCostPoints.Text),
                RewardAmount = -int.Parse(txtRewardAmount.Text),
                LoyaltyLevel_id = loyaltyLevel.Id
            };

            await _rewardService.AddRewardAsync(reward);
            await Parent.RefreshGui();
            ClearInputs();
        }

        private void ClearInputs()
        {
            txtName.Clear();
            txtDescription.Clear();
            txtCostPoints.Clear();
            txtRewardAmount.Clear();
        }

        private bool IsLettersOnly(string value)
        {
            return !string.IsNullOrEmpty(value) && value.All(c =>
                char.IsLetter(c) ||
                char.IsWhiteSpace(c) ||
                c == '-' ||
                char.IsDigit(c) ||
                "!@#$%^&*()_+[]{}|;:',.<>?/".Contains(c));
        }

        private bool IsNumbersOnly(string value)
        {
            return !string.IsNullOrEmpty(value) && value.All(char.IsDigit);
        }

    }
}
