using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
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
    /// Interaction logic for ucShowReceiptSidebar.xaml
    /// </summary>
    public partial class ucShowReceiptSidebar : UserControl
    {
        private ReceiptDTO _selectedReceipt { get; set; }

        public ucReceipts Parent { get; set; }

        private IReceiptService _receiptService;

        public ucShowReceiptSidebar(ReceiptDTO receipt)
        {
            InitializeComponent();
            _selectedReceipt = receipt;
            _receiptService = new ReceiptService();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadReceiptAsync();
        }

        private async Task LoadReceiptAsync()
        {
            txtReceipt.Text = await Task.Run(() => _receiptService.GenerateReceiptInStringFormat(_selectedReceipt));
        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            Parent.CloseSidebar();
        }

        private async void btnSaveToPdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Run(() => _receiptService.GenerateReceiptPdf(_selectedReceipt));
            } catch (ClientOperationException ex)
            {
                MessageBox.Show(ex.Message, "Receipt Operation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnVoid_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Run(() => _receiptService.VoidReceiptAsync(_selectedReceipt.Id, true));
                await Parent.RefreshGui();
            } catch (DataGridNoSelectionException ex)
            {
                MessageBox.Show(ex.Message, "Selection Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            } catch (ClientOperationException ex)
            {
                MessageBox.Show(ex.Message, "Receipt Operation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (ReceiptNotVoidableException ex)
            {
                MessageBox.Show(ex.Message, "Void Receipt Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
