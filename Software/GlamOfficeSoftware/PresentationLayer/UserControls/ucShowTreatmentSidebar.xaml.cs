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
        public ucShowTreatmentSidebar()
        {
            InitializeComponent();
        }

        internal void SetTreatmentDetails(TreatmentDTO treatment)
        {
            textTreatmentName.Text = treatment.Name;
            textPrice.Text = treatment.Price.HasValue ? $"{treatment.Price.Value:0.00} €" : "N/A";
            textDuration.Text = treatment.DurationMinutes.HasValue ? $"{treatment.DurationMinutes} min" : "N/A";
            textDescription.Text = string.IsNullOrEmpty(treatment.Description) ? "No description available" : treatment.Description;
            textTreatmentGroup.Text = string.IsNullOrEmpty(treatment.TreatmentGroupName) ? "No group" : treatment.TreatmentGroupName;
            textWorkPosition.Text = string.IsNullOrEmpty(treatment.WorkPositionName) ? "No position" : treatment.WorkPositionName;
        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            Parent.CloseSidebar();
        }

        private void btnDeleteTreatment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnEditTreatment_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
