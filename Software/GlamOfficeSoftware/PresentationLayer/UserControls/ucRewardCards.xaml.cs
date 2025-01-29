using BusinessLogicLayer.Exceptions;
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
        public static readonly DependencyProperty ParentProperty =
        DependencyProperty.Register(
            "Parent", typeof(ucClientRewards), typeof(ucRewardCards), new PropertyMetadata(null));

        public ucClientRewards Parent
        {
            get { return (ucClientRewards)GetValue(ParentProperty); }
            set { SetValue(ParentProperty, value); }
        }

        private RewardSystem _rewardSystem;

        public ucRewardCards()
        {
            InitializeComponent();
            _rewardSystem = new RewardSystem();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ShowButtons();
            NotEnoughPoints();
        }

        private void NotEnoughPoints()
        {
            if (Parent == null || Parent.txtClientsPoints.Text == null) return;

            var clientPoints = int.Parse(Parent.txtClientsPoints.Text);
            var costPoints = int.Parse(txtCostPoints.Text);

            if (clientPoints < costPoints && string.IsNullOrEmpty(txtReedemCode.Text) && string.IsNullOrEmpty(txtStatus.Text))
            {
                txtNotEnoughPoints.Visibility = Visibility.Visible;
                btnPurchase.Visibility = Visibility.Collapsed;
            }
        }

        private async void btnPurchase_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to buy this reward?",
                "Confirm Purchase",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var clientId = int.Parse(txtClientId.Text);
                    var rewardId = int.Parse(txtRewardId.Text);

                    await _rewardSystem.PurchaseReward(clientId, rewardId);

                    await Parent.RefreshGui(clientId);

                    ShowButtons();
                } catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private bool ShowButtons()
        {
            if (string.IsNullOrEmpty(txtReedemCode.Text) && string.IsNullOrEmpty(txtStatus.Text) && int.Parse(txtClientId.Text) != 0)
            {
                btnPurchase.Visibility = Visibility.Visible;
                return true;
            } else
            {
                btnPurchase.Visibility = Visibility.Collapsed;
                return false;
            }
        }
    }
}
