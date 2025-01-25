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
            Console.WriteLine("UserControl je učitan!");
            await LoadRewards();
            DataContext = this;
        }

        private async Task LoadRewards()
        {
            Console.WriteLine("Učitavanje nagrada započelo...");
            BronzeRewards = new ObservableCollection<RewardDTO>(await Task.Run(() => _rewardService.GetRewardsDtoByLoyaltyLevelName(LoyaltyLevels.Bronze)));
            SilverRewards = new ObservableCollection<RewardDTO>(await Task.Run(() => _rewardService.GetRewardsDtoByLoyaltyLevelName(LoyaltyLevels.Silver)));
            GoldRewards = new ObservableCollection<RewardDTO>(await Task.Run(() => _rewardService.GetRewardsDtoByLoyaltyLevelName(LoyaltyLevels.Gold)));
            PlatinumRewards = new ObservableCollection<RewardDTO>(await Task.Run(() => _rewardService.GetRewardsDtoByLoyaltyLevelName(LoyaltyLevels.Platinum)));
            VipRewards = new ObservableCollection<RewardDTO>(await Task.Run(() => _rewardService.GetRewardsDtoByLoyaltyLevelName(LoyaltyLevels.VIP)));
            Console.WriteLine("Učitavanje nagrada završeno...");
        }
    }
}
