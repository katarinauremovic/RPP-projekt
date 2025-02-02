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
      
        public ucAddScheduleSidebar(List<DayDTO> weekDays, List<EmployeeDTO> employees)
        {
            InitializeComponent();
            cmbDays.ItemsSource = weekDays;
            cmbDays.DisplayMemberPath = "Name";
            cmbDays.SelectedIndex = 0;

            cmbEmployees.ItemsSource = employees
    .Where(e => e.RoleName == "Regular user") 
    .Select(e => new { FullName = $"{e.Firstname} {e.Lastname}", Id = e.Id })
    .ToList();

            cmbEmployees.DisplayMemberPath = "FullName";
            cmbEmployees.SelectedIndex = cmbEmployees.Items.Count > 0 ? 0 : -1; // Postavi prvi item ako ih ima

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }
    }
}
