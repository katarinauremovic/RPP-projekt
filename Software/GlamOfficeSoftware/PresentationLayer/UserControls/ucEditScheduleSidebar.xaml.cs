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
            if (!TimeSpan.TryParse(txtStartTime.Text, out TimeSpan newStartTime) ||
        !TimeSpan.TryParse(txtEndTime.Text, out TimeSpan newEndTime))
            {
                MessageBox.Show("Neispravan format vremena! Koristite HH:mm", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (newStartTime >= newEndTime)
            {
                MessageBox.Show("Vrijeme početka mora biti prije vremena završetka.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (cmbDays.SelectedItem == null || cmbEmployees.SelectedItem == null)
            {
                MessageBox.Show("Morate odabrati zaposlenika i dan!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var updatedSchedule = new DailyScheduleDTO
            {
                DayId = (int)cmbDays.SelectedValue,
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
                MessageBox.Show($"Greška pri ažuriranju rasporeda: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
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
    }
}
