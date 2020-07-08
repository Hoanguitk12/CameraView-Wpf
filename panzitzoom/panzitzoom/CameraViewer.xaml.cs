using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Ozeki.Camera;
using Ozeki.Media;
using Ozeki.VoIP;
namespace panzitzoom
{
    /// <summary>
    /// Interaction logic for CameraViewer.xaml
    /// </summary>
    public partial class CameraViewer : Window
    {
        // khoi tao các camera
        private IIPCamera _camera1;
        private DrawingImageProvider _drawingImageProvider1;
        private MediaConnector _connector1;
        private MPEG4Recorder _mpeg4Recorder1;
        private FolderBrowserDialog _folderBrowserDialog1;

        private IIPCamera _camera2;
        private DrawingImageProvider _drawingImageProvider2;
        private MediaConnector _connector2;
        private MPEG4Recorder _mpeg4Recorder2;
        private FolderBrowserDialog _folderBrowserDialog2;

        private IIPCamera _camera3;
        private DrawingImageProvider _drawingImageProvider3;
        private MediaConnector _connector3;
        private MPEG4Recorder _mpeg4Recorder3;
        private FolderBrowserDialog _folderBrowserDialog3;

        private IIPCamera _camera4;
        private DrawingImageProvider _drawingImageProvider4;
        private MediaConnector _connector4;
        private MPEG4Recorder _mpeg4Recorder4;
        private FolderBrowserDialog _folderBrowserDialog4;
        public CameraViewer()
        {
            InitializeComponent();
            //cam1
            _drawingImageProvider1 = new DrawingImageProvider();
            _connector1 = new MediaConnector();
            viewVid1.SetImageProvider(_drawingImageProvider1);
            _folderBrowserDialog1 = new FolderBrowserDialog();
            //cam2
            _drawingImageProvider2 = new DrawingImageProvider();
            _connector2 = new MediaConnector();
            viewVid2.SetImageProvider(_drawingImageProvider2);
            _folderBrowserDialog2 = new FolderBrowserDialog();
            //cam3
            _drawingImageProvider3 = new DrawingImageProvider();
            _connector3 = new MediaConnector();
            viewVid3.SetImageProvider(_drawingImageProvider3);
            _folderBrowserDialog3 = new FolderBrowserDialog();
            //cam4
            _drawingImageProvider4 = new DrawingImageProvider();
            _connector4 = new MediaConnector();
            viewVid4.SetImageProvider(_drawingImageProvider4);
            _folderBrowserDialog4 = new FolderBrowserDialog();
        }
        // luong xu ly cam1
        private void Record1_Click(object sender, RoutedEventArgs e)
        {
            var path = txtPath1.Text;
            if (!String.IsNullOrEmpty(path))
                StartVideoCapture1(path);
        }

        private void Stop1_Click(object sender, RoutedEventArgs e)
        {
            StopVideoCapture1();
        }

        private void ConnectCam1_Click(object sender, RoutedEventArgs e)
        {
            _camera1 = IPCameraFactory.GetCamera(IpCam1.Text, UseCam1.Text, PassCam1.Text);

            _connector1.Connect(_camera1.VideoChannel, _drawingImageProvider1);
            _camera1.Start();
            viewVid1.Start();
        }

        private void FilePath1_Click(object sender, RoutedEventArgs e)
        {
            var result = _folderBrowserDialog1.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
                txtPath1.Text = _folderBrowserDialog1.SelectedPath;
        }

        private void StartVideoCapture1(string path)
        {
            var date = DateTime.Now.Year + "y-" + DateTime.Now.Month + "m-" + DateTime.Now.Day + "d-" +
                       DateTime.Now.Hour + "h-" + DateTime.Now.Minute + "m-" + DateTime.Now.Second + "s";
            string currentpath;
            if (String.IsNullOrEmpty(path))
                currentpath = date + ".mp4";
            else
                currentpath = path + "\\" + date + ".mp4";

            _mpeg4Recorder1 = new MPEG4Recorder(currentpath);
            _mpeg4Recorder1.MultiplexFinished += Mpeg4Recorder_MultiplexFinished1;
            _connector1.Connect(_camera1.AudioChannel, _mpeg4Recorder1.AudioRecorder);
            _connector1.Connect(_camera1.VideoChannel, _mpeg4Recorder1.VideoRecorder);
        }

