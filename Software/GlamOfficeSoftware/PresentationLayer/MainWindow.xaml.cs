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

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IClientService _clientService;

        public MainWindow()
        {
            InitializeComponent();
            _clientService = new ClientService();         
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadClients();
        }

        private async Task LoadClients()
        {
            dgvClients.ItemsSource = await _clientService.GetAllClientsAsync();
        }
    }
}
