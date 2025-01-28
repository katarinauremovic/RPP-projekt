using BusinessLogicLayer.Services;
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
    /// Interaction logic for ucRewardCards.xaml
    /// </summary>
    public partial class ucRewardCards : UserControl
    {
        private RewardSystem _rewardSystem;

        public ucRewardCards()
        {
            InitializeComponent();
            _rewardSystem = new RewardSystem();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ShowButtons();
        }

        private async void btnPurchase_Click(object sender, RoutedEventArgs e)
        {
            var clientId = int.Parse(txtClientId.Text);
            var rewardId = int.Parse(txtRewardId.Text);
            var ucClientReward = new ucClientRewards();
            
            await _rewardSystem.PurchaseReward(clientId, rewardId);

            await ucClientReward.LoadRewardsForSelectedClient(clientId);

            ucClientReward.HaveRewards(ucClientReward.Rewards.Count);

            ShowButtons();
        }

        void ShowButtons()
        {
            if (string.IsNullOrEmpty(txtReedemCode.Text) && string.IsNullOrEmpty(txtStatus.Text) && txtClientId.Text != "0")
            {
                btnPurchase.Visibility = Visibility.Visible;
            } else
            {
                btnPurchase.Visibility = Visibility.Collapsed;
            }
        }
    }
}
