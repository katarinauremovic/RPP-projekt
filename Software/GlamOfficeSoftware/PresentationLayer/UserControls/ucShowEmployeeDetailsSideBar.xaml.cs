using BusinessLogicLayer.Services;
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
    /// Interaction logic for ucShowEmployeeDetailsSideBar.xaml
    /// </summary>
    public partial class ucShowEmployeeDetailsSideBar : UserControl
    {
        public EmployeeDTO SelectedEmployee { get; set; }

        public ucEmployeeAdministration Parent { get; set; }

        private EmployeeService _employeeService;
        public ucShowEmployeeDetailsSideBar(EmployeeDTO employee)
        {
            InitializeComponent();
            _employeeService = new EmployeeService();
            SelectedEmployee = employee;
            LoadEmployeeDetails();
        }

        private void  LoadEmployeeDetails()
        {
            if (SelectedEmployee != null)
            {
                txtPIN.Text = SelectedEmployee.PIN;
                txtFirstname.Text = SelectedEmployee.Firstname;
                txtLastname.Text = SelectedEmployee.Lastname;
                txtEmail.Text = SelectedEmployee.Email;
                txtUsername.Text = SelectedEmployee.Username;
                txtPassword.Text = SelectedEmployee.Password;
                txtSalt.Text = SelectedEmployee.Salt;
                txtPhoneNumber.Text = SelectedEmployee.PhoneNumber;
                txtRole.Text = SelectedEmployee.RoleName;
                txtWorkPosition.Text = SelectedEmployee.WorkPositionName;


            }
        }
        

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedEmployee == null)
            {
                MessageBox.Show("No employee selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var editSidebar = new ucEditEmployeeDetails(SelectedEmployee);
            editSidebar.Parent = Parent;

            Parent.ccSidebar.Content = editSidebar;
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedEmployee == null)
            {
                MessageBox.Show("No employee selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var confirmBox = new PresentationLayer.Windows.winMessageBox();
            bool result = await confirmBox.ShowAsync("Confirm Deletion", $"Are you sure you want to delete {SelectedEmployee.Firstname} {SelectedEmployee.Lastname}?");

            if (result) 
            {
                try
                {
                    await _employeeService.DeleteEmployeeAsync(SelectedEmployee.Id);

                    Parent.RefreshGui();
                    Parent.CloseSideBarMenu();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting employee: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            Parent.CloseSideBarMenu();
        }

        private void btnDownloadQR_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnGenerateQR_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
