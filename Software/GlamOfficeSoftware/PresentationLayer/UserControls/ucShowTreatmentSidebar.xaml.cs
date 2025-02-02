using BusinessLogicLayer.Services;
using EntityLayer.DTOs;
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
    /// Interaction logic for ucShowTreatmentSidebar.xaml
    /// </summary>
    public partial class ucShowTreatmentSidebar : UserControl
    {
        public ucTreatmentManagement Parent { get; set; }
        public ucTreatmentManagement ParentControl { get; internal set; }

        private TreatmentDTO _currentTreatment;

        public ucShowTreatmentSidebar()
        {
            InitializeComponent();
        }

        internal void SetTreatmentDetails(TreatmentDTO treatment)
        {
            _currentTreatment = treatment;
            textTreatmentName.Text = treatment.Name;
            textPrice.Text = treatment.Price.HasValue ? $"{treatment.Price.Value:0.00} €" : "N/A";
            textDuration.Text = treatment.DurationMinutes.HasValue ? $"{treatment.DurationMinutes} min" : "N/A";
            textDescription.Text = string.IsNullOrEmpty(treatment.Description) ? "No description available" : treatment.Description;
            textTreatmentGroup.Text = string.IsNullOrEmpty(treatment.TreatmentGroupName) ? "No group" : treatment.TreatmentGroupName;
            textWorkPosition.Text = string.IsNullOrEmpty(treatment.WorkPositionName) ? "No position" : treatment.WorkPositionName;
        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            ParentControl.CloseSidebar();
        }

        private async void btnDeleteTreatment_Click(object sender, RoutedEventArgs e)
        {
          

            var confirmationBox = new winMessageBox();
            bool result = await confirmationBox.ShowAsync("Confirm Deletion",
                                                          $"Are you sure you want to delete '{_currentTreatment.Name}'?");

            if (result) 
            {
                await new TreatmentService().DeleteTreatmentAsync(_currentTreatment.idTreatment);

               

                ParentControl?.RefreshDataGrid();
                ParentControl?.CloseSidebar();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnEditTreatment_Click(object sender, RoutedEventArgs e)
        {
            if (_currentTreatment == null)
            {
                MessageBox.Show("No treatment selected.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var editSidebar = new ucEditTreatmentSidebar();
            editSidebar.ParentControl = ParentControl;
            editSidebar.LoadTreatmentDetails(_currentTreatment);

            if (ParentControl != null)
            {
                ParentControl.ShowSidebar(editSidebar);
            }
            else
            {
                MessageBox.Show("ParentControl is null!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
