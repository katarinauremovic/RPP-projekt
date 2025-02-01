using AForge.Video;
using AForge.Video.DirectShow;
using System;
using ZXing;
using System.Drawing;
using System.IO;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.Exceptions;

namespace PresentationLayer.Windows
{
    /// <summary>
    /// Interaction logic for LoginWithQRCode.xaml
    /// </summary>
    public partial class LoginWithQRCode : Window
    {
        private FilterInfoCollection CaptureDevice;
        private VideoCaptureDevice Finalframe;
        private DispatcherTimer Timer;
        private LoginOptions _loginOptions;
        private EmployeeService EmployeeService= new EmployeeService();
        public LoginWithQRCode(LoginOptions loginOptions)
        {
            InitializeComponent();
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(500);
            Timer.Tick += Timer_Tick;
            _loginOptions = loginOptions;
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            if(imgCamera.Source != null)
            {
                Bitmap bitmap = ConvertImageSourceToBitmap(imgCamera.Source);
                BarcodeReader barcodeReader = new BarcodeReader();
                var result = barcodeReader.Decode(bitmap);
                if(result != null)
                {
                    try
                    {
                        string decodedText = result.Text.Trim();
                        var employee = await EmployeeService.LogInWithQRCodeAsync(decodedText);
                        if (employee != null)
                        {
                            var mainWindow = new MainWindow();
                            mainWindow.ShowDialog();
                            this.Hide();
                        }
                    }
                    catch (InvalidQRCodeFormatException ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private Bitmap ConvertImageSourceToBitmap(ImageSource source)
        {
            BitmapImage bitmapImage = source as BitmapImage;
            if (bitmapImage != null)
            {
                MemoryStream stream = new MemoryStream();
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(stream);

                return new Bitmap(stream);
            }

            return null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo Device in CaptureDevice)
            {
                cmbCamera.Items.Add(Device.Name);
            }

            cmbCamera.SelectedIndex = 0;
            Finalframe = new VideoCaptureDevice();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Finalframe = new VideoCaptureDevice(CaptureDevice[cmbCamera.SelectedIndex].MonikerString);
            Finalframe.NewFrame += new NewFrameEventHandler(FinalFrame_NewFrame);
            Finalframe.Start();
            Timer.Start();
        }

        private void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();

            Dispatcher.Invoke(() =>
            {
                BitmapImage bitmapImage = ConvertBitmapToBitmapImage(bitmap);
                imgCamera.Source = bitmapImage;
            });
        }
        private BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
                memoryStream.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); 

                return bitmapImage;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Finalframe.IsRunning)
            {
                Finalframe.Stop();
            }
            Timer.Stop();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _loginOptions.Show();
            this.Hide();
            if (Finalframe.IsRunning)
            {
                Finalframe.Stop();
            }
            Timer.Stop();
        }
    }
}
