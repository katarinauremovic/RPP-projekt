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
        private IReceiptService _receiptService;

        public MainWindow Parent { get; set; }
       
        public ucReceipts()
        {
            InitializeComponent();
            _receiptService = new ReceiptService();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFilters();
            await RefreshGui();
        }

        public async Task RefreshGui()
        {
            ShowLoadingIndicator(true);
            await LoadReceiptsAsync();
            ShowLoadingIndicator(false);
        }

        private void LoadFilters()
        {
            cmbFilters.Items.Add("Receipt number");
            cmbFilters.Items.Add("Client");
            cmbFilters.Items.Add("Employee");

            cmbFilters.SelectedIndex = 0;
            cmbFilters.IsDropDownOpen = false;
        }

        private void cmbFilters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbFilters.IsDropDownOpen = true;
            if (cmbFilters.SelectedIndex == 0)
            {
                textSearch.Text = "Insert receipt number...";
            } else if (cmbFilters.SelectedIndex == 1)
            {
                textSearch.Text = "Insert clients first and lastname...";
            } else if (cmbFilters.SelectedIndex == 2)
            {
                textSearch.Text = "Insert employees first and lastname...";
            }
        }

        private void btnDropdown_Click(object sender, RoutedEventArgs e)
        {
            cmbFilters.IsDropDownOpen = true;
            Parent.CloseSidebarMenu();
        }


        private void textSearch_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtSearch.Focus();
            Parent.CloseSidebarMenu();
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
                await RefreshGui();
            }
        }

        private async void SearchingByFilter()
        {
            var pattern = txtSearch.Text.ToLower();
            var selectedItem = cmbFilters.SelectedIndex;

            if (string.IsNullOrEmpty(pattern))
            {
                ShowLoadingIndicator(true);
                await RefreshGui();
                ShowLoadingIndicator(false);
                return;
            }

            try
            {
                ShowLoadingIndicator(true);
                switch (selectedItem)
                {
                    case 0:
                        dgvReceipts.ItemsSource = await Task.Run(() => _receiptService.GetReceiptsByReceiptNumberPattrern(pattern));
                        break;
                    case 1:
                        dgvReceipts.ItemsSource = await Task.Run(() =>  _receiptService.GetReceiptsByClientsFirstAndLastNamePattern(pattern));
                        break;
                    case 2:
                        dgvReceipts.ItemsSource = await Task.Run(() =>  _receiptService.GetReceiptsByEmployeesFirstAndLastNamePattern(pattern));
                        break;
                    default:
                        MessageBox.Show("Invalid filter selection.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        break;
                }
            } catch (Exception ex)
            {
                MessageBox.Show($"Failed to search rececipts: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } finally
            {
                ShowLoadingIndicator(false);
            }
        }

        private void dgvReceipts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Parent.CloseSidebarMenu();

            var sidebarMenu = (FrameworkElement)ccSidebar.Content;

            if (sidebarMenu != null)
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

        private async void btnVoidReceipt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedReceipt = GetReceiptFromDataGrid();
                await Task.Run(() => _receiptService.VoidReceiptAsync(selectedReceipt.Id, true));
                await RefreshGui();
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
    }
}
