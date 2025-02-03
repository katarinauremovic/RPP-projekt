using BusinessLogicLayer.Services;
using EntityLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.UserControls
{
    public partial class ucEditScheduleSidebar : UserControl
    {
        private ucSchedule _parent;
        private DailyScheduleDTO _scheduleData;
        private ScheduleService _scheduleService = new ScheduleService();

        public ucEditScheduleSidebar(ucSchedule parent, List<DayDTO> availableDays, List<EmployeeDTO> employees, DailyScheduleDTO scheduleData)
        {
            InitializeComponent();
            _parent = parent;
            _scheduleData = scheduleData;

            cmbDays.ItemsSource = null;
            cmbDays.ItemsSource = availableDays;
            cmbDays.DisplayMemberPath = "Name";
            cmbDays.SelectedValuePath = "Id";

            cmbEmployees.ItemsSource = null;
            cmbEmployees.ItemsSource = employees
                .Where(e => e.RoleName == "Regular user")
                .ToList();
            cmbEmployees.DisplayMemberPath = "FullName";
            cmbEmployees.SelectedValuePath = "Id";

            var selectedEmployee = employees.FirstOrDefault(e => e.Id == _scheduleData.EmployeeId);
            var selectedDay = availableDays.FirstOrDefault(d => d.Id == _scheduleData.DayId);

            if (selectedEmployee == null)
                MessageBox.Show($"Greška: EmployeeId {_scheduleData.EmployeeId} nije pronađen u listi zaposlenika!");
            if (selectedDay == null)
                MessageBox.Show($"Greška: DayId {_scheduleData.DayId} nije pronađen u listi dana!");

            Task.Delay(100).ContinueWith(_ =>
            {
                Dispatcher.Invoke(() =>
                {
                    cmbEmployees.SelectedValue = _scheduleData.EmployeeId;
                    cmbDays.SelectedValue = _scheduleData.DayId;
                });
            });

            Task.Delay(100).ContinueWith(_ =>
            {
                Dispatcher.Invoke(() =>
                {
                    txtStartTime.Text = _scheduleData.WorkStartTime?.ToString(@"hh\:mm") ?? "";
                    txtEndTime.Text = _scheduleData.WorkEndTime?.ToString(@"hh\:mm") ?? "";
                });
            });

        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbDays.SelectedItem == null || cmbEmployees.SelectedItem == null)
            {
                ShowError("Morate odabrati zaposlenika i dan!");
                return;
            }

            var selectedDay = (DayDTO)cmbDays.SelectedItem;

            DateTime? newStartTime = ConvertToDateTime(selectedDay, txtStartTime.Text);
            DateTime? newEndTime = ConvertToDateTime(selectedDay, txtEndTime.Text);

            if (newStartTime == null || newEndTime == null)
            {
                ShowError("Vrijeme nije ispravno. Koristite format HH:mm.");
                return;
            }

            if (newStartTime >= newEndTime)
            {
                ShowError("Vrijeme početka mora biti prije vremena završetka.");
                return;
            }

            var updatedSchedule = new DailyScheduleDTO
            {
                DayId = selectedDay.Id,
                EmployeeId = (int)cmbEmployees.SelectedValue,
                WorkStartTime = newStartTime,
                WorkEndTime = newEndTime
            };

            try
            {
                await _scheduleService.UpdateDailyScheduleAsync(updatedSchedule);

                MessageBox.Show("Raspored je uspješno ažuriran!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

                _parent.CloseSidebar();
                await _parent.LoadData();
            }
            catch (Exception ex)
            {
                ShowError($"Greška pri ažuriranju rasporeda: {ex.Message}");
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
            MessageBox.Show(message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }
}
