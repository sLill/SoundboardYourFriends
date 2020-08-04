using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using SoundboardYourFriends.View;
using SoundboardYourFriends.Core;
using System.Runtime.InteropServices;
using System.ComponentModel;
using SoundboardYourFriends.Model;
using System.Collections.ObjectModel;
using System.Linq;
using NAudio.Wave;
using System.IO;
using SoundboardYourFriends.View.Windows;
using NAudio.CoreAudioApi;
using System.Text.RegularExpressions;
using System.Threading;

namespace SoundboardYourFriends.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        #region Member Variables..
        private HwndSource _hwndSource;
        private const int RECORD_HOTKEY_ID = 9000;
        #endregion Member Variables..

        #region Properties..
        #region RecordHotkey
        private Key? _recordHotKey;
        public Key? RecordHotkey
        {
            get { return _recordHotKey; }
            set
            {
                _recordHotKey = value;
                RecordHotkeyDisplay = _recordHotKey.ToString();
                RaisePropertyChanged();
            }
        }
        #endregion RecordHotkey

        #region RecordHotkeyDisplay
        private string _recordHotKeyDisplay = "Unassigned";
        public string RecordHotkeyDisplay
        {
            get { return $"CTRL + {_recordHotKeyDisplay}"; }
            set
            {
                _recordHotKeyDisplay = value;
                RaisePropertyChanged();
            }
        }
        #endregion RecordHotkeyDisplay

        #region SelectedListeningDevicesCollection
        private ObservableCollection<AudioDevice> _selectedListeningDevicesCollection = new ObservableCollection<AudioDevice>();
        public ObservableCollection<AudioDevice> SelectedListeningDevicesCollection
        {
            get { return _selectedListeningDevicesCollection; }
            set
            {
                _selectedListeningDevicesCollection = value;
                RaisePropertyChanged();
            }
        }
        #endregion SelectedListeningDevicesCollection

        #region SelectedOutputDevicesCollection
        private ObservableCollection<AudioDevice> _selectedOutputDevicesCollection = new ObservableCollection<AudioDevice>();
        public ObservableCollection<AudioDevice> SelectedOutputDevicesCollection
        {
            get { return _selectedOutputDevicesCollection; }
            set
            {
                _selectedOutputDevicesCollection = value;
                RaisePropertyChanged();
            }
        }
        #endregion SelectedOutputDevicesCollection

        #region SelectedSoundboardSample
        private SoundboardSample _selectedSoundboardSample;
        public SoundboardSample SelectedSoundboardSample
        { 
            get { return _selectedSoundboardSample; }
            set 
            { 
                _selectedSoundboardSample = value;
                RaisePropertyChanged();
            }
        }
        #endregion SelectedSoundboardSample

        #region SoundboardSampleCollection
        private ObservableCollection<SoundboardSample> _soundboardSampleCollection = new ObservableCollection<SoundboardSample>();
        public ObservableCollection<SoundboardSample> SoundboardSampleCollection 
        { 
            get { return _soundboardSampleCollection; }
            set 
            { 
                _soundboardSampleCollection = value;
                RaisePropertyChanged();
            } 
        }
        #endregion SoundboardSampleCollection
        #endregion Properties..

        #region Constructors..
        #region MainWindowViewModel
        public MainWindowViewModel()
        {
            SetEvents();
        }
        #endregion MainWindowViewModel
        #endregion Constructors..

        #region Methods..
        #region Events..
        #region OnAudioMeterTimerElapsed
        private void OnAudioMeterTimerElapsed(object sender, EventArgs e)
        {
            using (var deviceEnumerator = new MMDeviceEnumerator())
            {
                MMDevice defaultRenderDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Communications);

                if (SelectedListeningDevicesCollection.Any())
                {
                    SelectedListeningDevicesCollection[0].AudioPeak = (int)(WasapiLoopbackCapture.GetDefaultLoopbackCaptureDevice().AudioMeterInformation.MasterPeakValue * 100);
                }
                //OutputAudioPeak = (int)(defaultRenderDevice.AudioMeterInformation.MasterPeakValue * 100);
            }
        }
        #endregion OnAudioMeterTimerElapsed

        #region OnFileWritten
        public void OnFileWritten(object sender, EventArgs e)
        {
            string filePath = sender as string;
            string fileName = Path.GetFileNameWithoutExtension(filePath);

            double totalSeconds = AudioAgent.GetFileAudioDuration(filePath).TotalSeconds;
            SoundboardSample TestRecording = new SoundboardSample()
            {
                Name = fileName,
                FilePath = filePath,
                GroupName = "Ungrouped",
                FileTimeMax = totalSeconds,
                FileTimeMin = 0,
                FileTimeUpperValue = totalSeconds,
                FileTimeLowerValue = 0
            };

            SoundboardSampleCollection.Add(TestRecording);
        }
        #endregion OnFileWritten
        #endregion Events..

        #region Windows..
        #region RegisterHotKey
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vlc);
        #endregion RegisterHotKey

        #region UnregisterHotKey
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        #endregion UnregisterHotKey

        #region HwndHook
        public IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:

                    switch (wParam.ToInt32())
                    {
                        case RECORD_HOTKEY_ID:
                            int vkey = (((int)lParam >> 16) & 0xFFFF);
                            uint keyCode = Convert.ToUInt32(KeyInterop.VirtualKeyFromKey(RecordHotkey.Value).ToString("X"), 16);

                            if (vkey == keyCode)
                            { 
                                AudioAgent.WriteAudioBufferToFile();
                            }

                            handled = true;
                            break;
                    }
                    break;
            }

            return IntPtr.Zero;
        }
        #endregion HwndHook
        #endregion Windows..

        #region Closing
        public void Closing()
        {
            AudioAgent.StopListening();
        }
        #endregion Closing

        #region DeleteSample
        public void DeleteSample(SoundboardSample soundboardSample)
        {
            File.Delete(soundboardSample.FilePath);
            SoundboardSampleCollection.Remove(soundboardSample);
        }
        #endregion DeleteSample

        #region LoadAudioDevices
        private void LoadAudioDevices()
        {
            // Listening Devices
            //if (ApplicationConfiguration.DefaultListeningDeviceIds.Any())
            //{
            //    var audioDeviceCollection = new List<AudioDevice>();
            //    for (int i = 0; i < WaveIn.DeviceCount; i++)
            //    {
            //        var deviceProperties = WaveIn.GetCapabilities(i);
            //        audioDeviceCollection.Add(new AudioDevice()
            //        {
            //            FriendlyName = deviceProperties.ProductName,
            //            DeviceId = i,
            //            NameGuid = deviceProperties.NameGuid
            //        });
            //    }

            //    ApplicationConfiguration.DefaultListeningDeviceIds.ForEach(deviceId =>
            //    {
            //        var matchedDevice = audioDeviceCollection.FirstOrDefault(x => x.NameGuid == deviceId);
            //        if (matchedDevice != null)
            //        {
            //            SelectedListeningDevicesCollection.Add(matchedDevice);
            //        }
            //    });
            //}
            //else
            //{
            //    int deviceId = 0;
            //    using (var deviceEnumerator = new MMDeviceEnumerator())
            //    {
            //        // Use the windows default listening device if one has not been setup
            //        var systemDefaultListeningDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);
            //        SelectedListeningDevicesCollection.Add(new AudioDevice()
            //        {
            //            FriendlyName = systemDefaultListeningDevice.FriendlyName,
            //            DeviceId = deviceId
            //        });

            //        deviceId++;
            //    }
            //}

            //// Output Devices
            //if (ApplicationConfiguration.DefaultOutputDeviceIds.Any())
            //{
            //    var audioDeviceCollection = new List<AudioDevice>();
            //    for (int i = 0; i < WaveOut.DeviceCount; i++)
            //    {
            //        var deviceProperties = WaveOut.GetCapabilities(i);
            //        audioDeviceCollection.Add(new AudioDevice()
            //        {
            //            FriendlyName = deviceProperties.ProductName,
            //            DeviceId = i,
            //            NameGuid = deviceProperties.NameGuid
            //        });
            //    }

            //    ApplicationConfiguration.DefaultOutputDeviceIds.ForEach(deviceId =>
            //    {
            //        var matchedDevice = audioDeviceCollection.FirstOrDefault(x => x.NameGuid == deviceId);
            //        if (matchedDevice != null)
            //        {
            //            SelectedOutputDevicesCollection.Add(matchedDevice);
            //        }
            //    });
            //}
            //else
            //{
            //    int deviceId = 0;
            //    using (var deviceEnumerator = new MMDeviceEnumerator())
            //    { 
            //        // Use the windows default output device if one has not been setup
            //        var systemDefaultOutputDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Communications);
            //        SelectedOutputDevicesCollection.Add(new AudioDevice()
            //        {
            //            FriendlyName = systemDefaultOutputDevice.FriendlyName,
            //            DeviceId = deviceId
            //        });

            //        deviceId++;
            //    }
            //}

        }
        #endregion LoadAudioDevices

        #region LoadAudioSamples
        public void LoadAudioSamples()
        {
            foreach (string audioSamplePath in Directory.GetFiles(ApplicationConfiguration.SoundboardSampleDirectory, "*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(ApplicationConfiguration.SoundboardSampleDirectory, audioSamplePath);
                string[] directorySplit = relativePath.Split('\\');
                double totalSeconds = AudioAgent.GetFileAudioDuration(audioSamplePath).TotalSeconds;

                string groupName = "Ungrouped";
                if (directorySplit.Length > 1)
                {
                    groupName = directorySplit[0];
                }

                _soundboardSampleCollection.Add(new SoundboardSample()
                {
                    Name = Path.GetFileNameWithoutExtension(audioSamplePath),
                    FilePath = audioSamplePath,
                    GroupName = groupName,
                    FileTimeMax = totalSeconds,
                    FileTimeMin = 0,
                    FileTimeUpperValue = totalSeconds,
                    FileTimeLowerValue = 0
                });
            }
        }
        #endregion LoadAudioSamples

        #region LoadConfig
        public void LoadConfig(Window applicationWindow)
        {
            LoadAudioDevices();

            RegisterRecordHotKey(new WindowInteropHelper(applicationWindow).Handle, Key.Up);
        }
        #endregion LoadConfig

        #region PlayAudioSample
        public void PlayAudioSample(SoundboardSample soundboardSample, PlaybackType playbackType)
        {
            AudioAgent.BeginAudioPlayback(soundboardSample.FilePath, playbackType, soundboardSample.FileTimeLowerValue, soundboardSample.FileTimeUpperValue);
        }
        #endregion PlayAudioSample

        #region RegisterRecordHotKey
        public void RegisterRecordHotKey(IntPtr viewHandle, Key key)
        {
            RecordHotkey = key;

            _hwndSource = HwndSource.FromHwnd(viewHandle);
            _hwndSource.AddHook(HwndHook);

            // Modifiers
            //const uint MOD_NONE = 0x0000; // (none)
            //const uint MOD_ALT = 0x0001; // ALT
            const uint MOD_CONTROL = 0x0002; // CTRL
            //const uint MOD_SHIFT = 0x0004; // SHIFT
            //const uint MOD_WIN = 0x0008; // WINDOWS

            uint keyCode = Convert.ToUInt32(KeyInterop.VirtualKeyFromKey(key).ToString("X"), 16);
            if (!RegisterHotKey(viewHandle, RECORD_HOTKEY_ID, MOD_CONTROL, keyCode))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
        #endregion RegisterRecordHotKey

        #region SaveSample
        public void SaveSample(SoundboardSample soundboardSample)
        {
            // File length
            if (soundboardSample.FileTimeUpperValue != soundboardSample.FileTimeMax || soundboardSample.FileTimeLowerValue != soundboardSample.FileTimeMin)
            {
                AudioAgent.TrimFile(soundboardSample.FilePath, soundboardSample.FileTimeLowerValue, soundboardSample.FileTimeUpperValue);

                soundboardSample.FileTimeMin = 0;
                soundboardSample.FileTimeMax = AudioAgent.GetFileAudioDuration(soundboardSample.FilePath).Seconds;
            }

            // File name
            if (Path.GetFileNameWithoutExtension(soundboardSample.FilePath) != soundboardSample.Name)
            {
                string newFilePath = Path.Combine(ApplicationConfiguration.SoundboardSampleDirectory, soundboardSample.GroupName, $"{soundboardSample.Name}.wav");
                File.Move(soundboardSample.FilePath, newFilePath);

                soundboardSample.FilePath = newFilePath;
            }
        }
        #endregion SaveSample

        #region SetAudioDevice
        public void SetAudioDevice(AudioDeviceType audioDeviceType)
        {
            using (AudioDeviceDialog audioDeviceDialog = new AudioDeviceDialog(audioDeviceType))
            {
                audioDeviceDialog.ShowDialog();

                ObservableCollection<AudioDevice> AudioDeviceCollection = audioDeviceDialog.SelectedAudioDevices == null 
                    ? new ObservableCollection<AudioDevice>() { new AudioDevice() { FriendlyName = "No device(s) selected" } } 
                    : new ObservableCollection<AudioDevice>(audioDeviceDialog.SelectedAudioDevices);

                AudioAgent.InitializeOutputDevices(AudioDeviceCollection.Select(audioDevice => audioDevice.DeviceId));

                switch (audioDeviceType)
                {
                    case AudioDeviceType.Input:
                        SelectedListeningDevicesCollection = AudioDeviceCollection;
                        break;
                    case AudioDeviceType.Output:
                        SelectedOutputDevicesCollection = AudioDeviceCollection;
                        break;
                }
            }
        }
        #endregion SetAudioDevice

        private void SetEvents()
        {
            AudioAgent.FileWritten += OnFileWritten;

            // Audio Meter Polling Rate
            var audioMeterUpdateTimer = new System.Timers.Timer(100);
            audioMeterUpdateTimer.Start();

            audioMeterUpdateTimer.Elapsed += OnAudioMeterTimerElapsed;
        }

        #region StopAudioPlayback
        public void StopAudioPlayback()
        {
            AudioAgent.StopAudioPlayback();
        }
        #endregion StopAudioPlayback

        #region UnregisterRecordHotkey
        public void UnregisterRecordHotkey(IntPtr viewHandle)
        {
            _hwndSource?.RemoveHook(HwndHook);
            UnregisterHotKey(viewHandle, RECORD_HOTKEY_ID);
        }
        #endregion UnregisterRecordHotkey
        #endregion Methods..
    }
}
