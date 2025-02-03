using BusinessLogicLayer.Services;
using EntityLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
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

            cmbDays.ItemsSource = availableDays;
            cmbDays.DisplayMemberPath = "Name";

            cmbEmployees.ItemsSource = employees;
            cmbEmployees.DisplayMemberPath = "FullName";

            cmbDays.SelectedValue = _scheduleData.DayId;
            cmbEmployees.SelectedValue = _scheduleData.EmployeeId;

            txtStartTime.Text = _scheduleData.WorkStartTime?.ToString(@"hh\:mm");
            txtEndTime.Text = _scheduleData.WorkEndTime?.ToString(@"hh\:mm");
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

            try
            {
                await _scheduleService.UpdateDailyScheduleAsync(_scheduleData.DayId, _scheduleData.EmployeeId, newStartTime, newEndTime);
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
