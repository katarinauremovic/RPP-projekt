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
        private ucSchedule _parent;
        private ScheduleService _scheduleService = new ScheduleService();

        public ucAddScheduleSidebar(ucSchedule parent, List<DayDTO> availableDays, List<EmployeeDTO> employees)
        {
            InitializeComponent();
            _parent = parent;

            cmbDays.ItemsSource = availableDays;
            cmbDays.DisplayMemberPath = "Name";
            cmbDays.SelectedIndex = -1;

            cmbEmployees.ItemsSource = employees
                .Where(e => e.RoleName == "Regular user")
                .ToList();
            cmbEmployees.DisplayMemberPath = "FullName";
            cmbEmployees.SelectedIndex = -1;
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            txtErrorMessage.Visibility = Visibility.Collapsed;

            if (cmbEmployees.SelectedItem == null || cmbDays.SelectedItem == null)
            {
                ShowError("Odaberite zaposlenika i dan.");
                return;
            }

            var selectedEmployee = (EmployeeDTO)cmbEmployees.SelectedItem;
            var selectedDay = (DayDTO)cmbDays.SelectedItem;

            DateTime? startTime = ConvertToDateTime(selectedDay, txtStartTime.Text);
            DateTime? endTime = ConvertToDateTime(selectedDay, txtEndTime.Text);

            if (startTime == null || endTime == null)
            {
                ShowError("Vrijeme nije ispravno. Koristite format HH:mm.");
                return;
            }

            if (startTime >= endTime)
            {
                ShowError("Vrijeme početka mora biti prije vremena završetka.");
                return;
            }

            try
            {
                var dailySchedule = new DailyScheduleDTO
                {
                    DayId = selectedDay.Id,
                    EmployeeId = selectedEmployee.Id,
                    WorkStartTime = startTime,
                    WorkEndTime = endTime
                };

                await _scheduleService.AddDailyScheduleAsync(dailySchedule);
                await _parent.LoadData();

                _parent.CloseSidebar();
            }
            catch (Exception ex)
            {
                ShowError($"Greška: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _parent.CloseSidebar();
        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            _parent.CloseSidebar();
        }

        private DateTime? ConvertToDateTime(DayDTO selectedDay, string timeText)
        {
            if (selectedDay?.Date == null || string.IsNullOrWhiteSpace(timeText))
                return null;

            if (TimeSpan.TryParse(timeText, out TimeSpan time))
                return selectedDay.Date.Value.Date.Add(time);

            return null;
        }

        private void ShowError(string message)
        {
            txtErrorMessage.Text = message;
            txtErrorMessage.Visibility = Visibility.Visible;
        }
    }
}
