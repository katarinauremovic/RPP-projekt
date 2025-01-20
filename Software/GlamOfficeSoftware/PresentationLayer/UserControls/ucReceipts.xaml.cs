using BusinessLogicLayer.Exceptions;
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
    /// Interaction logic for ucReceipts.xaml
    /// </summary>
    public partial class ucReceipts : UserControl
    {
        public MainWindow Parent { get; set; }
        
        private IReceiptService _receiptService;
        public ucReceipts()
        {
            InitializeComponent();
            _receiptService = new ReceiptService();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ShowLoadingIndicator(true);
            await LoadReceiptsAsync();
            ShowLoadingIndicator(false);
        }

        private async Task LoadReceiptsAsync()
        {
            try
            {
                var receipts = await Task.Run(() => _receiptService.GetAllReceiptsDTOAsync());
                dgvReceipts.ItemsSource = receipts;
            } catch (ClientOperationException ex)
            {
                throw new ClientOperationException("Failed to load receipts.", ex);
            }
        }

        private void ShowLoadingIndicator(bool isLoading)
        {
            if (isLoading)
            {
                loadingIndicator.Visibility = Visibility.Visible;
                dgvReceipts.Visibility = Visibility.Collapsed;
            } else
            {
                loadingIndicator.Visibility = Visibility.Collapsed;
                dgvReceipts.Visibility = Visibility.Visible;
            }
        }
    }
}
