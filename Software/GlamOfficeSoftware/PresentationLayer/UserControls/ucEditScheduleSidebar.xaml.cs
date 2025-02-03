using BusinessLogicLayer.Services;
using EntityLayer.DTOs;
using System;
using System.Collections.Generic;
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

            cmbDays.IsEnabled = false;
            cmbEmployees.IsEnabled = false;

            SetSelectedValues(availableDays, employees);
        }

        private void SetSelectedValues(List<DayDTO> availableDays, List<EmployeeDTO> employees)
        {
            
            var selectedDay = availableDays.FirstOrDefault(d => d.Id == _scheduleData.DayId);
            var selectedEmployee = employees.FirstOrDefault(e => e.Id == _scheduleData.EmployeeId);

            cmbDays.Items.Clear();
            cmbEmployees.Items.Clear();

            if (selectedDay != null)
            {
                cmbDays.Items.Add(selectedDay);
                cmbDays.DisplayMemberPath = "Name";
                cmbDays.SelectedValuePath = "Id";
            }

            if (selectedEmployee != null)
            {
                cmbEmployees.Items.Add(selectedEmployee);
                cmbEmployees.DisplayMemberPath = "FullName";
                cmbEmployees.SelectedValuePath = "Id";
            }

            Task.Delay(100).ContinueWith(_ =>
            {
                Dispatcher.Invoke(() =>
                {
                    cmbDays.SelectedIndex = 0;
                    cmbEmployees.SelectedIndex = 0;

                    txtStartTime.Text = _scheduleData.WorkStartTime?.ToString(@"hh\:mm") ?? "";
                    txtEndTime.Text = _scheduleData.WorkEndTime?.ToString(@"hh\:mm") ?? "";
                });
            });
        }


        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStartTime.Text) || string.IsNullOrWhiteSpace(txtEndTime.Text))
            {
                ShowError("You must enter both start and end time.");
                return;
            }

            if (!TimeSpan.TryParse(txtStartTime.Text, out TimeSpan newStartTime) ||
                !TimeSpan.TryParse(txtEndTime.Text, out TimeSpan newEndTime))
            {
                ShowError("Invalid time format. Use HH:mm.");
                return;
            }

            if (newStartTime >= newEndTime)
            {
                ShowError("Start time must be earlier than end time.");
                return;
            }

            TimeSpan workStart = TimeSpan.FromHours(7);
            TimeSpan workEnd = TimeSpan.FromHours(20);

            if (newStartTime < workStart || newEndTime > workEnd)
            {
                ShowError("Work hours must be between 07:00 and 20:00.");
                return;
            }


            var updatedSchedule = new DailyScheduleDTO
            {
                DayId = _scheduleData.DayId,
                EmployeeId = _scheduleData.EmployeeId,
                WorkStartTime = _scheduleData.WorkStartTime?.Date.Add(newStartTime),
                WorkEndTime = _scheduleData.WorkEndTime?.Date.Add(newEndTime)
            };

            try
            {
                await _scheduleService.UpdateDailyScheduleAsync(updatedSchedule);
                MessageBox.Show("Schedule successfully updated!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

                _parent.CloseSidebar();
                await _parent.LoadData();
            }
            catch (Exception ex)
            {
                ShowError($"Error updating schedule: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => _parent.CloseSidebar();
        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e) => _parent.CloseSidebar();

        private void ShowError(string message) => MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
