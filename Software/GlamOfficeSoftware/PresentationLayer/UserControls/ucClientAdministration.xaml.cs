using BusinessLogicLayer.Interfaces;
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
using System.Collections.ObjectModel;
using EntityLayer.Entities;
using System.ComponentModel;
using System.Windows.Media.Animation;
using EntityLayer.DTOs;
using BusinessLogicLayer.Exceptions;
using System.Runtime.CompilerServices;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for ucClientAdministration.xaml
    /// </summary>
    public partial class ucClientAdministration : UserControl
    {
        private IClientService _clientService;
        public MainWindow Parent { get; set; }

        public ucClientAdministration()
        {
            InitializeComponent();
            _clientService = new ClientService();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFilters();
            ShowLoadingIndicator(true);
            await LoadClientsAsync();
            ShowLoadingIndicator(false);
        }

        private void LoadFilters()
        {
            cmbFilters.Items.Add("First and lastname");
            cmbFilters.Items.Add("Email");
            cmbFilters.Items.Add("Phone number");

            cmbFilters.SelectedIndex = 0;
            cmbFilters.IsDropDownOpen = false;
        }

        private void btnDropdown_Click(object sender, RoutedEventArgs e)
        {
            cmbFilters.IsDropDownOpen = true;
            CloseSidebarMenu();
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

        private void textSearch_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtSearch.Focus();
            CloseSidebarMenu();
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

        public async void RefreshGui()
        {
            await LoadClientsAsync();
            await Task.Delay(1);
            txtSearch.Clear();
            cmbFilters.SelectedIndex = 0;
            cmbFilters.IsDropDownOpen = false;
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

        private void ShowLoadingIndicator(bool isLoading)
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

        private async void SearchingByFilter()
        {
            var pattern = txtSearch.Text.ToLower();
            var selectedItem = cmbFilters.SelectedIndex;

            if (string.IsNullOrEmpty(pattern))
            {
                ShowLoadingIndicator(true);
                await LoadClientsAsync();
                ShowLoadingIndicator(false);
                return;
            }

            try
            {
                ShowLoadingIndicator(true);
                switch (selectedItem)
                {
                    case 0:
                        dgvClients.ItemsSource = await Task.Run(() => _clientService.GetClientsByFirstAndLastNamePattern(pattern));
                        break;
                    case 1:
                        dgvClients.ItemsSource = await Task.Run(() => _clientService.GetClientsByEmailPattern(pattern));
                        break;
                    case 2:
                        dgvClients.ItemsSource = await Task.Run(() =>  _clientService.GetClientsByPhoneNumberPattern(pattern));
                        break;
                    default:
                        MessageBox.Show("Invalid filter selection.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        break;
                }
            } catch (Exception ex)
            {
                MessageBox.Show($"Failed to search clients: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } finally
            {
                ShowLoadingIndicator(false);
            }
        }

        private void btnShowClientsProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var client = GetClientFromDataGrid();
                SwitchClient(client);
                ShowSidebar();
            } catch (DataGridNoSelectionException ex)
            {
                MessageBox.Show(ex.Message, "Selection Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            } catch (ClientOperationException ex)
            {
                MessageBox.Show(ex.Message, "Client Operation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgvClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sidebarMenu = (FrameworkElement)ccSidebar.Content;

            if (sidebarMenu == null && Parent.ccSidebarMenu.Content != null)
            {
                CloseSidebarMenu();
            }

            try
            {
                var client = GetClientFromDataGrid();

                if (ccSidebar.Content != null)
                {
                    SwitchClient(client);
                }

            } catch (DataGridNoSelectionException ex)
            {
                MessageBox.Show(ex.Message, "Selection Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            } catch (ClientOperationException ex)
            {
                MessageBox.Show(ex.Message, "Client Operation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void btnAddClient_Click(object sender, RoutedEventArgs e)
        {
            var ucAddNewClientSidebar = new ucAddNewClientSidebar();
            ucAddNewClientSidebar.Parent = this;
            ccSidebar.Content = ucAddNewClientSidebar;
            ShowSidebar();

        }

        public async void CloseSidebar()
        {
            var slideOutAnimation = FindResource("SlideOutAnimation") as Storyboard;
            var sidebarMenu = (FrameworkElement)ccSidebar.Content;

            if (sidebarMenu != null)
            {
                slideOutAnimation?.Begin(sidebarMenu);

                slideOutAnimation.Completed += (s, e) =>
                {
                    ccSidebar.Content = null;
                    sidebarMenu.Visibility = Visibility.Collapsed;
                };
            }

            //kod prvog zatvaranja se otvara na selection changed! Sad više NE!
            await Task.Delay(500);
            ccSidebar.Content = null;
        }

        private void ShowSidebar()
        {
            var slideInAnimation = FindResource("SlideInAnimation") as Storyboard;
            var sidebarMenu = (FrameworkElement)ccSidebar.Content;

            if (sidebarMenu != null)
            {
                sidebarMenu.Visibility = Visibility.Visible;

                sidebarMenu.Margin = new Thickness(240, 0, 0, 0);

                var marginAnimation = new ThicknessAnimation
                {
                    From = new Thickness(240, 0, 0, 0),
                    To = new Thickness(0, 0, 0, 0),
                    Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
                };

                sidebarMenu.BeginAnimation(MarginProperty, marginAnimation);
            }
        }

        private void CloseSidebarMenu()
        {
            Parent.CloseSidebarMenu();
        }

        public ClientDTO GetClientFromDataGrid()
        {
            var client = dgvClients.SelectedItem as ClientDTO;
            if (client == null && ccSidebar.Content == null)
            {
                throw new DataGridNoSelectionException("No client selected in the DataGrid.");
            }
            return client;
        }


        public void SwitchClient(ClientDTO client)
        {
            try
            {
                if (client != null)
                {
                    try
                    {
                        var ucClientProfileSidebar = new ucShowClientsProfileSidebar(client);
                        ucClientProfileSidebar.Parent = this;
                        ccSidebar.Content = ucClientProfileSidebar;
                    } catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            } catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
