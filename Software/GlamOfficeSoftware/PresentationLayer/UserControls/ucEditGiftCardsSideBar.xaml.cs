using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Services;
using EntityLayer.Entities;
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
    /// Interaction logic for ucEditGiftCardsSideBar.xaml
    /// </summary>
    public partial class ucEditGiftCardsSideBar : UserControl
    {
        public ucGiftCardAdministration Parent { get; set; }

        private GiftCardService _giftCardService;

        private ClientService _clientService;

        private GiftCard _giftCard;
        public ucEditGiftCardsSideBar(GiftCard giftCard)
        {
            InitializeComponent();
            _giftCardService = new GiftCardService();
            _clientService = new ClientService();
            _giftCard = giftCard;
            LoadGiftCardDetails();
            LoadStatuses();
            LoadAllClients();


        }

        private async void LoadAllClients()
        {
            cboClients.Items.Clear();
            var clients = await _clientService.GetAllClientsAsync(); 
            foreach (var client in clients)
            {
                string fullName = $"{client.Firstname} {client.Lastname}";
                cboClients.Items.Add(fullName);
            }
        }

        private void LoadStatuses()
        {
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("Active");
            cmbStatus.Items.Add("Redeemed");
            cmbStatus.Items.Add("Expired");
        }

        private void LoadGiftCardDetails()
        {
            if (_giftCard != null)
            {
                txtValue.Text = _giftCard.Value.ToString();
                txtAmountToSpend.Text = _giftCard.ToSpend.ToString();
                txtDescription.Text = _giftCard.Description;
                txtPromoCode.Text = _giftCard.PromoCode;
                if (_giftCard.ActivationDate.HasValue)
                    dpActivationDate.SelectedDate = _giftCard.ActivationDate.Value;

                if (_giftCard.ExpirationDate.HasValue)
                    dpExpirationDate.SelectedDate = _giftCard.ExpirationDate.Value;

                if (_giftCard.RedemptionDate.HasValue)
                    dpRedemptionDate.SelectedDate = _giftCard.RedemptionDate.Value;

                cmbStatus.SelectedItem = _giftCard.Status;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddGiftCardToClient_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancelAdding_Click(object sender, RoutedEventArgs e)
        {
            Parent.CloseSideBarMenu();
        }

        private void btnAddNewGiftCard_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void btnsaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (_giftCard == null)
            {
                MessageBox.Show("Error: No gift card selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                
                _giftCard.Value = decimal.TryParse(txtValue.Text, out decimal value) ? value : 0;
                _giftCard.ToSpend = decimal.TryParse(txtAmountToSpend.Text, out decimal amount) ? amount : 0;
                _giftCard.Description = txtDescription.Text;
                _giftCard.PromoCode = txtPromoCode.Text;

                
                _giftCard.ActivationDate = dpActivationDate.SelectedDate ?? _giftCard.ActivationDate;
                _giftCard.ExpirationDate = dpExpirationDate.SelectedDate ?? _giftCard.ExpirationDate;
                _giftCard.RedemptionDate = dpRedemptionDate.SelectedDate ?? _giftCard.RedemptionDate;


                if (cmbStatus.SelectedItem != null)
                {
                    _giftCard.Status = cmbStatus.SelectedItem.ToString();
                }
 
                await _giftCardService.UpdateGiftCardAsync(_giftCard);
                
                await AssignGiftCardToClientAsync();
                
                Parent.CloseSideBarMenu();
            }
            catch (UnsuccessfulOperationException ex)
            {
                MessageBox.Show($"Error saving changes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Parent.RefreshGui();
        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            Parent.CloseSideBarMenu();
        }

        private void btnAddToClient_Click(object sender, RoutedEventArgs e)
        {
            spClients.Visibility = Visibility.Visible;
           
        }

        private async Task AssignGiftCardToClientAsync()
        {
            if (cboClients.SelectedItem == null)
            {
                
                return;
            }

            
            string selectedClientName = cboClients.SelectedItem.ToString();

            
            var clients = await _clientService.GetAllClientsAsync();
            var selectedClient = clients.FirstOrDefault(c => $"{c.Firstname} {c.Lastname}" == selectedClientName);

            if (selectedClient == null)
            {
                MessageBox.Show("Selected client was not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                await _clientService.AssignGiftCardToClientAsync(selectedClient.idClient, _giftCard.idGiftCard);
                MessageBox.Show("Gift card assigned successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (ClientNotFoundException ex) 
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (UnsuccessfulOperationException ex)
            {
                MessageBox.Show($"Error assigning gift card: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
