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

        private void btnPurchase_Click(object sender, RoutedEventArgs e)
        {
            awa
        }
    }
}
