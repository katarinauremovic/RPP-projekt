using BusinessLogicLayer.Interfaces;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for ucSchedule.xaml
    /// </summary>
    public partial class ucSchedule : UserControl
    {
        public MainWindow Parent { get; set; }
        private ScheduleService _scheduleService = new ScheduleService();
        private EmployeeService employeeService = new EmployeeService();
        private List<DayDTO> _weekDays;
        private List<EmployeeDTO> _employees;
        public ucSchedule()
        {
            InitializeComponent();
            _ = LoadData();
        }

        public async Task LoadData()
        {
            var scheduleService = new ScheduleService();
            _weekDays = (await scheduleService.GetOrCreateDaysForNextWeekAsync()).ToList();
            _employees = (await employeeService.GetAllEmployeesAsync()).ToList();

            if (_weekDays.Count > 0)
            {
                DateTime startDate = _weekDays.First().Date.Value;
                DateTime endDate = _weekDays.Last().Date.Value; 

                txtDateRange.Text = $"{startDate:MMMM dd} - {endDate:MMMM dd, yyyy}";
            }
        }


        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            await LoadData();
            var sidebar = new ucAddScheduleSidebar(_weekDays, _employees);
            ccSidebar.Content = sidebar;
            ccSidebar.Visibility = Visibility.Visible;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ShowSidebar()
        {
            var slideInAnimation = FindResource("SlideInAnimation") as Storyboard;
            var sidebarMenu = (FrameworkElement)ccSidebar.Content;

            if (sidebarMenu != null)
            {
                sidebarMenu.Visibility = Visibility.Visible;
                sidebarMenu.Margin = new Thickness(250, 0, 0, 0);

                var marginAnimation = new ThicknessAnimation
                {
                    From = new Thickness(250, 0, 0, 0),
                    To = new Thickness(0, 0, 0, 0),
                    Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
                };

                sidebarMenu.BeginAnimation(MarginProperty, marginAnimation);
            }
        }
    }
}
