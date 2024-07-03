using ScreenRecorderLib;

namespace ScreenRecorder
{
    public partial class ScreenRecorderForm : Form
    {
        private bool isMicEnabled = false;
        Recorder? _recorder;
        public ScreenRecorderForm()
        {
            InitializeComponent();
            stopBtn.Enabled = false;
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            using (var overlayForm = new OverlayForm())
            {
                if (overlayForm.ShowDialog() == DialogResult.OK)
                {
                    var selection = overlayForm.SelectionRectangle;
                    var selectedMonitorIndex = overlayForm.SelectedMonitorIndex;

                    StartRecording(selection, selectedMonitorIndex);
                }
            }
        }

        private void StartRecording(Rectangle selection, int selectedMonitorIndex)
        {
            SaveFileDialog saveFileDialog = new()
            {
                Filter = "MP4 file|*.mp4",
                Title = "Save the recording",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileName = "test.mp4"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string videoPath = saveFileDialog.FileName;

                if (File.Exists(videoPath))
                {
                    File.Delete(videoPath);
                }
                RecorderOptions options = ConfigureRecordingOptions(selection, selectedMonitorIndex);

                _recorder = Recorder.CreateRecorder(options);
                _recorder.OnRecordingComplete += RecordingComplete!;
                _recorder.OnRecordingFailed += RecordingFailed!;

                microphoneBtn.Enabled = false;

                _recorder.Record(videoPath);
                stopBtn.Enabled = true;  // Enable the stop button when recording starts
                startBtn.Enabled = false;
            }
            else
            {
                MessageBox.Show("Recording cancelled.");
                ResetButtonStates();
            }
        }

        private RecorderOptions ConfigureRecordingOptions(Rectangle selection, int selectedMonitorIndex)
        {
            var screenBounds = Screen.AllScreens[selectedMonitorIndex].Bounds;

            var sources = new List<RecordingSourceBase>
            {
                Recorder.GetDisplays()[selectedMonitorIndex]
            };

            var options = new RecorderOptions
            {
                SourceOptions = new SourceOptions
                {
                    RecordingSources = sources,
                },
                OutputOptions = new OutputOptions
                {
                    SourceRect = new ScreenRect(
                        -screenBounds.Left + selection.X,
                        -screenBounds.Top + selection.Y,
                        selection.Width,
                        selection.Height),
                    RecorderMode = RecorderMode.Video,
                    OutputFrameSize = new ScreenSize(selection.Width, selection.Height),
                    Stretch = StretchMode.Uniform,
                },
                AudioOptions = new AudioOptions
                {
                    IsAudioEnabled = true,
                    IsInputDeviceEnabled = isMicEnabled,
                    AudioInputDevice = Recorder.GetSystemAudioDevices(AudioDeviceSource.InputDevices).FirstOrDefault()?.DeviceName,
                },
                VideoEncoderOptions = new VideoEncoderOptions
                {
                    Bitrate = 8000 * 1000,
                    Framerate = 60,
                    IsFixedFramerate = true,
                    Encoder = new H264VideoEncoder
                    {
                        BitrateMode = H264BitrateControlMode.CBR,
                        EncoderProfile = H264Profile.Main
                    },
                    IsFragmentedMp4Enabled = true,
                    IsThrottlingDisabled = false,
                    IsHardwareEncodingEnabled = true,
                    IsLowLatencyEnabled = false,
                    IsMp4FastStartEnabled = false
                },
                MouseOptions = new MouseOptions
                {
                    IsMouseClicksDetected = true,
                    MouseLeftClickDetectionColor = "#FFFF00",
                    MouseRightClickDetectionColor = "#FFFF00",
                    MouseClickDetectionRadius = 30,
                    MouseClickDetectionDuration = 100,
                    IsMousePointerEnabled = true,
                    MouseClickDetectionMode = MouseDetectionMode.Hook
                },
                SnapshotOptions = new SnapshotOptions
                {
                    SnapshotsWithVideo = false,
                    SnapshotsIntervalMillis = 1000,
                    SnapshotFormat = ImageFormat.PNG,
                    SnapshotsDirectory = ""
                },
                LogOptions = new LogOptions
                {
                    IsLogEnabled = true,
                    LogFilePath = "recorder.log",
                    LogSeverityLevel = LogLevel.Debug
                }
            };

            return options;
        }

        private void RecordingComplete(object sender, RecordingCompleteEventArgs e)
        {
            MessageBox.Show("Recording complete: " + e.FilePath);
            ResetButtonStates();
        }

        private void RecordingFailed(object sender, RecordingFailedEventArgs e)
        {
            MessageBox.Show("Recording failed: " + e.Error);
            ResetButtonStates();
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            if (_recorder != null)
            {
                _recorder.Stop();
                _recorder.Dispose();
                _recorder = null!;

                ResetButtonStates();
            }
        }

        private void ResetButtonStates()
        {
            microphoneBtn.Enabled = true;
            stopBtn.Enabled = false;
            startBtn.Enabled = true;
        }

        private void microphoneBtn_Click(object sender, EventArgs e)
        {
            isMicEnabled = !isMicEnabled;
            microphoneBtn.Image = isMicEnabled ? Properties.Resources.icons8_mic_50 : Properties.Resources.icons8_block_microphone_50;
        }
    }
}
