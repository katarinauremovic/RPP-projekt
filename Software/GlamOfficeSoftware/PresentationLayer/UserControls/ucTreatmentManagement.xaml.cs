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
    /// Interaction logic for ucTreatmentManagement.xaml
    /// </summary>
   

    public partial class ucTreatmentManagement : UserControl
    {
        private TreatmentService _treatmentService = new TreatmentService();
        public MainWindow Parent { get; set; }
        public ucTreatmentManagement()
        {
            InitializeComponent();
            LoadDataGrid();
        }

        private void LoadFilters()
        {
            cmbFilters.Items.Add("Search");
            cmbFilters.Items.Add("Treatment group");
            cmbFilters.Items.Add("Work position");

            cmbFilters.SelectedIndex = 0;
            cmbFilters.IsDropDownOpen = false;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFilters();
            LoadSortingCriteria();
            LoadDataGrid();
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
        private async void LoadDataGrid()
        {
            var treatments = await _treatmentService.GetAllTreatmentsAsync();
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
            }
            else
            {
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

    }
}
