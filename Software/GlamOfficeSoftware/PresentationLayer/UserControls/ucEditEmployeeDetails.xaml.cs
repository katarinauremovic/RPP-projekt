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
    /// Interaction logic for ucEditEmployeeDetails.xaml
    /// </summary>
    public partial class ucEditEmployeeDetails : UserControl
    {
        public ucEmployeeAdministration Parent { get; set; }

        private EmployeeService _employeeService;
        private EmployeeDTO _employee;
        public ucEditEmployeeDetails(EmployeeDTO employee)
        {
            InitializeComponent();
            InitializeComponent();
            _employeeService = new EmployeeService();
            _employee = employee;
            LoadEmployeeData();
        }

        private void LoadEmployeeData()
        {
            if (_employee != null)
            {
                txtPIN.Text = _employee.PIN;
                txtFirstname.Text = _employee.Firstname;
                txtLastname.Text = _employee.Lastname;
                txtEmail.Text = _employee.Email;
                txtUsername.Text = _employee.Username;
                txtPassword.Text = _employee.Password;
                txtSalt.Text = _employee.Salt;
                txtPhoneNumber.Text = _employee.PhoneNumber;
                cmbRole.SelectedItem = cmbRole.Items
                    .Cast<Role>()
                    .FirstOrDefault(r => r.Name == _employee.RoleName);

                cmbWorkPosition.SelectedItem = cmbWorkPosition.Items
                    .Cast<WorkPosition>()
                    .FirstOrDefault(wp => wp.Name == _employee.WorkPositionName);
            }
        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            Parent.CloseSideBarMenu();
        }

        private async void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (_employee == null)
                return;

            try
            {
                _employee.PIN = txtPIN.Text;
                _employee.Firstname = txtFirstname.Text;
                _employee.Lastname = txtLastname.Text;
                _employee.Email = txtEmail.Text;
                _employee.Username = txtUsername.Text;
                _employee.PhoneNumber = txtPhoneNumber.Text;
                _employee.RoleName = cmbRole.Text;
                _employee.WorkPositionName = cmbWorkPosition.Text;
                var employeeToUpdate = new Employee
                {
                    idEmployee = _employee.Id,
                    PIN = _employee.PIN,
                    Firstname = _employee.Firstname,
                    Lastname = _employee.Lastname,
                    Email = _employee.Email,
                    Username = _employee.Username,
                    PhoneNumber = _employee.PhoneNumber,
                    Password = _employee.Password,
                    Salt = _employee.Salt,
                    Role_idRole = (cmbRole.SelectedItem as Role)?.idRole ?? 0,
                    WorkPosition_idWorkPosition = (cmbWorkPosition.SelectedItem as WorkPosition)?.idWorkPosition ?? 0
                };
 
                await _employeeService.UpdateEmployeeAsync(employeeToUpdate);

                Parent.RefreshGui();
                Parent.CloseSideBarMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating employee: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Parent.CloseSideBarMenu();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadRolesAsync();
            await LoadWorkPositionsAsync();
            LoadEmployeeData();
        }
        private async Task LoadRolesAsync()
        {
            var roles = await _employeeService.GetRolesAsync();
            cmbRole.ItemsSource = roles;
            cmbRole.DisplayMemberPath = "Name";
        }

        private async Task LoadWorkPositionsAsync()
        {
            var workPositions = await _employeeService.GetWorkPositionsAsync();
            cmbWorkPosition.ItemsSource = workPositions;
            cmbWorkPosition.DisplayMemberPath = "Name";
        }
    }
}
