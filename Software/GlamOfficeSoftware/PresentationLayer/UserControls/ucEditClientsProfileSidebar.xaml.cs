using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using EntityLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for ucEditClientsProfileSidebar.xaml
    /// </summary>
    public partial class ucEditClientsProfileSidebar : UserControl
    {
        private ClientDTO _selectedClient { get; set; }
        public ucShowClientsProfileSidebar Parent { get; set; }

        private IClientService _clientService;
        public ucEditClientsProfileSidebar(ClientDTO selectedClient)
        {
            InitializeComponent();
            _selectedClient = selectedClient;
            _clientService = new ClientService();
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
            textRewardPoints.Text = _selectedClient.Points.ToString();
            textLoyaltyLevel.Text = _selectedClient.LoyaltyLevel;
            textGiftCardDesc.Text = _selectedClient.GiftCardDescription;
            textReservationDates.Text = _selectedClient.ReservationsDates;
            textReviewsComments.Text = _selectedClient.ReviewsComments;
        }

        private void textFirstname_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtFirstname.Focus();
        }

        private void txtFirstname_TextChanged(object sender, TextChangedEventArgs e)
        {
            var firstName = txtFirstname.Text;
            var placeholderFirstName = textFirstname;

            if (!string.IsNullOrEmpty(firstName) && firstName.Length >= 0)
            {
                placeholderFirstName.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderFirstName.Text = _selectedClient.Firstname;
                placeholderFirstName.Visibility = Visibility.Visible;
            }

            if (!IsLettersOnly(firstName))
            {
                if (string.IsNullOrEmpty(firstName)) return;
                txtLastname.Clear();
                MessageBox.Show("Firstname must contain only letters.");
                return;
            }
        }

        private void textLastname_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtLastname.Focus();
        }

        private void txtLastname_TextChanged(object sender, TextChangedEventArgs e)
        {
            var lastName = txtLastname.Text;
            var placeholderLastName = textLastname;

            if (!string.IsNullOrEmpty(lastName) && lastName.Length >= 0)
            {
                placeholderLastName.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderLastName.Text = _selectedClient.Lastname;
                placeholderLastName.Visibility = Visibility.Visible;
            }

            if (!IsLettersOnly(lastName))
            {
                if (string.IsNullOrEmpty(lastName)) return;
                txtLastname.Clear();
                MessageBox.Show("Lastname must contain only letters.");
                return;
            }
        }

        private void textEmail_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtEmail.Focus();
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            var email = txtEmail.Text;
            var placeholderEmail = textEmail;

            if (!string.IsNullOrEmpty(email) && email.Length >= 0)
            {
                placeholderEmail.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderEmail.Visibility = Visibility.Visible;
            }
        }

        private void textPhoneNumber_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtPhoneNumber.Focus();
        }

        private void txtPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            var telephone = txtPhoneNumber.Text;
            var placeholderTelephone = textPhoneNumber;

            if (!string.IsNullOrEmpty(telephone) && telephone.Length >= 0)
            {
                placeholderTelephone.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderTelephone.Text = _selectedClient.PhoneNumber;
                placeholderTelephone.Visibility = Visibility.Visible;
            }

            if (!IsValidTelephone(telephone))
            {
                if (string.IsNullOrWhiteSpace(telephone)) return;
                txtPhoneNumber.Clear();
                MessageBox.Show("Please enter a valid phone number!");
                return;
            }
        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            var ucClientAdministration = Parent.Parent;
            ucClientAdministration.CloseSidebar();
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var firstname = string.IsNullOrWhiteSpace(txtFirstname.Text) ? _selectedClient.Firstname : txtFirstname.Text;
                var lastname = string.IsNullOrWhiteSpace(txtLastname.Text) ? _selectedClient.Lastname : txtLastname.Text;
                var email = string.IsNullOrWhiteSpace(txtEmail.Text) ? _selectedClient.Email : txtEmail.Text;
                var phoneNumber = string.IsNullOrWhiteSpace(txtPhoneNumber.Text) ? _selectedClient.PhoneNumber : txtPhoneNumber.Text;

                var clientDTO = new ClientDTO
                {
                    Id = _selectedClient.Id,
                    Firstname = firstname,
                    Lastname = lastname,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Points = _selectedClient.Points,
                    GiftCardDescription = _selectedClient.GiftCardDescription,
                    ReservationsDates = _selectedClient.ReservationsDates,
                    ReviewsComments = _selectedClient.ReviewsComments
                };

                await _clientService.UpdateClientAsync(clientDTO);
                LoadNewClientsProfile(clientDTO);
            } catch (ClientNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Client Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LoadNewClientsProfile(ClientDTO clientDTO)
        {
            var ucClientAdministration = Parent.Parent;
            ucClientAdministration.RefreshGui();

            ucClientAdministration.dgvClients.Dispatcher.InvokeAsync(async () =>
            {
                await Task.Delay(50);

                var clients = ucClientAdministration.dgvClients.ItemsSource as IEnumerable<ClientDTO>;
                if (clients != null)
                {
                    var selectedClient = clients.FirstOrDefault(c => c.Id == clientDTO.Id);
                    if (selectedClient != null)
                    {
                        ucClientAdministration.dgvClients.SelectedItem = selectedClient;
                        ucClientAdministration.dgvClients.ScrollIntoView(selectedClient);
                    }
                }
            });

            //osigurač
            ucClientAdministration.SwitchClient(clientDTO);
        }


        //Provjere
        private bool IsLettersOnly(string value)
        {
            return !string.IsNullOrEmpty(value) && value.All(c => char.IsLetter(c) || char.IsWhiteSpace(c) || c == '-');
        }

        private bool IsValidTelephone(string telephone)
        {
            return Regex.IsMatch(telephone, @"^[\+0-9\s]+$");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Parent.Parent.ccSidebar.Content = Parent;
        }
    }
}
