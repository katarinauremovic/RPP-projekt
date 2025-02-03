using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using EntityLayer.DTOs;
using EntityLayer.Entities;
using EntityLayer.Enums;
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
    /// Interaction logic for ucAddNewReservation.xaml
    /// </summary>
    public partial class ucAddNewReservation : UserControl
    {
        private ICollection<TreatmentDTO> _selectedTreatments;
        public ucReservationAdministration Parent { get; set; } 
        private IClientService _clientService;  

        private ITreatmentService _treatmentService;

        private IWeeklyScheduleForReservation _weeklyScheduleForReservationService;

        private IEmployeeService _employeeService;  

        private IClientHasRewardService _clientHasRewardService;

        private IGiftCardService _giftCardService;

        private IRewardService _rewardService;

        private IReservationHasTreatmentService _reservationHasTreatmentService;

        private IReservationService _reservationService;

        public ucAddNewReservation()
        {
            InitializeComponent();
            _clientService = new ClientService();
            _treatmentService = new TreatmentService();
            _weeklyScheduleForReservationService = new WeeklyScheduleForReservationService();
            _employeeService = new EmployeeService();
            _giftCardService = new GiftCardService();   
            _clientHasRewardService = new ClientHasRewardService();
            _rewardService = new RewardService();
            _reservationHasTreatmentService = new ReservationHasTreatmentService();
            _selectedTreatments = new List<TreatmentDTO>();
            _reservationService = new ReservationService();
            LoadAllClients();
            LoadTreatments();
            LoadEmployees();
            LoadAvailableDays();
        }

        private async void LoadEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            
            var formattedEmployees = employees.Select(e => new EmployeeDTO
            {
                FullName = $"{e.Firstname} {e.Lastname}",
                Id = e.Id
            }).ToList();
            
            cmbEmployee.ItemsSource = formattedEmployees;
            cmbEmployee.DisplayMemberPath = "FullName";
            cmbEmployee.SelectedValuePath = "Id";
        }

        private async void LoadAvailableDays()
        {
            var days = await _weeklyScheduleForReservationService.GetDaysForWeeklyScheduleAsync(3);

            var formattedDays = days.Select(d => new DayDTO
            {
                Name = d.Name,
                Id = d.idDay 
            }).ToList();

            cmbDay.ItemsSource = formattedDays;
            cmbDay.DisplayMemberPath = "Name";
            cmbDay.SelectedValuePath = "Id";
        }

        private async void LoadTreatments()
        {
            var treatments = await _treatmentService.GetAllTreatmentsAsync();

            var formattedTreatments = treatments.Select(t => new TreatmentDTO
            {
                Name = $"{t.Name}",
                idTreatment = t.idTreatment,
                Price = t.Price
            }).ToList();

            listTreatments.ItemsSource = formattedTreatments;
            listTreatments.DisplayMemberPath = "Name";
        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            Parent.CloseSideBarMenu();
        }

        private async void btnSaveReservation_Click(object sender, RoutedEventArgs e)
        {
            var reservation = new Reservation
            {
                Date = dpDate.SelectedDate.Value,
                StartTime = DateTime.ParseExact(tpStartTime.Text, "HH:mm:ss", null),
                EndTime = DateTime.ParseExact(tpEndTime.Text, "HH:mm:ss", null),
                Remark = txtRemark.Text,
                TotalTreatmentAmount = decimal.Parse(txtTotalTreatmentAmount.Text),
                GiftCardDiscount = decimal.Parse(txtGiftCardDiscount.Text),
                RewardDiscount = decimal.Parse(txtRewardDiscount.Text),
                TotalPrice = decimal.Parse(txtTotalPrice.Text),
                isPaid = false,
                Status = ReservationStatuses.Confirmed.ToString(),
                Client_idClient = (int)cmbClient.SelectedValue,
                Day_idDay = (int)cmbDay.SelectedValue,
                Employee_idEmployee = (int)cmbEmployee.SelectedValue
            };

            await _reservationService.AddReservationAsync(reservation);
            await SaveInRht();
        }

        public async Task SaveInRht()
        {
            var reservation = await _reservationService.GetLastReservationAsync();

            foreach(var treatment in _selectedTreatments)
            {
                var reservationHasTreatment = new Reservation_has_Treatment
                {
                    Treatment_idTreatment = treatment.idTreatment,
                    Reservation_idReservation = reservation.idReservation,
                    Amount = 1
                };

                await _reservationHasTreatmentService.AddReservationHasTreatmentAsync(reservationHasTreatment);
            }
        }

        private void btnCancelReservation_Click(object sender, RoutedEventArgs e)
        {
            Parent.CloseSideBarMenu();
        }

        private async void LoadAllClients()
        {
            var clients = await _clientService.GetAllClientsDTOAsync();

            var formattedClients = clients.Select(c => new ClientDTO
            {
                Firstname = $"{c.Firstname} {c.Lastname}",
                Id = c.Id
            }).ToList();

            cmbClient.ItemsSource = formattedClients;
            cmbClient.DisplayMemberPath = "Firstname";
            cmbClient.SelectedValuePath = "Id";
        }

        private void listTreatments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedTreatments = listTreatments.SelectedItems.Cast<TreatmentDTO>().ToList();

            double? totalPrice = _selectedTreatments.Sum(t => t.Price);

            txtTotalPrice.Text = totalPrice.ToString();
            txtTotalTreatmentAmount.Text = totalPrice.ToString();
        }

        private void txtGiftCardDiscount_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void txtRewardDiscount_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void txtPromoDiscount_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void txtPromoDiscount_LostFocus(object sender, RoutedEventArgs e)
        {
      
           
        }

        

        private async void txtRewardDiscount_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
               
                string promoCode = txtRewardDiscount.Text;

                if (string.IsNullOrEmpty(promoCode))
                {
                    return;
                }


                var chr = await _clientHasRewardService.GetRewardByRedeemCode(promoCode);
                var reward = await _rewardService.GetRewardByIdAsync(chr.Reward_idReward);

                if (reward == null)
                {
                    MessageBox.Show("Code is not valid!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                } else
                {
                    if (double.TryParse(txtTotalPrice.Text, out double totalPrice))
                    {

                        double newTotalPrice = totalPrice + (double)reward.RewardAmount;


                        txtTotalPrice.Text = newTotalPrice.ToString("F2");
                    }


                    await _clientHasRewardService.UpdateClientHasReward(chr);
                    txtRewardDiscount.Text = reward.RewardAmount.ToString();

                    MessageBox.Show($"Promo kod '{promoCode}' is successfully used!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }  
            }
            catch (Exception ex)
            {
                MessageBox.Show($"There was a mistake: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtPromoDiscount_LostFocus_1(object sender, RoutedEventArgs e)
        {
            try
            {

                if (double.TryParse(txtPromoDiscount.Text, out double promoDiscount))
                {

                    if (promoDiscount >= 0)
                    {
                        MessageBox.Show("Promo discount must be a negative value!", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                        txtPromoDiscount.Text = "0";
                        return;
                    }


                    if (double.TryParse(txtTotalPrice.Text, out double totalPrice))
                    {

                        double newTotalPrice = totalPrice + promoDiscount;


                        txtTotalPrice.Text = newTotalPrice.ToString("F2");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid promo discount input!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtPromoDiscount.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void txtGiftCardDiscount_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                string promoCode = txtGiftCardDiscount.Text.Trim();

                if (string.IsNullOrEmpty(promoCode))
                {
                    return; 
                }

               

                
                var giftCard = await _giftCardService.GetOneGiftCardByPromoCodeAsync(promoCode);

                if (giftCard == null)
                {
                    MessageBox.Show("Invalid gift card promo code!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

               
                if (giftCard.ToSpend == 0)
                {
                    MessageBox.Show("This gift card has no available balance!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                
                if (double.TryParse(txtTotalPrice.Text, out double totalPrice))
                {
                    double newTotalPrice = totalPrice + (double)giftCard.ToSpend;

                    if (newTotalPrice < 0)
                    {
                        newTotalPrice = 0; 
                    }

                   
                    txtTotalPrice.Text = newTotalPrice.ToString("F2");
                    txtGiftCardDiscount.Text = giftCard.ToSpend.ToString();
                }

             
                bool redeemed = await _giftCardService.RedeemGiftCardAsync(promoCode);

                if (redeemed)
                {
                    MessageBox.Show($"Gift card '{promoCode}' successfully redeemed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Failed to redeem the gift card!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        

    }
}
}
