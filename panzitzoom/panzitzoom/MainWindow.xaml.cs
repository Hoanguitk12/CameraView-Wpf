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
using Ozeki.Media;
using Ozeki.Camera;
using System.Globalization;
using System.Drawing;
using Ozeki.Vision;
using System.Diagnostics;

namespace panzitzoom
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IIPCamera _camera;
        private DrawingImageProvider _drawingImageProvider =new DrawingImageProvider();
        private MediaConnector _connector = new MediaConnector();

        private List<DiscoveredDeviceInfo> _deviceList;
        public MainWindow()
        {
            InitializeComponent();
            _drawingImageProvider = new DrawingImageProvider();
            _connector = new MediaConnector();
            videoViewer.SetImageProvider(_drawingImageProvider);
            IPCameraFactory.DeviceDiscovered += IPCamera_DiscoveryCompleted;
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
           // _camera = new IPCamera(txtIp.Text, txtUser.Text, txtPass.Text);
            _camera = IPCameraFactory.GetCamera(txtIp.Text, txtUser.Text, txtPass.Text);

            _connector.Connect(_camera.VideoChannel, _drawingImageProvider);
            _camera.Start();
            videoViewer.Start();
            
        }

        private void MouseDownMove(object sender, MouseButtonEventArgs e)
        {
            var button = sender as Button;
            if (button != null) Move(button.Content.ToString());
        }

        private void MouseUpMove(object sender, MouseButtonEventArgs e)
        {
            if (_camera != null)
                _camera.CameraMovement.StopMovement();
        }

        private void Move(string direction)
        {
            if (_camera == null) return;
            switch (direction)
            {
                case "Up Left":
                    _camera.CameraMovement.ContinuousMove(MoveDirection.LeftUp);
                    break;
                case "Up":
                    _camera.CameraMovement.ContinuousMove(MoveDirection.Up);
                    break;
                case "Up Right":
                    _camera.CameraMovement.ContinuousMove(MoveDirection.RightUp);
                    break;
                case "Left":
                    _camera.CameraMovement.ContinuousMove(MoveDirection.Left);
                    break;
                case "Right":
                    _camera.CameraMovement.ContinuousMove(MoveDirection.Right);
                    break;
                case "Down Left":
                    _camera.CameraMovement.ContinuousMove(MoveDirection.LeftDown);
                    break;
                case "Down":
                    _camera.CameraMovement.ContinuousMove(MoveDirection.Down);
                    break;
                case "Down Right":
                    _camera.CameraMovement.ContinuousMove(MoveDirection.RightDown);
                    break;
                case "In":
                    //_camera.CameraMovement.Zoom(CameraMovement.In);
                    _camera.CameraMovement.Zoom(MoveDirection.In);
                    break;
                case "Out":
                    _camera.CameraMovement.Zoom(MoveDirection.Out);
                    break;
            }
        }
        private void button_Discover_Click(object sender, RoutedEventArgs e)
        {
            _deviceList = new List<DiscoveredDeviceInfo>();

            InvokeGuiThread(() => comboBox_Devices.Items.Clear());

            IPCameraFactory.DiscoverDevices();
        }

        private void IPCamera_DiscoveryCompleted(object sender, DiscoveryEventArgs e)
        {
            _deviceList.Add(e.Device);
            InvokeGuiThread(() => comboBox_Devices.Items.Add(e.Device.Name));
        }

        private void comboBox_Devices_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            InvokeGuiThread(() =>
            {
                var selected = comboBox_Devices.SelectedIndex;
                if (selected < 0 || selected > _deviceList.Count - 1) return;
                textBox_Host.Text = _deviceList[selected].Host;
                textBox_Port.Text = _deviceList[selected].Port.ToString();
            });
        }
        // Setting the initial values of the scroll bars
        private void Camera_CameraStateChanged(object sender, CameraStateEventArgs e)
        {
            switch (e.State)
            {
                case CameraState.Streaming:
                    InvokeGUI(() =>
                    {
                        if (_camera.ImagingSettings.BrightnessInterval != null)
                        {
                            TrackBarBrightness.Minimum = (int)_camera.ImagingSettings.BrightnessInterval.Min;
                            TrackBarBrightness.Maximum = (int)_camera.ImagingSettings.BrightnessInterval.Max;
                            TrackBarBrightness.Value = (int)_camera.ImagingSettings.Brightness;
                        }
                        if (_camera.ImagingSettings.ContrastInterval != null)
                        {
                            TrackBarContrast.Minimum = (int)_camera.ImagingSettings.ContrastInterval.Min;
                            TrackBarContrast.Maximum = (int)_camera.ImagingSettings.ContrastInterval.Max;
                            TrackBarContrast.Value = (int)_camera.ImagingSettings.Contrast;
                        }
                        if (_camera.ImagingSettings.ColorSaturationInterval != null)
                        {
                            TrackBarSaturation.Minimum = (int)_camera.ImagingSettings.ColorSaturationInterval.Min;
                            TrackBarSaturation.Maximum = (int)_camera.ImagingSettings.ColorSaturationInterval.Max;
                            TrackBarSaturation.Value = (int)_camera.ImagingSettings.ColorSaturation;
                        }

                        BrightnessLabel.Content = _camera.ImagingSettings.Brightness.ToString(CultureInfo.InvariantCulture);
                        ContrastLabel.Content = _camera.ImagingSettings.Contrast.ToString(CultureInfo.InvariantCulture);
                        SaturationLabel.Content = _camera.ImagingSettings.ColorSaturation.ToString(CultureInfo.InvariantCulture);
                    });

                    break;
            }
        }

        // Setting the camera's image
        private void TrackBarBrightness_Scroll(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_camera != null)
            {
                _camera.ImagingSettings.SetAttributes(new CameraImaging { Brightness = Convert.ToSingle(TrackBarBrightness.Value) });
                _camera.ImagingSettings.RefreshProperties();
                BrightnessLabel.Content = _camera.ImagingSettings.Brightness.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void TrackBarContrast_Scroll(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_camera != null)
            {
                _camera.ImagingSettings.SetAttributes(new CameraImaging { Contrast = Convert.ToSingle(TrackBarContrast.Value) });
                _camera.ImagingSettings.RefreshProperties();
                ContrastLabel.Content = _camera.ImagingSettings.Contrast.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void TrackBarSaturation_Scroll(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_camera != null)
            {
                _camera.ImagingSettings.SetAttributes(new CameraImaging { ColorSaturation = Convert.ToSingle(TrackBarSaturation.Value) });
                _camera.ImagingSettings.RefreshProperties();
                SaturationLabel.Content = _camera.ImagingSettings.ColorSaturation.ToString(CultureInfo.InvariantCulture);
            }
        }
        private void InvokeGuiThread(Action action)
        {
            Dispatcher.BeginInvoke(action);
        }
        private void InvokeGUI(Action action)
        {
            Dispatcher.BeginInvoke(action);
        }

        //Cameraviewwr
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CameraViewer cam = new CameraViewer();
                cam.Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            CameraViewer cam = new CameraViewer();
            cam.Show();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            videoViewer.Focus();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            // HelpView h = new HelpView();
            //h.Show();
            Process.Start("http://www.camera-sdk.com/p_6632-supported-cameras.html?fbclid=IwAR12wAzzt_zuSk7n1YOMtLsfMRPmafOoUQTG9bEbxgoeblTBqCGGIocyrY0");
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            AuthorInfoView a = new AuthorInfoView();
            a.Show();
        }
    }
}
