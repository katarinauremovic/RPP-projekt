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
using System.Windows.Media.Animation;
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
           await RefreshGui();
        }

        public async Task RefreshGui()
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

        private void btnShowReceipt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var receipt = GetReceiptFromDataGrid();
                SwitchReceipt(receipt);
                ShowSidebar();
            } catch (DataGridNoSelectionException ex)
            {  
                MessageBox.Show(ex.Message, "Selection Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            } catch (ClientOperationException ex)
            {
                MessageBox.Show(ex.Message, "Receipt Operation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private ReceiptDTO GetReceiptFromDataGrid()
        {
            var receipt = dgvReceipts.SelectedItem as ReceiptDTO;
            if (receipt == null && ccSidebar.Content == null)
            {
                throw new DataGridNoSelectionException("No receipt selected in the DataGrid.");
            }
            return receipt;
        }

        public void SwitchReceipt(ReceiptDTO receipt)
        {
            try
            {
                if (receipt != null)
                {
                    try
                    {
                        var ucShowReceiptSidebar = new ucShowReceiptSidebar(receipt);
                        ucShowReceiptSidebar.Parent = this;
                        ccSidebar.Content = ucShowReceiptSidebar;
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

        private async void btnVoidReceipt_Click(object sender, RoutedEventArgs e)
        {
            var selectedReceipt = GetReceiptFromDataGrid();
            await Task.Run(() => _receiptService.VoidReceiptAsync(selectedReceipt.Id, true));
            await RefreshGui();
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

        private void dgvReceipts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var receipt = GetReceiptFromDataGrid();

                if (ccSidebar.Content != null)
                {
                    SwitchReceipt(receipt); 
                }

            } catch (DataGridNoSelectionException ex)
            {
                MessageBox.Show(ex.Message, "Selection Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            } catch (ClientOperationException ex)
            {
                MessageBox.Show(ex.Message, "Receipt Operation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
