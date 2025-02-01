using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using EntityLayer.DTOs;
using EntityLayer.Entities;
using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Lifetime;
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
    /// Interaction logic for ucClientRewards.xaml
    /// </summary>
    public partial class ucClientRewards : UserControl
    {
        public ObservableCollection<RewardDTO> Rewards { get; set; }

        public ucRewards Parent { get; set; }

        private IClientService _clientService;

        private RewardSystem _rewardSystem;

        public ucClientRewards()
        {
            InitializeComponent();
            _clientService = new ClientService();
            _rewardSystem = new RewardSystem();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFilters();
            ShowLoadingClientsIndicator(true);
            await LoadClientsAsync();
            ShowLoadingClientsIndicator(false);
        }

        private void btnDropdown_Click(object sender, RoutedEventArgs e)
        {
            cmbFilters.IsDropDownOpen = true;
        }

        private void cmbFilters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbFilters.IsDropDownOpen = true;
            if (cmbFilters.SelectedIndex == 0)
            {
                textSearch.Text = "Insert first and lastname...";
            } else if (cmbFilters.SelectedIndex == 1)
            {
                textSearch.Text = "Insert e-mail...";
            } else if (cmbFilters.SelectedIndex == 2)
            {
                textSearch.Text = "Insert phone number...";
            }
        }

        private void textSearch_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                txtSearch.Focus();
            }), System.Windows.Threading.DispatcherPriority.Input);
        }

        private async void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var placeholderSearch = textSearch;
            var pattern = txtSearch.Text.ToLower();

            if (!string.IsNullOrEmpty(pattern) && pattern.Length >= 0)
            {
                placeholderSearch.Visibility = Visibility.Collapsed;
                SearchingByFilter();
            } else
            {
                placeholderSearch.Visibility = Visibility.Visible;
                await LoadClientsAsync();
            }
        }

        private async void dgvClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvClients.SelectedItem == null) return;

            ShowLoadingRewardsIndicator(true);
            loadingIndicatorRewards.Text = "Loading rewards, please wait...";

            var selectedClient = dgvClients.SelectedItem as ClientDTO;
            
            await LoadRewardsForSelectedClient(selectedClient.Id);

            txtClientsPoints.Text = selectedClient.Points.ToString();
        }

        private async Task LoadRewardsForSelectedClient(int clientId)
        {
            Rewards = new ObservableCollection<RewardDTO>(await Task.Run(() => _rewardSystem.GetRewardsDtoForClientAsync(clientId)));
            rewardsItemsControl.ItemsSource = Rewards;
            HaveRewards(Rewards.Count);
        }

        public async Task RefreshGui(int clientId)
        {
            var clients = await Task.Run(() => _clientService.GetAllClientsDTOAsync());
            var selectedClient = clients.FirstOrDefault(c => c.Id == clientId);

            if (selectedClient != null)
            {
                dgvClients.ItemsSource = clients;
                await Task.Delay(100);
                dgvClients.SelectedItem = selectedClient;
                txtClientsPoints.Text = selectedClient.Points.ToString();
                await LoadRewardsForSelectedClient(clientId);
            }
        }

        private void HaveRewards(int count)
        {
            if (count == 0)
            {
                loadingIndicatorRewards.Text = "No rewards available for this client.";
                ShowLoadingRewardsIndicator(true);
            } else
            {
                ShowLoadingRewardsIndicator(false);
            }
        }

        private async Task LoadClientsAsync()
        {
            try
            {
                var clients = await Task.Run(() => _clientService.GetAllClientsDTOAsync());
                dgvClients.ItemsSource = clients;
            } catch (ClientOperationException ex)
            {
                throw new ClientOperationException("Failed to load clients.", ex);
            }
        }

        private void ShowLoadingClientsIndicator(bool isLoading)
        {
            if (isLoading)
            {
                loadingIndicator.Visibility = Visibility.Visible;
                dgvClients.Visibility = Visibility.Collapsed;
            } else
            {
                loadingIndicator.Visibility = Visibility.Collapsed;
                dgvClients.Visibility = Visibility.Visible;
            }
        }

        private void ShowLoadingRewardsIndicator(bool isLoading)
        {
            if (isLoading)
            {
                loadingIndicatorRewards.Visibility = Visibility.Visible;
                rewardsItemsControl.Visibility = Visibility.Collapsed;
            } else
            {
                loadingIndicatorRewards.Visibility = Visibility.Collapsed;
                rewardsItemsControl.Visibility = Visibility.Visible;
            }
        }

        private void LoadFilters()
        {
            cmbFilters.Items.Add("First and lastname");
            cmbFilters.Items.Add("Email");
            cmbFilters.Items.Add("Phone number");

            cmbFilters.SelectedIndex = 0;
            cmbFilters.IsDropDownOpen = false;
        }

        private async void SearchingByFilter()
        {
            var pattern = txtSearch.Text.ToLower();
            var selectedItem = cmbFilters.SelectedIndex;

            if (string.IsNullOrEmpty(pattern))
            {
                ShowLoadingClientsIndicator(true);
                await LoadClientsAsync();
                ShowLoadingClientsIndicator(false);
                return;
            }

            try
            {
                ShowLoadingClientsIndicator(true);
                switch (selectedItem)
                {
                    case 0:
                        dgvClients.ItemsSource = await Task.Run(() => _clientService.GetClientsByFirstAndLastNamePattern(pattern));
                        break;
                    case 1:
                        dgvClients.ItemsSource = await Task.Run(() => _clientService.GetClientsByEmailPattern(pattern));
                        break;
                    case 2:
                        dgvClients.ItemsSource = await Task.Run(() => _clientService.GetClientsByPhoneNumberPattern(pattern));
                        break;
                    default:
                        await LoadClientsAsync();
                        break;
                }
            } catch (Exception ex)
            {
                MessageBox.Show($"Failed to search clients: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } finally
            {
                ShowLoadingClientsIndicator(false);
            }
        }
    }
}
