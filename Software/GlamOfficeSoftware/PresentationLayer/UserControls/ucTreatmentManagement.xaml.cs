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
    /// Interaction logic for ucTreatmentManagement.xaml
    /// </summary>
   

    public partial class ucTreatmentManagement : UserControl
    {
        private TreatmentService _treatmentService = new TreatmentService();
        public MainWindow Parent { get; set; }
        private  ucAddNewTreatmentSidebar _addNewTreatmentSidebar;
        private TreatmentDTO _selectedTreatment;
        public ucTreatmentManagement()
        {

            InitializeComponent();
            _addNewTreatmentSidebar = new ucAddNewTreatmentSidebar();
        }

        private void LoadFilters()
        {
            cmbFilters.Items.Add("Search");
            cmbFilters.Items.Add("Treatment group");
            cmbFilters.Items.Add("Work position");

            cmbFilters.SelectedIndex = 0;
            cmbFilters.IsDropDownOpen = false;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           
            LoadFilters();
            LoadSortingCriteria();
            ShowLoadingIndicator(true);
            await LoadDataGrid();
            ShowLoadingIndicator(false);
        }

        private void btnDropdown_Click(object sender, RoutedEventArgs e)
        {
            cmbFilters.IsDropDownOpen = true;
            
        }
        private void LoadSortingCriteria()
        {
            cmbSorting.Items.Add("Name");
            cmbSorting.Items.Add("Price");
            cmbSorting.Items.Add("Duration");

            cmbSorting.SelectedIndex = 0;
            cmbSorting.IsDropDownOpen = false;
        }

        private void btnDropdownSorting_Click(object sender, RoutedEventArgs e)
        {
            cmbSorting.IsDropDownOpen = true;
        }
        private async Task LoadDataGrid()
        {
           
            var treatments = await Task.Run(() => _treatmentService.GetAllTreatmentsAsync());
            dgvTreatments.ItemsSource = treatments;
           
        }

        private async void cmbFilters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFilters.SelectedItem == null)
                return;

            ComboBoxItem selectedItem = cmbFilters.SelectedItem as ComboBoxItem;

            string selectedFilter = cmbFilters.SelectedItem.ToString();


            if (string.IsNullOrEmpty(selectedFilter))
                return;

            if (selectedFilter == "Search")
            {
                txtSearch.Visibility = Visibility.Visible;
                cmbFilterValues.Visibility = Visibility.Collapsed;
                btnDropdownSearch.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnDropdownSearch.Visibility = Visibility.Visible;
                txtSearch.Visibility = Visibility.Collapsed;
                cmbFilterValues.Visibility = Visibility.Visible;
                await LoadFilterValues(selectedFilter);
            }
        }

        private async Task LoadFilterValues(string filterType)
        {
            cmbFilterValues.Items.Clear();

            if (filterType == "Treatment group")
            {
                var groups = await _treatmentService.GetAllTreatmentGroupsAsync();
                foreach (var group in groups)
                {
                    cmbFilterValues.Items.Add(new ComboBoxItem { Content = group.Name, Tag = group.idTreatmentGroup });
                }
            }
            else if (filterType == "Work position")
            {
                var positions = await _treatmentService.GetAllWorkPositionsAsync();
                foreach (var position in positions)
                {
                    cmbFilterValues.Items.Add(new ComboBoxItem { Content = position.Name, Tag = position.idWorkPosition });
                }
            }

            cmbFilterValues.SelectedIndex = 0;
        }

        private async void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                await LoadDataGrid();
                return;
            }
            loadingIndicator.Visibility = Visibility.Visible;
            dgvTreatments.Visibility = Visibility.Collapsed;

            var filteredTreatments = await _treatmentService.GetTreatmentByNameAsync(txtSearch.Text);

            dgvTreatments.ItemsSource = filteredTreatments;
            loadingIndicator.Visibility = Visibility.Collapsed;
            dgvTreatments.Visibility = Visibility.Visible;
        }

        private async void cmbFilterValues_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFilterValues.SelectedItem == null)
                return;
            loadingIndicator.Visibility = Visibility.Visible;
            dgvTreatments.Visibility = Visibility.Collapsed;

            string selectedFilter = cmbFilters.SelectedItem.ToString(); 
            int selectedId = (int)((ComboBoxItem)cmbFilterValues.SelectedItem).Tag; 

            if (selectedFilter == "Treatment group")
            {
                dgvTreatments.ItemsSource = await _treatmentService.GetTreatmentsByGroupAsync(selectedId);
            }
            else if (selectedFilter == "Work position")
            {
                dgvTreatments.ItemsSource = await _treatmentService.GetTreatmentsByWorkPositionAsync(selectedId);
            }
            loadingIndicator.Visibility = Visibility.Collapsed;
            dgvTreatments.Visibility = Visibility.Visible;
        }

        private void btnDropdownSearch_Click(object sender, RoutedEventArgs e)
        {
            cmbFilterValues.IsDropDownOpen = true;
        }

        private void cmbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            if (cmbSorting.SelectedItem != null)
            {
                SortTreatments();
            }
        

        }
        private bool isAscending = true;
        private string lastSortCriterion = "";

        private void SortTreatments()
        {
            if (dgvTreatments.ItemsSource == null || !dgvTreatments.ItemsSource.Cast<TreatmentDTO>().Any())
                return;

            string selectedSort = cmbSorting.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedSort)) return;

          

            if (selectedSort != lastSortCriterion)
            {
                isAscending = true;
            }
            else
            {
                isAscending = !isAscending; 
            }

            lastSortCriterion = selectedSort;

            List<TreatmentDTO> sortedList = dgvTreatments.ItemsSource.Cast<TreatmentDTO>().ToList();

            switch (selectedSort)
            {
                case "Name":
                    sortedList = isAscending ? sortedList.OrderBy(t => t.Name).ToList()
                                             : sortedList.OrderByDescending(t => t.Name).ToList();
                    break;
                case "Price":
                    sortedList = isAscending ? sortedList.OrderBy(t => t.Price ?? 0).ToList()
                                             : sortedList.OrderByDescending(t => t.Price ?? 0).ToList();
                    break;
                case "Duration":
                    sortedList = isAscending ? sortedList.OrderBy(t => t.DurationMinutes ?? 0).ToList()
                                             : sortedList.OrderByDescending(t => t.DurationMinutes ?? 0).ToList();
                    break;
                default:
                    return;
            }

            Console.WriteLine($"Sorting applied: {selectedSort}, Order: {(isAscending ? "Ascending" : "Descending")}");

            dgvTreatments.ItemsSource = null;
            dgvTreatments.ItemsSource = sortedList;
        }


        private void cmbSorting_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SortTreatments();
        }

        private void btnShowTreatmentInfo_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTreatment != null)
            {
                var detailsSidebar = new ucShowTreatmentSidebar();
                detailsSidebar.ParentControl = this;
                ccSidebar.Content = null;
                ShowTreatmentDetailsSidebar(_selectedTreatment);
            }
            else
            {
                MessageBox.Show("Please select a treatment first.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void btnAddTreatment_Click(object sender, RoutedEventArgs e)
        {
            if (!(ccSidebar.Content is ucAddNewTreatmentSidebar))
            {
                _addNewTreatmentSidebar = new ucAddNewTreatmentSidebar();
                _addNewTreatmentSidebar.ParentControl = this;

                ccSidebar.Content = _addNewTreatmentSidebar;
                ShowSidebar();
            }
            else
            {
                CloseSidebar();
            }
        }



        internal async void RefreshDataGrid()
        {
            loadingIndicator.Visibility = Visibility.Visible;
            dgvTreatments.Visibility = Visibility.Collapsed;

            var treatments = await _treatmentService.GetAllTreatmentsAsync();
            dgvTreatments.ItemsSource = treatments;

            loadingIndicator.Visibility = Visibility.Collapsed;
            dgvTreatments.Visibility = Visibility.Visible;
        }

        private void dgvTreatments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvTreatments.SelectedItem is TreatmentDTO selectedTreatment)
            {
                _selectedTreatment = selectedTreatment; 
            }
        }
        private void ShowTreatmentDetailsSidebar(TreatmentDTO treatment)
        {
            if (treatment == null) return;

            if (!(ccSidebar.Content is ucShowTreatmentSidebar detailsSidebar))
            {
                detailsSidebar = new ucShowTreatmentSidebar();
                detailsSidebar.ParentControl = this;
                detailsSidebar.SetTreatmentDetails(treatment);

                ccSidebar.Content = detailsSidebar;
                ShowSidebar(detailsSidebar);
            }
        }

        internal async void CloseSidebar()
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

            await Task.Delay(500);
            ccSidebar.Content = null;
        }
        internal void ShowSidebar()
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
        internal void ShowSidebar(UserControl sidebar)
        {
            if (sidebar == null)
                return;

            ccSidebar.Content = sidebar;  
            sidebar.Visibility = Visibility.Visible;

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

        private void ShowLoadingIndicator(bool isLoading)
        {
            if (isLoading)
            {
                loadingIndicator.Visibility = Visibility.Visible;
                dgvTreatments.Visibility = Visibility.Collapsed;
            }
            else
            {
                loadingIndicator.Visibility = Visibility.Collapsed;
                dgvTreatments.Visibility = Visibility.Visible;
            }
        }

        private void btnShowTreatmentGroups_Click(object sender, RoutedEventArgs e)
        {
            if (!(ccSidebar.Content is ucTreatmentsGroupSidebar))
            {
                var groupsSidebar = new ucTreatmentsGroupSidebar();
                groupsSidebar.ParentControl = this;

                ccSidebar.Content = groupsSidebar;
                ShowSidebar();
            }
            else
            {
                CloseSidebar();
            }
        }
    }
}
