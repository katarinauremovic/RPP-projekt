using BusinessLogicLayer.Services;
using DataAccessLayer.Repositories;
using EntityLayer.Entities;
using PresentationLayer.Windows;
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
    /// Interaction logic for ucPromotionCreating.xaml
    /// </summary>
    public partial class ucPromotionCreating : UserControl
    {
        public MainWindow Parent { get; set; }
        private PromotionEmailService _promotionEmailService = new PromotionEmailService();
        private readonly ClientService _clientService = new ClientService();

        public ucPromotionCreating()
        {
            InitializeComponent();
        }

        private async void btnSendEmail_Click(object sender, RoutedEventArgs e)
        {
            if (rtbEmailPreview.Document.Blocks.Count == 0)
            {
                MessageBox.Show("Please generate the email first.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirmationBox = new winMessageBox();
            bool result = await confirmationBox.ShowAsync("Confirm Sending", "Are you sure you want to send this email to all clients?");
            if (!result) return;

            var clients = await _clientService.GetAllClientsDTOAsync();

            if (clients == null || !clients.Any())
            {
                MessageBox.Show("No clients found in the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            txtSendingStatus.Visibility = Visibility.Visible;

            foreach (var client in clients)
            {
                if (string.IsNullOrWhiteSpace(client.Email))
                    continue;

                await _promotionEmailService.SendPromotionalEmailAsync(
                    client.Email,
                    client.Firstname, 
                    txtPromotionName.Text,
                    txtAmount.Text,
                    txtDescription.Text,
                    dpEndDate.SelectedDate.Value.ToShortDateString()
                );

                await Task.Delay(100);
            }
            txtSendingStatus.Visibility = Visibility.Collapsed;

            MessageBox.Show("Emails sent successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

  
        private async void btnGenerateEmail_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPromotionName.Text) ||
         string.IsNullOrWhiteSpace(txtAmount.Text) ||
         string.IsNullOrWhiteSpace(txtDescription.Text) ||
         dpEndDate.SelectedDate == null)
            {
                MessageBox.Show("Please fill in all fields!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var clients = await _clientService.GetAllClientsDTOAsync();
            string clientName = clients.Any() ? clients.First().Firstname : "Valued Customer";

            string emailBody = _promotionEmailService.GenerateEmailBody(
                clientName, 
                txtPromotionName.Text,
                txtAmount.Text,
                txtDescription.Text,
                dpEndDate.SelectedDate.Value.ToShortDateString());

            rtbEmailPreview.Document.Blocks.Clear();
            rtbEmailPreview.Document.Blocks.Add(new Paragraph(new Run(emailBody)));
        }

        private void btnApplyDiscount_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