        private void Mpeg4Recorder_MultiplexFinished1(object sender, VoIPEventArgs<bool> e)
        {
            _connector1.Disconnect(_camera1.AudioChannel, _mpeg4Recorder1.AudioRecorder);
            _connector1.Disconnect(_camera1.VideoChannel, _mpeg4Recorder1.VideoRecorder);
            _mpeg4Recorder1.Dispose();
        }

        

        private void StopVideoCapture1()
        {
            if (_mpeg4Recorder1 != null)
            {
                _mpeg4Recorder1.Multiplex();
                _connector1.Disconnect(_camera1.AudioChannel, _mpeg4Recorder1.AudioRecorder);
                _connector1.Disconnect(_camera1.VideoChannel, _mpeg4Recorder1.VideoRecorder);
            }
        }

        // luong xu ly cam 2
        private void Record2_Click(object sender, RoutedEventArgs e)
        {
            var path = txtPath2.Text;
            if (!String.IsNullOrEmpty(path))
                StartVideoCapture2(path);
        }

        private void Stop2_Click(object sender, RoutedEventArgs e)
        {
            StopVideoCapture2();
        }

        private void ConnectCam2_Click(object sender, RoutedEventArgs e)
        {
            _camera2 = IPCameraFactory.GetCamera(IpCam2.Text, UseCam2.Text, PassCam2.Text);

            _connector2.Connect(_camera2.VideoChannel, _drawingImageProvider2);
            _camera2.Start();
            viewVid2.Start();
        }

        private void FilePath2_Click(object sender, RoutedEventArgs e)
        {
            var result = _folderBrowserDialog2.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
                txtPath2.Text = _folderBrowserDialog2.SelectedPath;
        }
        private void StartVideoCapture2(string path)
        {
            var date = DateTime.Now.Year + "y-" + DateTime.Now.Month + "m-" + DateTime.Now.Day + "d-" +
                       DateTime.Now.Hour + "h-" + DateTime.Now.Minute + "m-" + DateTime.Now.Second + "s";
            string currentpath;
            if (String.IsNullOrEmpty(path))
                currentpath = date + ".mp4";
            else
                currentpath = path + "\\" + date + ".mp4";

            _mpeg4Recorder2 = new MPEG4Recorder(currentpath);
            _mpeg4Recorder2.MultiplexFinished += Mpeg4Recorder_MultiplexFinished2;
            _connector2.Connect(_camera2.AudioChannel, _mpeg4Recorder2.AudioRecorder);
            _connector2.Connect(_camera2.VideoChannel, _mpeg4Recorder2.VideoRecorder);
        }

        private void Mpeg4Recorder_MultiplexFinished2(object sender, VoIPEventArgs<bool> e)
        {
            _connector2.Disconnect(_camera2.AudioChannel, _mpeg4Recorder2.AudioRecorder);
            _connector2.Disconnect(_camera2.VideoChannel, _mpeg4Recorder2.VideoRecorder);
            _mpeg4Recorder2.Dispose();
        }



        private void StopVideoCapture2()
        {
            if (_mpeg4Recorder2 != null)
            {
                _mpeg4Recorder2.Multiplex();
                _connector2.Disconnect(_camera2.AudioChannel, _mpeg4Recorder2.AudioRecorder);
                _connector2.Disconnect(_camera2.VideoChannel, _mpeg4Recorder2.VideoRecorder);
            }
        }

        // luong xu ly cam 3
        private void Record3_Click(object sender, RoutedEventArgs e)
        {
            var path = txtPath3.Text;
            if (!String.IsNullOrEmpty(path))
                StartVideoCapture3(path);
        }

        private void Stop3_Click(object sender, RoutedEventArgs e)
        {
            StopVideoCapture3();
        }

        private void ConnectCam3_Click(object sender, RoutedEventArgs e)
        {
            _camera3 = IPCameraFactory.GetCamera(IpCam3.Text, UseCam3.Text, PassCam3.Text);

            _connector3.Connect(_camera3.VideoChannel, _drawingImageProvider3);
            _camera3.Start();
            viewVid3.Start();
        }

