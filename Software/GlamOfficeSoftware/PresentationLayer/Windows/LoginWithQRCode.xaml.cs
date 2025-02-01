﻿using AForge.Video;
using AForge.Video.DirectShow;
using System;
using ZXing;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.Exceptions;
using System.Threading;

namespace PresentationLayer.Windows
{
    public partial class LoginWithQRCode : Window
    {
        private FilterInfoCollection CaptureDevice;
        private VideoCaptureDevice Finalframe;
        private DispatcherTimer Timer;
        private LoginOptions _loginOptions;
        private EmployeeService EmployeeService = new EmployeeService();

        public LoginWithQRCode(LoginOptions loginOptions)
        {
            InitializeComponent();
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(250);
            Timer.Tick += Timer_Tick;
            _loginOptions = loginOptions;
        }

        private bool isProcessingQRCode = false;

        private async void Timer_Tick(object sender, EventArgs e)
        {
            if (isProcessingQRCode || Finalframe == null || !Finalframe.IsRunning || imgCamera.Source == null)
                return;

            Bitmap bitmap = ConvertImageSourceToBitmap(imgCamera.Source);
            if (bitmap == null)
                return;

            string decodedText = await DecodeQRCodeAsync(bitmap);
            if (decodedText == null)
                return;

            await HandleLoginAsync(decodedText);
        }

        private async Task<string> DecodeQRCodeAsync(Bitmap bitmap)
        {
            return await Task.Run(() =>
            {
                BarcodeReader barcodeReader = new BarcodeReader();
                var result = barcodeReader.Decode(bitmap);
                if (result == null || string.IsNullOrWhiteSpace(result.Text))
                    return null;
                return result.Text.Trim();
            });
        }

        private async Task HandleLoginAsync(string decodedText)
        {
            if (isProcessingQRCode)
                return;

            isProcessingQRCode = true;

            try
            {
                var employee = await Task.Run(async () =>
                {
                    using (var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(5))) // Timeout od 5 sekundi
                    {
                        return await EmployeeService.LogInWithQRCodeAsync(decodedText);
                    }
                });

                if (employee != null)
                {
                    await StopCameraAsync();
                    Dispatcher.Invoke(() =>
                    {
                        var mainWindow = new MainWindow();
                        mainWindow.Show();
                        this.Hide();
                    });
                }
                else
                {
                    ShowErrorMessage("QR kod nije valjan ili ne sadrži ispravne podatke.");
                }
            }
            catch (TaskCanceledException)
            {
                ShowErrorMessage("Prijava putem QR koda je predugo trajala. Pokušajte ponovno.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Došlo je do greške: {ex.Message}");
            }
            finally
            {
                isProcessingQRCode = false;
            }
        }

        private void ShowErrorMessage(string message)
        {
            Dispatcher.Invoke(() =>
            {
                lblErrorMessage.Content = message;
                lblErrorMessage.Visibility = Visibility.Visible;
            });
        }

        private Bitmap ConvertImageSourceToBitmap(ImageSource source)
        {
            if (source is BitmapImage bitmapImage)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BitmapEncoder encoder = new BmpBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                    encoder.Save(stream);
                    return new Bitmap(stream);
                }
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

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            await StopCameraAsync();
        }

        private async Task StopCameraAsync()
        {
            await Dispatcher.InvokeAsync(() =>
            {
                if (Finalframe != null && Finalframe.IsRunning)
                {
                    Finalframe.SignalToStop();
                    Finalframe.WaitForStop();
                }
                Timer.Stop();
            });
        }


        private async void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            await StopCameraAsync();
            _loginOptions.Show();
            this.Hide();
            
        }
    }
}
