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
            await _receiptService.GenerateReceiptPdf(_selectedReceipt);
        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnSaveToPdf_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnVoid_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
