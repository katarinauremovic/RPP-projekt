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
    /// Interaction logic for ucClientAdministration.xaml
    /// </summary>
    public partial class ucClientAdministration : UserControl
    {
        public ucClientAdministration()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFilters();
        }

        private void LoadFilters()
        {
            //cmb fill
            cmbFilters.Items.Add("PIN");
            cmbFilters.Items.Add("First and lastname");
            cmbFilters.Items.Add("Email");
            cmbFilters.Items.Add("Phone number");

            cmbFilters.SelectedIndex = 1;
            cmbFilters.IsDropDownOpen = false;
        }

        private void btnDropdown_Click(object sender, RoutedEventArgs e)
        {
            cmbFilters.IsDropDownOpen = true;
        }

        private void textSearch_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void cmbFilters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbFilters.IsDropDownOpen = true;
            if (cmbFilters.SelectedIndex == 0)
            {
                textSearch.Text = "Insert PIN...";
            } else if (cmbFilters.SelectedIndex == 1)
            {
                textSearch.Text = "Insert first and lastname...";
            } else if (cmbFilters.SelectedIndex == 2)
            {
                textSearch.Text = "Insert e-mail...";
            } else if (cmbFilters.SelectedIndex == 3)
            {
                textSearch.Text = "Insert phone number...";
            }
        }
    }
}
