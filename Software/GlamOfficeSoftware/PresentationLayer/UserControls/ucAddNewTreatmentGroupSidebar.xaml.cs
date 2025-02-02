using BusinessLogicLayer.Exceptions;
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
    /// Interaction logic for ucAddNewTreatmentGroup.xaml
    /// </summary>
    public partial class ucAddNewTreatmentGroupSidebar : UserControl
    {
        private TreatmentService _treatmentService = new TreatmentService();
        public ucTreatmentManagement ParentControl { get; set; }

        public ucAddNewTreatmentGroupSidebar()
        {
            InitializeComponent();
        }

        private async void btnSaveGroup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtGroupName.Text))
                {
                    throw new EmptyFieldsForTreatmentsException("Group name cannot be empty.");
                }

                await _treatmentService.AddTreatmentGroupAsync(txtGroupName.Text);

                ParentControl?.CloseSidebar();
            }
            catch (EmptyFieldsForTreatmentsException ex)
            {
                MessageBox.Show(ex.Message, "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ParentControl?.CloseSidebar();
        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            ParentControl?.CloseSidebar();
        }
    }
}
