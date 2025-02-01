using BusinessLogicLayer.Services;
using EntityLayer.DTOs;
using EntityLayer.Entities;
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
    /// Interaction logic for ucAddNewTreatmentSidebar.xaml
    /// </summary>
    public partial class ucAddNewTreatmentSidebar : UserControl
    {
        private TreatmentService _treatmentService = new TreatmentService();
        public ucTreatmentManagement ParentControl { get; set; }

        public ucAddNewTreatmentSidebar()
        {
            InitializeComponent();
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Name is required!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedGroup = cmbTreatmentGroup.SelectedItem as TreatmentGroup;
            var selectedPosition = cmbWorkPosition.SelectedItem as WorkPosition;


            var treatmentDTO = new TreatmentDTO
            {
                Name = txtName.Text,
                Price = double.TryParse(txtPrice.Text, out double price) ? (double?)price : null,
                Description = txtDescription.Text,
                DurationMinutes = decimal.TryParse(txtDuration.Text, out decimal duration) ? (decimal?)duration : null,
                TreatmentGroupName = selectedGroup?.Name,
                WorkPositionName = selectedPosition?.Name
            };

            await _treatmentService.AddTreatmentAsync(treatmentDTO);

           

            ParentControl?.RefreshDataGrid(); 
            CloseSidebar();
        }

        private void CloseSidebar()
        {
            ParentControl.CloseSidebar();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ParentControl.CloseSidebar();

        }
        private async void LoadComboBoxes()
        {
            var groups = await _treatmentService.GetAllTreatmentGroupsAsync();
            cmbTreatmentGroup.ItemsSource = groups;
            cmbTreatmentGroup.DisplayMemberPath = "Name"; 
            cmbTreatmentGroup.SelectedValuePath = "idTreatmentGroup"; 

            var positions = await _treatmentService.GetAllWorkPositionsAsync();
            cmbWorkPosition.ItemsSource = positions;
            cmbWorkPosition.DisplayMemberPath = "Name";
            cmbWorkPosition.SelectedValuePath = "idWorkPosition";
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        { 
            LoadComboBoxes();
        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;

        }
    }
}
