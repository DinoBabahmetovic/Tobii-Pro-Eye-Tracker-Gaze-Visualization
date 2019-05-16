using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using Tobii.Research;

namespace TobiiFirstApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            //Show();
            //Canvas myCanvas = canvasGaze;
            //Ellipse myEllipse = circleGaze;

            //Console.WriteLine("Width of the canvas is {0}", myCanvas.Width);
            //Canvas.SetLeft(myEllipse, 1870);
            //Canvas.SetTop(myEllipse, 0);
            int counter = 0;


            Ellipse ellipse = new Ellipse() { Width = 50, Height = 50, Fill = Brushes.OrangeRed };
            Ellipse ellipse2 = new Ellipse() { Width = 50, Height = 50, Fill = Brushes.Orange };
            MainCanvas.Children.Add(ellipse);
            MainCanvas.Children.Add(ellipse2);
            Console.WriteLine(System.Windows.SystemParameters.PrimaryScreenWidth);
            Console.WriteLine(System.Windows.SystemParameters.PrimaryScreenHeight);
            Canvas.SetLeft(ellipse, 1870);
            Canvas.SetTop(ellipse, 1030);

            EyeTrackerCollection eyeTrackers = EyeTrackingOperations.FindAllEyeTrackers();
            foreach (IEyeTracker eyeTr in eyeTrackers)
            {
                Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}", eyeTr.Address, eyeTr.DeviceName, eyeTr.Model, eyeTr.SerialNumber, eyeTr.FirmwareVersion, eyeTr.RuntimeVersion);
            }

            var eyeTracker = eyeTrackers.FirstOrDefault();





            // Start listening to HMD gaze data.
            eyeTracker.GazeDataReceived += (sender, e) => EyeTracker_HMDGazeDataReceived(sender, e, ellipse, ellipse2, ref counter);
            // Wait for some data to be received.
            //System.Threading.Thread.Sleep(5000);
            // Stop listening to HMD gaze data.
            //eyeTracker.GazeDataReceived -= (sender, e) => EyeTracker_HMDGazeDataReceived(sender, e, ellipse);

        }

        public static void EyeTracker_HMDGazeDataReceived(object sender, GazeDataEventArgs e, Ellipse el, Ellipse el2, ref int cnt)
        {
            if (cnt > 2)
            {
                Console.WriteLine(
                "Got gaze data with {0} left eye direction described by unit vector ({1}, {2}) in the coordinate system.",
                e.LeftEye.GazePoint.Validity,
                e.LeftEye.GazePoint.PositionOnDisplayArea.X,
                e.LeftEye.GazePoint.PositionOnDisplayArea.Y);
                if (e.LeftEye.GazePoint.Validity.ToString() == "Valid")
                {

                    /*Thread thread = new Thread(delegate ()
                    {
                        Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            Canvas.SetLeft(el, System.Windows.SystemParameters.PrimaryScreenWidth * e.LeftEye.GazePoint.PositionOnDisplayArea.X - 50);
                            Canvas.SetTop(el, System.Windows.SystemParameters.PrimaryScreenHeight * e.LeftEye.GazePoint.PositionOnDisplayArea.Y - 50);
                        }));
                    });
                    thread.IsBackground = true;
                    thread.Start();
                    */

                    /*Application.Current.Dispatcher.Invoke(() =>
                    {
                        Canvas.SetLeft(el, System.Windows.SystemParameters.PrimaryScreenWidth * e.LeftEye.GazePoint.PositionOnDisplayArea.X - 50);
                        Canvas.SetTop(el, System.Windows.SystemParameters.PrimaryScreenHeight * e.LeftEye.GazePoint.PositionOnDisplayArea.Y - 50);
                    });
                    */

                    if (Application.Current != null)
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            Canvas.SetLeft(el, System.Windows.SystemParameters.PrimaryScreenWidth * e.LeftEye.GazePoint.PositionOnDisplayArea.X - 50);
                            Canvas.SetTop(el, System.Windows.SystemParameters.PrimaryScreenHeight * e.LeftEye.GazePoint.PositionOnDisplayArea.Y - 50);
                        }), DispatcherPriority.Background);
                }
                if (e.RightEye.GazePoint.Validity.ToString() == "Valid")
                {
                    if (Application.Current != null)
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            Canvas.SetLeft(el2, System.Windows.SystemParameters.PrimaryScreenWidth * e.RightEye.GazePoint.PositionOnDisplayArea.X - 50);
                            Canvas.SetTop(el2, System.Windows.SystemParameters.PrimaryScreenHeight * e.RightEye.GazePoint.PositionOnDisplayArea.Y - 50);
                        }), DispatcherPriority.Background);
                }
                cnt = 0;
            }
            else cnt++;
            
        }
    }
}
