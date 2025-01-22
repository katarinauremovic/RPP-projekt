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
    /// Interaction logic for winMessageBox.xaml
    /// </summary>
    public partial class winMessageBox : Window
    {
        private TaskCompletionSource<bool> _tcs;

        public winMessageBox()
        {
            InitializeComponent();
        }

        public Task<bool> ShowAsync(string title, string message)
        {
            Title = title;
            txtMessage.Text = message;
            _tcs = new TaskCompletionSource<bool>();
            Visibility = Visibility.Visible;
            return _tcs.Task;
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            Close(true);
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            Close(false);
        }

        private void Close(bool response)
        {
            Visibility = Visibility.Collapsed;
            if (_tcs != null && !_tcs.Task.IsCompleted)
            {
                _tcs.SetResult(response);
            }
        }

        public void HideButtons()
        {
            btnYes.Visibility = Visibility.Collapsed;
            btnNo.Visibility = Visibility.Collapsed;
        }
    }
}
