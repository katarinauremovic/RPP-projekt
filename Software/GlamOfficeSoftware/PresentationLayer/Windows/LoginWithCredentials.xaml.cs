using BusinessLogicLayer;
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
using System.Windows.Shapes;

namespace PresentationLayer.Windows
{
    /// <summary>
    /// Interaction logic for LoginWithCredentials.xaml
    /// </summary>
    public partial class LoginWithCredentials : Window
    {
        private LoginOptions _loginOptionsForm;
        private EmployeeService _employeeService;
        public LoginWithCredentials(LoginOptions loginOptionsForm)
        {
            InitializeComponent();
            _loginOptionsForm = loginOptionsForm;
            _employeeService = new EmployeeService();
        }

        
        private async void LogIn()
        {
            Cursor = Cursors.Wait;
            lblErrorMessage.Visibility = Visibility.Collapsed;
            var username = txtUsername.Text;
            var password = txtPassword.Password;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                lblErrorMessage.Content = "Enter username and password";
                lblErrorMessage.Visibility = Visibility.Visible;
                Cursor = Cursors.Arrow;
                return;
            }
            var employee = await _employeeService.LogInWithCredentialsAsync(username, password);
            if (employee != null) {
                LoggedInEmployee.SetLoggedInEmployee(employee);
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Hide();
            }
            else
            {
                lblErrorMessage.Content = "Incorrect data";
                lblErrorMessage.Visibility = Visibility.Visible;
            }
            Cursor = Cursors.Arrow;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _loginOptionsForm.Show();
            this.Hide();
        }

        private void btnLogin_Click_1(object sender, RoutedEventArgs e)
        {
            LogIn();
        }
    }
}
