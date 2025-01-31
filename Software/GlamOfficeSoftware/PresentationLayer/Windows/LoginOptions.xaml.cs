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
    /// Interaction logic for LoginOptions.xaml
    /// </summary>
    public partial class LoginOptions : Window
    {

        public LoginOptions()
        {
            InitializeComponent();
        }

        private void btnCredentialsLogin_Click(object sender, RoutedEventArgs e)
        {
            var loginCredentialsForm = new LoginWithCredentials(this);
            loginCredentialsForm.Show();
            this.Hide();
        }

        private void btnQRCodeLogin_Click(object sender, RoutedEventArgs e)
        {
            var loginQRCodeForm = new LoginWithQRCode(this);
            loginQRCodeForm.Show();
            this.Hide();
        }
    }
}
