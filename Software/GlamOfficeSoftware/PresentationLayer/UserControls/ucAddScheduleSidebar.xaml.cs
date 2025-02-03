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
            _parent.CloseSidebar();
        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            _parent.CloseSidebar();
        }
    }
}
