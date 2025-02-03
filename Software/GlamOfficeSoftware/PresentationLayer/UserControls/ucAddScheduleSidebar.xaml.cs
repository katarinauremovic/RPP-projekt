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
    /// Interaction logic for ucEmployeeScheduleItem.xaml
    /// </summary>
    public partial class ucAddScheduleSidebar : UserControl
    {
        public ucSchedule _parentControl { get; set; }
        public ucAddScheduleSidebar(ucSchedule parentControl, List<DayDTO> weekDays, List<EmployeeDTO> employees)
        {
            InitializeComponent();
            _parentControl = parentControl;

            cmbDays.ItemsSource = weekDays;
            cmbDays.DisplayMemberPath = "Name";
            cmbDays.SelectedIndex = 0;

            cmbEmployees.ItemsSource = employees
                .Where(e => e.RoleName == "Regular user")
                .Select(e => new { FullName = $"{e.Firstname} {e.Lastname}", Id = e.Id })
                .ToList();

            cmbEmployees.DisplayMemberPath = "FullName";
            cmbEmployees.SelectedIndex = cmbEmployees.Items.Count > 0 ? 0 : -1; 
        }
        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            txtErrorMessage.Visibility = Visibility.Collapsed;

            if (cmbEmployees.SelectedItem == null || cmbDays.SelectedItem == null)
            {
                txtErrorMessage.Text = "Odaberite zaposlenika i dan.";
                txtErrorMessage.Visibility = Visibility.Visible;
                return;
            }

            var selectedEmployee = (dynamic)cmbEmployees.SelectedItem;
            var selectedDay = (DayDTO)cmbDays.SelectedItem;
            string startTimeText = txtStartTime.Text;
            string endTimeText = txtEndTime.Text;

            if (!TimeSpan.TryParse(startTimeText, out TimeSpan startTime) ||
                !TimeSpan.TryParse(endTimeText, out TimeSpan endTime))
            {
                txtErrorMessage.Text = "Neispravan format vremena. Koristite HH:mm.";
                txtErrorMessage.Visibility = Visibility.Visible;
                return;
            }

            if (startTime >= endTime)
            {
                txtErrorMessage.Text = "Vrijeme početka mora biti prije vremena završetka.";
                txtErrorMessage.Visibility = Visibility.Visible;
                return;
            }

            var scheduleService = new ScheduleService();

            try
            {
                var dailySchedule = new DailyScheduleDTO
                {
                    DayId = selectedDay.Id,
                    EmployeeId = selectedEmployee.Id,
                    WorkStartTime = startTime,
                    WorkEndTime = endTime
                };

                await scheduleService.AddDailyScheduleAsync(dailySchedule);
                await _parentControl.LoadData();

                Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                txtErrorMessage.Text = $"Greška: {ex.Message}";
                txtErrorMessage.Visibility = Visibility.Visible;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _parentControl.CloseSidebar();
        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }
    }
}
