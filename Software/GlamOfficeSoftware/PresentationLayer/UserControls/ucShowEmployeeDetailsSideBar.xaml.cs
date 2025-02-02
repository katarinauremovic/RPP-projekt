using BusinessLogicLayer.Services;
using EntityLayer.DTOs;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for ucShowEmployeeDetailsSideBar.xaml
    /// </summary>
    public partial class ucShowEmployeeDetailsSideBar : UserControl
    {
        public EmployeeDTO SelectedEmployee { get; set; }

        public ucEmployeeAdministration Parent { get; set; }

        private EmployeeService _employeeService;
        public ucShowEmployeeDetailsSideBar(EmployeeDTO employee)
        {
            InitializeComponent();
            _employeeService = new EmployeeService();
            SelectedEmployee = employee;
            LoadEmployeeDetails();
        }

        private void  LoadEmployeeDetails()
        {
            if (SelectedEmployee != null)
            {
                txtPIN.Text = SelectedEmployee.PIN;
                txtFirstname.Text = SelectedEmployee.Firstname;
                txtLastname.Text = SelectedEmployee.Lastname;
                txtEmail.Text = SelectedEmployee.Email;
                txtUsername.Text = SelectedEmployee.Username;
                txtPassword.Text = SelectedEmployee.Password;
                txtSalt.Text = SelectedEmployee.Salt;
                txtPhoneNumber.Text = SelectedEmployee.PhoneNumber;
                txtRole.Text = SelectedEmployee.RoleName;
                txtWorkPosition.Text = SelectedEmployee.WorkPositionName;


            }
        }
        

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedEmployee == null)
            {
                MessageBox.Show("No employee selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var editSidebar = new ucEditEmployeeDetails(SelectedEmployee);
            editSidebar.Parent = Parent;

            Parent.ccSidebar.Content = editSidebar;
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedEmployee == null)
            {
                MessageBox.Show("No employee selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var confirmBox = new PresentationLayer.Windows.winMessageBox();
            bool result = await confirmBox.ShowAsync("Confirm Deletion", $"Are you sure you want to delete {SelectedEmployee.Firstname} {SelectedEmployee.Lastname}?");

            if (result) 
            {
                try
                {
                    await _employeeService.DeleteEmployeeAsync(SelectedEmployee.Id);

                    Parent.RefreshGui();
                    Parent.CloseSideBarMenu();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting employee: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            Parent.CloseSideBarMenu();
        }

        private void btnDownloadQR_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var pdfPath = GeneratePDFWithQRCode();
                if (pdfPath != null)
                {
                    MessageBox.Show($"PDF je spremljen: {pdfPath}");
                    lblErrorMessage.Visibility = Visibility.Collapsed; 
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Greška kod preuzimanja QR koda: {ex.Message}");
            }
        }

        private void btnGenerateQR_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                imgQRCode.Source = GenerateQRCode();
                lblErrorMessage.Visibility = Visibility.Collapsed; 
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Greška kod generiranja QR koda: {ex.Message}");
            }
        }

        private ImageSource GenerateQRCode()
        {
            var username = txtUsername.Text;
            var password = txtPassword.Text;

            var data = $"{username}:{password}";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            QRCoder.QRCode qrCode = new QRCoder.QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20);

            return ConvertBitmapToImageSource(qrCodeImage);
        }
        private ImageSource ConvertBitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
       

        private string GeneratePDFWithQRCode()
        {
            var salt = txtSalt.Text;
            var username = txtUsername.Text;
            var password = txtPassword.Text;
            var data = $"{username}:{password}";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            QRCoder.QRCode qrCode = new QRCoder.QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string pdfPath = System.IO.Path.Combine(desktopPath, $"QRCode_{timestamp}.pdf");

            try
            {
                using (PdfDocument document = new PdfDocument())
                {
                    PdfPage page = document.AddPage();
                    XGraphics gfx = XGraphics.FromPdfPage(page);
                    XImage xImage;

                    using (MemoryStream memory = new MemoryStream())
                    {
                        qrCodeImage.Save(memory, ImageFormat.Png);
                        memory.Position = 0;
                        xImage = XImage.FromStream(memory);
                    }

                    // Calculate position to center the QR code on the page
                    double x = (page.Width.Point - (xImage.PixelWidth * 72 / xImage.HorizontalResolution)) / 2;
                    double y = (page.Height.Point - (xImage.PixelHeight * 72 / xImage.VerticalResolution)) / 2;

                    gfx.DrawImage(xImage, x, y, xImage.PixelWidth * 72 / xImage.HorizontalResolution, xImage.PixelHeight * 72 / xImage.VerticalResolution);
                    document.Save(pdfPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while generating the PDF: {ex.Message}");
                return null;
            }

            return pdfPath;
        }
        private void ShowErrorMessage(string message)
        {
            lblErrorMessage.Content = message;
            lblErrorMessage.Visibility = Visibility.Visible;
        }

    }
}
