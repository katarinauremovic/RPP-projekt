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
    /// Interaction logic for ucEmployeeAdministration.xaml
    /// </summary>
    public partial class ucEmployeeAdministration : UserControl
    {
        public MainWindow Parent { get; set; }
        public ucEmployeeAdministration()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            LoadFilters();
            LoadSortingList();
        }

       

        private void LoadSortingList()
        {
            cmbSortingList.Items.Add("Name ascending");
            cmbSortingList.Items.Add("Name descending");
            cmbSortingList.Items.Add("Surname ascending");
            cmbSortingList.Items.Add("Surname descending");

            cmbSortingList.SelectedIndex = 0;
            cmbSortingList.IsDropDownOpen = false;
        }

        private void LoadFilters()
        {
            cmbFilters.Items.Add("Name");
            cmbFilters.Items.Add("Surname");
            cmbFilters.Items.Add("Key word");
            cmbFilters.Items.Add("Work position");
            cmbFilters.Items.Add("Role");

            cmbFilters.SelectedIndex = 0;
            cmbFilters.IsDropDownOpen = false;
        }

        private void btnDropdownMenu_Click(object sender, RoutedEventArgs e)
        {
            cmbFilters.IsDropDownOpen = true;
        }

        private void cmbFilters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            

            cmbFilters.IsDropDownOpen = true; 

            switch (cmbFilters.SelectedIndex)
            {
                case 0:
                    textSearch.Text = "Insert first name...";
                    break;
                case 1:
                    textSearch.Text = "Insert surname...";
                    break;
                case 2:
                    textSearch.Text = "Insert key word...";
                    break;
                case 3:
                    textSearch.Text = "Insert work position...";
                    break;
                case 4:
                    textSearch.Text = "Insert role...";
                    break;
            }
        }
        private void ShowLoadingIndicator(bool isLoading)
        {
            if (isLoading)
            {
                loadingIndicator.Visibility = Visibility.Visible;
                dgvEmployees.Visibility = Visibility.Collapsed;
            }
            else
            {
                loadingIndicator.Visibility = Visibility.Collapsed;
                dgvEmployees.Visibility = Visibility.Visible;
            }
        }

        private void btnDropdownList_Click(object sender, RoutedEventArgs e)
        {
            cmbSortingList.IsDropDownOpen = true;
        }

        private void cmbSortingList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void btnShowEmployeesDetails_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnAddNewEmployee_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgvEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void textSearch_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtSearch.Focus();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var pattern = txtSearch.Text.ToLower();

            if (!string.IsNullOrEmpty(pattern))
            {
                textSearch.Visibility = Visibility.Collapsed;
            }
            else
            {
                textSearch.Visibility = Visibility.Visible;
            }
        }

        private void cmbFilters_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void cmbSortingList_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        
    }
}
