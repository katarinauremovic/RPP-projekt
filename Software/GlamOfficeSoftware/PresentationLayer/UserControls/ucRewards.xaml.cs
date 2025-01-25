using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using EntityLayer.DTOs;
using EntityLayer.Entities;
using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ucRewards.xaml
    /// </summary>
    public partial class ucRewards : UserControl
    {
        public ObservableCollection<RewardDTO> BronzeRewards { get; set; }
        public ObservableCollection<RewardDTO> SilverRewards { get; set; }
        public ObservableCollection<RewardDTO> GoldRewards { get; set; }
        public ObservableCollection<RewardDTO> PlatinumRewards { get; set; }
        public ObservableCollection<RewardDTO> VipRewards { get; set; }

        public MainWindow Parent { get; set; }

        private IRewardService _rewardService;

        public ucRewards()
        {
            InitializeComponent();
            _rewardService = new RewardService();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ShowLoadingIndicators(true);
            await LoadRewards();
            ShowLoadingIndicators(false);
            DataContext = this;
        }

        private async Task LoadRewards()
        {
            BronzeRewards = new ObservableCollection<RewardDTO>(await Task.Run(() => _rewardService.GetRewardsDtoByLoyaltyLevelName(LoyaltyLevels.Bronze)));
            SilverRewards = new ObservableCollection<RewardDTO>(await Task.Run(() => _rewardService.GetRewardsDtoByLoyaltyLevelName(LoyaltyLevels.Silver)));
            GoldRewards = new ObservableCollection<RewardDTO>(await Task.Run(() => _rewardService.GetRewardsDtoByLoyaltyLevelName(LoyaltyLevels.Gold)));
            PlatinumRewards = new ObservableCollection<RewardDTO>(await Task.Run(() => _rewardService.GetRewardsDtoByLoyaltyLevelName(LoyaltyLevels.Platinum)));
            VipRewards = new ObservableCollection<RewardDTO>(await Task.Run(() => _rewardService.GetRewardsDtoByLoyaltyLevelName(LoyaltyLevels.VIP)));
        }

        private void ShowLoadingIndicators(bool isLoading)
        {
            loadingIndicatorBronze.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;
            loadingIndicatorSilver.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;
            loadingIndicatorGold.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;
            loadingIndicatorPlatinum.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;
            loadingIndicatorVip.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;

            bronzeRewardsItemsControl.Visibility = isLoading ? Visibility.Collapsed : Visibility.Visible;
            silverRewardsItemsControl.Visibility = isLoading ? Visibility.Collapsed : Visibility.Visible;
            goldRewardsItemsControl.Visibility = isLoading ? Visibility.Collapsed : Visibility.Visible;
            platinumRewardsItemsControl.Visibility = isLoading ? Visibility.Collapsed : Visibility.Visible;
            vipRewardsItemsControl.Visibility = isLoading ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
