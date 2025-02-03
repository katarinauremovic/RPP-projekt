using BusinessLogicLayer.Interfaces;
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
    /// Interaction logic for ucEditTreatmentSidebar.xaml
    /// </summary>
    public partial class ucEditTreatmentSidebar : UserControl
    {
        private readonly ITreatmentService _treatmentService = new TreatmentService();
        private TreatmentDTO _currentTreatment;
        public ucTreatmentManagement ParentControl { get; set; }

        public ucEditTreatmentSidebar()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            await SaveChangesAsync();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseSidebar();
        }
        public async void LoadTreatmentDetails(TreatmentDTO treatment)
        {
            _currentTreatment = treatment;

            txtName.Text = treatment.Name;
            txtPrice.Text = treatment.Price.HasValue ? treatment.Price.Value.ToString() : "";
            txtDuration.Text = treatment.DurationMinutes.HasValue ? treatment.DurationMinutes.Value.ToString() : "";
            txtDescription.Text = treatment.Description;

            await LoadComboBoxesAsync();

            cmbTreatmentGroup.SelectedItem = cmbTreatmentGroup.Items.Cast<TreatmentGroup>()
                .FirstOrDefault(g => g.Name == treatment.TreatmentGroupName);
            cmbWorkPosition.SelectedItem = cmbWorkPosition.Items.Cast<WorkPosition>()
                .FirstOrDefault(wp => wp.Name == treatment.WorkPositionName);
        }

        private async Task LoadComboBoxesAsync()
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
        private async Task SaveChangesAsync()
        {
            if (_currentTreatment == null) return;

            _currentTreatment.Name = txtName.Text;
            _currentTreatment.Price = double.TryParse(txtPrice.Text, out double price) ? (double?)price : null;
            _currentTreatment.DurationMinutes = decimal.TryParse(txtDuration.Text, out decimal duration) ? (decimal?)duration : null;
            _currentTreatment.Description = txtDescription.Text;

            _currentTreatment.TreatmentGroupName = (cmbTreatmentGroup.SelectedItem as TreatmentGroup)?.Name;
            _currentTreatment.WorkPositionName = (cmbWorkPosition.SelectedItem as WorkPosition)?.Name;

            await _treatmentService.UpdateTreatmentAsync(_currentTreatment);

            

            ParentControl?.LoadDataGridAsync();  
            CloseSidebar();
        }
        private void CloseSidebar()
        {
           ParentControl?.CloseSidebar();
        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            CloseSidebar();
        }
    }
}
