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
    /// Interaction logic for ucTreatmentsGroupSidebar.xaml
    /// </summary>
    public partial class ucTreatmentsGroupSidebar : UserControl
    {
        private TreatmentService _treatmentService = new TreatmentService();

        public ucTreatmentManagement ParentControl { get; internal set; }
        public ucTreatmentsGroupSidebar()
        {
            InitializeComponent();
        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddGroup_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteGroup_Click(object sender, RoutedEventArgs e)
        {

        }
        private async Task LoadTreatmentGroups()
        {
            var groups = await _treatmentService.GetAllTreatmentGroupsAsync();

            listGroups.ItemsSource = groups;
            listGroups.DisplayMemberPath = "Name";
            listGroups.SelectedValuePath = "idTreatmentGroup"; 
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadTreatmentGroups();
        }
    }
}
