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
    /// Interaction logic for ucSidebarMenu.xaml
    /// </summary>
    public partial class ucSidebarMenu : UserControl
    {
        public MainWindow Parent { get; set; }

        public ucSidebarMenu()
        {
            InitializeComponent();
        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            Parent.CloseSidebarMenu();
        }

        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnClientAdministration_Click(object sender, RoutedEventArgs e)
        {
            var ucClientAdministration = new ucClientAdministration();
            ucClientAdministration.Parent = Parent;
            Parent.ccContent.Content = ucClientAdministration;
        }

        private void btnReceipts_Click(object sender, RoutedEventArgs e)
        {
            var ucReceipts = new ucReceipts();
            ucReceipts.Parent = Parent;
            Parent.ccContent.Content = ucReceipts;
        }

        private void btnRewards_Click(object sender, RoutedEventArgs e)
        {
            var ucRewards = new ucRewards();
            ucRewards.Parent = Parent;
            Parent.ccContent.Content = ucRewards;
        }
    }
}
