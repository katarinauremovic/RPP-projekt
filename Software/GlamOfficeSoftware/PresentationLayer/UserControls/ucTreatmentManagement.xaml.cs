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
        public MainWindow Parent { get; set; }
        public ucTreatmentManagement()
        {
            InitializeComponent();
        }

        private void LoadFilters()
        {
            cmbFilters.Items.Add("Name");
            cmbFilters.Items.Add("Treatment group");
            cmbFilters.Items.Add("Work position");

            cmbFilters.SelectedIndex = 0;
            cmbFilters.IsDropDownOpen = false;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFilters();
            LoadSortingCriteria();
        }

        private void btnDropdown_Click(object sender, RoutedEventArgs e)
        {
            cmbFilters.IsDropDownOpen = true;
            
        }
        private void LoadSortingCriteria()
        {
            cmbSorting.Items.Add("Naziv");
            cmbSorting.Items.Add("Cijena");
            cmbSorting.Items.Add("Trajanje");

            cmbSorting.SelectedIndex = 0;
            cmbSorting.IsDropDownOpen = false;
        }

        private void btnDropdownSorting_Click(object sender, RoutedEventArgs e)
        {
            cmbSorting.IsDropDownOpen = true;
        }
    }
}
