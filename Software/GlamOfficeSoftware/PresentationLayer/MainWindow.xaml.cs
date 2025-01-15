using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using PresentationLayer.UserControls;
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

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSidebarMenu_Click(object sender, RoutedEventArgs e)
        {
            var ucSidebarMenu = new ucSidebarMenu();
            ucSidebarMenu.Parent = this;
            ccSidebarMenu.Content = ucSidebarMenu;

            ShowSidebarMenu();
        }

        public void CloseSidebarMenu()
        {
            var slideOutAnimation = FindResource("SlideOutAnimation") as Storyboard;
            var sidebarMenu = (FrameworkElement)ccSidebarMenu.Content;
            var content = (FrameworkElement)ccContent.Content;

            if (sidebarMenu != null)
            {
                slideOutAnimation?.Begin(sidebarMenu);
                
                slideOutAnimation.Completed += (s, e) =>
                {
                    ccSidebarMenu.Content = null;
                    sidebarMenu.Visibility = Visibility.Collapsed;
                };
            }
        }

        private void ShowSidebarMenu()
        {
            var slideInAnimation = FindResource("SlideInAnimation") as Storyboard;
            var sidebarMenu = (FrameworkElement)ccSidebarMenu.Content;

            if (sidebarMenu != null)
            {
                sidebarMenu.Visibility = Visibility.Visible;

                sidebarMenu.Margin = new Thickness(240, 0, 0, 0);

                var marginAnimation = new ThicknessAnimation
                {
                    From = new Thickness(240, 0, 0, 0),
                    To = new Thickness(0, 0, 0, 0),
                    Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
                };

                sidebarMenu.BeginAnimation(MarginProperty, marginAnimation);
            }
        }
    }
}
