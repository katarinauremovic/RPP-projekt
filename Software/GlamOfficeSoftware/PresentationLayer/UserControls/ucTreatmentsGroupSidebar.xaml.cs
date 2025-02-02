using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Services;
using EntityLayer.Entities;
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
using System.Windows.Media.Animation;
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
            ParentControl?.CloseSidebar();
        }

        private void btnAddGroup_Click(object sender, RoutedEventArgs e)
        {
            
            if (ParentControl == null)
            {
                MessageBox.Show("ParentControl is null!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var addGroupSidebar = new ucAddNewTreatmentGroupSidebar();
            addGroupSidebar.ParentControl = ParentControl;

            ParentControl.ShowSidebar(addGroupSidebar);
        }

        

        private async void btnDeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listGroups.SelectedItem == null)
                {
                    throw new DataGridNoSelectionException("No group selected. Please select a group to delete.");
                }

                var selectedGroup = (TreatmentGroup)listGroups.SelectedItem;
                var confirmationBox = new winMessageBox();
                bool result = await confirmationBox.ShowAsync("Confirm Deletion", $"Are you sure you want to delete '{selectedGroup.Name}'?");

                if (result)
                {
                    await _treatmentService.DeleteTreatmentGroupAsync(selectedGroup.idTreatmentGroup);
                    RefreshGroupList();
                }
            }
            catch (DataGridNoSelectionException ex)
            {
                MessageBox.Show(ex.Message, "Selection Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        public async void RefreshGroupList()
        {
            var groups = await _treatmentService.GetAllTreatmentGroupsAsync();
            listGroups.ItemsSource = groups;
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
