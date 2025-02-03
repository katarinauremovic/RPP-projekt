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
    /// Interaction logic for ucEditScheduleSidebar.xaml
    /// </summary>
    public partial class ucEditScheduleSidebar : UserControl
    {
        private ucSchedule _parent;
        private DailyScheduleDTO _scheduleData;
        private ScheduleService _scheduleService = new ScheduleService();
        public ucEditScheduleSidebar(ucSchedule parent, DailyScheduleDTO scheduleData)
        {
            InitializeComponent();
            _parent = parent;
            _scheduleData = scheduleData;

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
                await _parent.LoadData(); // Ponovno učitaj podatke
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
