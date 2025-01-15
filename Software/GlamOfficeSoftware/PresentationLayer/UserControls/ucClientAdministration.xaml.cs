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

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for ucClientAdministration.xaml
    /// </summary>
    public partial class ucClientAdministration : UserControl
    {
        private IClientService _clientService;

        public ucClientAdministration()
        {
            InitializeComponent();
            _clientService = new ClientService();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFilters();
        }

        private void LoadFilters()
        {
            //cmb fill
            cmbFilters.Items.Add("PIN");
            cmbFilters.Items.Add("First and lastname");
            cmbFilters.Items.Add("Email");
            cmbFilters.Items.Add("Phone number");

            cmbFilters.SelectedIndex = 1;
            cmbFilters.IsDropDownOpen = false;
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
                textSearch.Text = "Insert PIN...";
            } else if (cmbFilters.SelectedIndex == 1)
            {
                textSearch.Text = "Insert first and lastname...";
            } else if (cmbFilters.SelectedIndex == 2)
            {
                textSearch.Text = "Insert e-mail...";
            } else if (cmbFilters.SelectedIndex == 3)
            {
                textSearch.Text = "Insert phone number...";
            }
        }

        private void textSearch_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtSearch.Focus();
        }

        private async void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var placeholderSearch = textSearch;
            var pattern = txtSearch.Text.ToLower();

            if (!string.IsNullOrEmpty(pattern) && pattern.Length >= 0)
            {
                placeholderSearch.Visibility = Visibility.Collapsed;
                //UpdateData();
            } else
            {
                placeholderSearch.Visibility = Visibility.Visible;
                dgvClients.ItemsSource = await _clientService.GetAllClientsAsync();
                //HideColumns();
            }
        }
    }
}
