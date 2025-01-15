using EntityLayer.DTOs;
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
    /// Interaction logic for ucShowClientsProfileSidebar.xaml
    /// </summary>
    public partial class ucShowClientsProfileSidebar : UserControl
    {
        private ClientDTO _selectedClient { get; set; }
        public ucClientAdministration Parent { get; set; }
        public ucShowClientsProfileSidebar(ClientDTO selectedClient)
        {
            InitializeComponent();
            _selectedClient = selectedClient;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadClientsData();
        }

        private void LoadClientsData()
        {
            textFirstname.Text = _selectedClient.Firstname;
            textLastname.Text = _selectedClient.Lastname;
            textEmail.Text = _selectedClient.Email;
            textPhoneNumber.Text = _selectedClient.PhoneNumber;
            textRewardPoints.Text = _selectedClient.RewardPointsCount.ToString();
            textGiftCardDesc.Text = _selectedClient.GiftCardDescription;
            textReservationDates.Text = _selectedClient.ReservationsDates;
            textReviewsComments.Text = _selectedClient.ReviewsComments;
        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            Parent.CloseSidebarMenu();
        }

        private void btnEditProfile_Click(object sender, RoutedEventArgs e)
        {
            var ucEditClientsProfileSidebar = new ucEditClientsProfileSidebar(_selectedClient);
            ucEditClientsProfileSidebar.Parent = this;
            Parent.ccSidebar.Content = ucEditClientsProfileSidebar;
        }
    }
}
