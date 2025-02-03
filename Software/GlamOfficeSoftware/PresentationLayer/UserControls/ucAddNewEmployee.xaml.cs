using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using EntityLayer.Entities;
using NStringGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
using BusinessLogicLayer.Exceptions;
using System.Windows.Media.Animation;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for ucAddNewEmployee.xaml
    /// </summary>
    public partial class ucAddNewEmployee : UserControl
    {
        public ucEmployeeAdministration Parent { get; set; }
        private IEmployeeService _employeeService;
        
        
        public ucAddNewEmployee()
        {
            InitializeComponent();
            _employeeService = new EmployeeService();
        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            Parent.CloseSideBarMenu();
        }

        private async void btnAddNewEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                if (string.IsNullOrWhiteSpace(txtFirstname.Text) ||
                    string.IsNullOrWhiteSpace(txtPIN.Text) ||
                    string.IsNullOrWhiteSpace(txtLastname.Text) ||
                    string.IsNullOrWhiteSpace(txtEmail.Text) ||
                    string.IsNullOrWhiteSpace(txtUsername.Text) ||
                    string.IsNullOrWhiteSpace(txtPhoneNumber.Text) ||
                    string.IsNullOrWhiteSpace(txtSalt.Text) ||
                    string.IsNullOrWhiteSpace(txtPassword.Text) ||
                    cmbRole.SelectedItem == null ||
                    cmbWorkPosition.SelectedItem == null)
                {
                    throw new InvalidEmployeeDataException("All fields must be filled.");
                }

                if (!IsValidPIN(txtPIN.Text))
                {
                    throw new InvalidPINException("OIB must contain exactly 11 digits.");
                }

                if (!IsLettersOnly(txtFirstname.Text) || !IsLettersOnly(txtLastname.Text))
                {
                    throw new InvalidEmployeeDataException("Firstname and lastname must contain only letters.");
                }

                if (!IsValidTelephone(txtPhoneNumber.Text))
                {
                    throw new InvalidPhoneNumberException("Phone number must contain only digits.");
                }
                if (!IsValidEmail(txtEmail.Text))
                {
                    throw new InvalidEmailException("Invalid email format. It must contain '@' and a valid domain.");
                }

                bool isUsernameTaken = await _employeeService.IsUsernameTakenAsync(txtUsername.Text);
                if (isUsernameTaken)
                {
                    throw new UsernameTakenException("This username is already taken. Please choose another.");
                }

                string hashedPassword = HashPassword(txtSalt.Text, txtPassword.Text);

                var employee = new Employee
                {
                    PIN = txtPIN.Text,
                    Firstname = txtFirstname.Text,
                    Lastname = txtLastname.Text,
                    Email = txtEmail.Text,
                    Username = txtUsername.Text,
                    Password = hashedPassword,
                    Salt = txtSalt.Text,
                    PhoneNumber = txtPhoneNumber.Text,
                    Role_idRole = (cmbRole.SelectedItem as Role).idRole,
                    WorkPosition_idWorkPosition = (cmbWorkPosition.SelectedItem as WorkPosition).idWorkPosition

                };

                await _employeeService.AddNewEmployeeAsync(employee);
                Parent.RefreshGui();
                ClearInputs();
                Parent.CloseSideBarMenu();
            }
            catch (InvalidEmployeeDataException ex)
            {
                MessageBox.Show(ex.Message, "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (InvalidPhoneNumberException ex)
            {
                MessageBox.Show(ex.Message, "Invalid Phone Number", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (InvalidPINException ex)
            {
                MessageBox.Show(ex.Message, "Invalid OIB", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (InvalidEmailException ex)
            {
                MessageBox.Show(ex.Message, "Invalid Email format", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (UsernameTakenException ex)
            {
                MessageBox.Show(ex.Message, "Username is already taken. Please choose another one.", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ClearInputs()
        {
            txtFirstname.Clear();
            txtLastname.Clear();
            txtEmail.Clear();
            txtPhoneNumber.Clear();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Parent.CloseSideBarMenu();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadWorkPositions();
            LoadRoles();
        }

        private async void LoadRoles()
        {
            var roles = await _employeeService.GetRolesAsync();
            cmbRole.ItemsSource = roles;
            cmbRole.DisplayMemberPath = "Name";
        }

        private async void LoadWorkPositions()
        {
            var workPositions = await _employeeService.GetWorkPositionsAsync();
            cmbWorkPosition.ItemsSource = workPositions;
            cmbWorkPosition.DisplayMemberPath = "Name";
        }

        private void cmbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            txtSalt.Text = NStringGenerator.NStringGenerator.Generate();
            txtPassword.Text = NStringGenerator.NStringGenerator.Generate();
        }

        private bool IsValidPIN(string pin)
        {
            return Regex.IsMatch(pin, @"^\d{11}$");
        }

        private bool IsLettersOnly(string value)
        {
            return !string.IsNullOrEmpty(value) && value.All(c => char.IsLetter(c) || char.IsWhiteSpace(c) || c == '-');
        }

        private bool IsValidTelephone(string telephone)
        {
            return Regex.IsMatch(telephone, @"^[\+0-9\s]+$");
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }


        private string HashPassword(string salt, string password)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(salt + password);
                byte[] hashedBytes = sha512.ComputeHash(inputBytes);
                return Convert.ToBase64String(hashedBytes); 
            }
        }

    }
}