        private void FilePath3_Click(object sender, RoutedEventArgs e)
        {
            var result = _folderBrowserDialog3.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
                txtPath3.Text = _folderBrowserDialog3.SelectedPath;
        }
        private void StartVideoCapture3(string path)
        {
            var date = DateTime.Now.Year + "y-" + DateTime.Now.Month + "m-" + DateTime.Now.Day + "d-" +
                       DateTime.Now.Hour + "h-" + DateTime.Now.Minute + "m-" + DateTime.Now.Second + "s";
            string currentpath;
            if (String.IsNullOrEmpty(path))
                currentpath = date + ".mp4";
            else
                currentpath = path + "\\" + date + ".mp4";

            _mpeg4Recorder3 = new MPEG4Recorder(currentpath);
            _mpeg4Recorder3.MultiplexFinished += Mpeg4Recorder_MultiplexFinished3;
            _connector3.Connect(_camera3.AudioChannel, _mpeg4Recorder3.AudioRecorder);
            _connector3.Connect(_camera3.VideoChannel, _mpeg4Recorder3.VideoRecorder);
        }

        private void Mpeg4Recorder_MultiplexFinished3(object sender, VoIPEventArgs<bool> e)
        {
            _connector3.Disconnect(_camera3.AudioChannel, _mpeg4Recorder3.AudioRecorder);
            _connector3.Disconnect(_camera3.VideoChannel, _mpeg4Recorder3.VideoRecorder);
            _mpeg4Recorder3.Dispose();
        }



        private void StopVideoCapture3()
        {
            if (_mpeg4Recorder3 != null)
            {
                _mpeg4Recorder3.Multiplex();
                _connector3.Disconnect(_camera3.AudioChannel, _mpeg4Recorder3.AudioRecorder);
                _connector3.Disconnect(_camera3.VideoChannel, _mpeg4Recorder3.VideoRecorder);
            }
        }

        // luong xu ly cam 4
        private void Record4_Click(object sender, RoutedEventArgs e)
        {
            var path = txtPath4.Text;
            if (!String.IsNullOrEmpty(path))
                StartVideoCapture4(path);
        }

        private void Stop4_Click(object sender, RoutedEventArgs e)
        {
            StopVideoCapture4();
        }

        private void ConnectCam4_Click(object sender, RoutedEventArgs e)
        {
            _camera4 = IPCameraFactory.GetCamera(IpCam4.Text, UseCam4.Text, PassCam4.Text);

            _connector4.Connect(_camera4.VideoChannel, _drawingImageProvider4);
            _camera4.Start();
            viewVid4.Start();
        }

        private void FilePath4_Click(object sender, RoutedEventArgs e)
        {
            var result = _folderBrowserDialog4.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
                txtPath4.Text = _folderBrowserDialog4.SelectedPath;
        }

        private void StartVideoCapture4(string path)
        {
            var date = DateTime.Now.Year + "y-" + DateTime.Now.Month + "m-" + DateTime.Now.Day + "d-" +
                       DateTime.Now.Hour + "h-" + DateTime.Now.Minute + "m-" + DateTime.Now.Second + "s";
            string currentpath;
            if (String.IsNullOrEmpty(path))
                currentpath = date + ".mp4";
            else
                currentpath = path + "\\" + date + ".mp4";

            _mpeg4Recorder4 = new MPEG4Recorder(currentpath);
            _mpeg4Recorder4.MultiplexFinished += Mpeg4Recorder_MultiplexFinished4;
            _connector4.Connect(_camera4.AudioChannel, _mpeg4Recorder4.AudioRecorder);
            _connector4.Connect(_camera4.VideoChannel, _mpeg4Recorder4.VideoRecorder);
        }

        private void Mpeg4Recorder_MultiplexFinished4(object sender, VoIPEventArgs<bool> e)
        {
            _connector4.Disconnect(_camera4.AudioChannel, _mpeg4Recorder4.AudioRecorder);
            _connector4.Disconnect(_camera4.VideoChannel, _mpeg4Recorder4.VideoRecorder);
            _mpeg4Recorder4.Dispose();
        }



        private void StopVideoCapture4()
        {
            if (_mpeg4Recorder4 != null)
            {
                _mpeg4Recorder4.Multiplex();
                _connector4.Disconnect(_camera4.AudioChannel, _mpeg4Recorder4.AudioRecorder);
                _connector4.Disconnect(_camera4.VideoChannel, _mpeg4Recorder4.VideoRecorder);
            }
        }

        // luong xu ly cam 4



    }
}
