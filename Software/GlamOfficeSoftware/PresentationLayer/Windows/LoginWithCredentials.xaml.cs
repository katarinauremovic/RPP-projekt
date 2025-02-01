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
        private EmployeeService _employeeService = new EmployeeService();
        public LoginWithCredentials(LoginOptions loginOptionsForm)
        {
            InitializeComponent();
            _loginOptionsForm = loginOptionsForm;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            LogIn();
        }

        private async void LogIn()
        {
           var username = txtUsername.Text;
            var password = txtPassword.Password;
            var employee = await _employeeService.LogInWithCredentialsAsync(username, password);
            if (employee != null) {
                var mainWindow = new MainWindow();
                mainWindow.ShowDialog();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Pogrešno korisničko ime ili lozinka");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _loginOptionsForm.Show();
            this.Hide();
        }
    }
}
