using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using EntityLayer.Entities;
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
    /// Interaction logic for ucShowGiftCardSideBar.xaml
    /// </summary>
    public partial class ucShowGiftCardSideBar : UserControl
    {
        public ucGiftCardAdministration Parent { get; set; }

        private GiftCard _selectedGiftCard;

        private IGiftCardService _giftCardService;
        public ucShowGiftCardSideBar(GiftCard selected)
        {
            InitializeComponent();
            _selectedGiftCard = selected;
            _giftCardService = new GiftCardService();

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Parent.CloseSideBarMenu();
        }

        private async void btnSaveToPdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Run(() => _giftCardService.GenerateGiftCardInPdf(_selectedGiftCard));
            }
            catch (ClientOperationException ex)
            {
                MessageBox.Show(ex.Message, "GiftCard Operation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            Parent.CloseSideBarMenu();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadGiftCardAsync();
        }

        private async Task LoadGiftCardAsync()
        {
            txtGiftCard.Text = await Task.Run(() => _giftCardService.GenerateGiftCardInStringFormat(_selectedGiftCard));
        }
    }
}
