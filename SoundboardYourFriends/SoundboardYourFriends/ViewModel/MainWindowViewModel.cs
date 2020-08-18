using NAudio.Wave;
using SoundboardYourFriends.Core;
using SoundboardYourFriends.Core.Windows;
using SoundboardYourFriends.Model;
using SoundboardYourFriends.View.Windows;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Interop;

namespace SoundboardYourFriends.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        #region Member Variables..
        private static HwndSource _hwndSource;
        private const int _recordHotkeyId = 90000;
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
            get { return $"{ApplicationConfiguration.GlobalKeyModifer} + {_recordHotKeyDisplay}"; }
            set
            {
                _recordHotKeyDisplay = value;
                RaisePropertyChanged();
            }
        }
        #endregion RecordHotkeyDisplay

        #region SelectedCaptureDevicesCollection
        private ObservableCollection<AudioDevice> _selectedCaptureDevicesCollection = new ObservableCollection<AudioDevice>();
        public ObservableCollection<AudioDevice> SelectedCaptureDevicesCollection
        {
            get { return _selectedCaptureDevicesCollection; }
            set
            {
                _selectedCaptureDevicesCollection = value;
                ApplicationConfiguration.DefaultCaptureDeviceIds = value.Select(x => x.DeviceId);

                RaisePropertyChanged();
            }
        }
        #endregion SelectedCaptureDevicesCollection

        #region SelectedOutputDevicesCollection
        private ObservableCollection<AudioDevice> _selectedOutputDevicesCollection = new ObservableCollection<AudioDevice>();
        public ObservableCollection<AudioDevice> SelectedOutputDevicesCollection
        {
            get { return _selectedOutputDevicesCollection; }
            set
            {
                _selectedOutputDevicesCollection = value;
                ApplicationConfiguration.DefaultOutputDeviceIds = value.Select(x => x.DeviceId);
                ApplicationConfiguration.OutputDevicePlaybackTypeCollection = value.ToDictionary(x => x.DeviceId, x => x.PlaybackType);

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

        #region WindowHandle
        private IntPtr _windowHandle;
        public IntPtr WindowHandle
        {
            get { return _windowHandle; }
            set { _windowHandle = value; }
        }
        #endregion WindowHandle
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
            if (SelectedCaptureDevicesCollection.Any())
            {
                SelectedCaptureDevicesCollection[0].AudioPeak = (int)(WasapiLoopbackCapture.GetDefaultLoopbackCaptureDevice().AudioMeterInformation.MasterPeakValue * 100);
            }

            SelectedOutputDevicesCollection.ToList().ForEach(outputDevice =>
            {
                outputDevice.AudioPeak = (int)(outputDevice.AudioMeterInformation.MasterPeakValue * 100);
            });
        }
        #endregion OnAudioMeterTimerElapsed

        #region OnFileWritten
        public void OnFileWritten(object sender, EventArgs e)
        {
            string filePath = sender as string;
            double totalSeconds = AudioAgent.GetFileAudioDuration(filePath).TotalSeconds;

            SoundboardSample NewSoundboardSample = new SoundboardSample(filePath)
            {
                GroupName = "Ungrouped",
                FileTimeMax = totalSeconds,
                FileTimeMin = 0,
                FileTimeUpperValue = totalSeconds,
                FileTimeLowerValue = 0,
                HotkeyId = SoundboardSampleCollection.Count
            };

            SoundboardSampleCollection.Add(NewSoundboardSample);
        }
        #endregion OnFileWritten
        #endregion Events..

        #region Closing
        public void Closing()
        {
            AudioAgent.StopCapture();
            UnregisterHotKeysAndHooks();

            ApplicationConfiguration.RecordHotkey = RecordHotkey.Value;
            SoundboardSampleCollection.ToList().ForEach(x => x.SaveMetadataProperties());
        }
        #endregion Closing

        #region DeleteSampleAsync
        public async Task DeleteSampleAsync(SoundboardSample soundboardSample)
        {
            // Recursively try to acquire file lock. This can sometimes take a few seconds to release after 
            // a audio playback event has ended
            bool success = false;
            while (!success)
            {
                try
                {
                    AudioAgent.StopAudioPlayback(SelectedOutputDevicesCollection.ToList());
                    File.Delete(soundboardSample.FilePath);
                    SoundboardSampleCollection.Remove(soundboardSample);

                    success = true;
                }
                catch (Exception ex)
                {
                    await Task.Delay(500);
                }
            }
        }
        #endregion DeleteSampleAsync

        #region HwndHook
        public IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:

                    // Record button
                    int virtualKeyParam = (((int)lParam >> 16) & 0xFFFF);
                    if (_recordHotKey != null && _recordHotKey != Key.None)
                    {
                        uint keyCode = Convert.ToUInt32(KeyInterop.VirtualKeyFromKey(_recordHotKey.Value).ToString("X"), 16);
                        if (_recordHotkeyId == wParam.ToInt32() && virtualKeyParam == keyCode)
                        {
                            AudioAgent.WriteAudioBufferToFile();
                        }
                    }

                    // Soundboard samples
                    var soundboardSample = SoundboardSampleCollection.Where(x => x.HotkeyId == wParam.ToInt32()).FirstOrDefault();
                    if (soundboardSample != null && soundboardSample.Hotkey != Key.None)
                    {
                        uint keyCode = Convert.ToUInt32(KeyInterop.VirtualKeyFromKey(soundboardSample.Hotkey).ToString("X"), 16);
                        if (soundboardSample.HotkeyId == wParam.ToInt32() && virtualKeyParam == keyCode)
                        {
                            AudioAgent.BeginAudioPlayback(soundboardSample.FilePath, SelectedOutputDevicesCollection.ToList(), PlaybackType.Global, soundboardSample.FileTimeLowerValue, soundboardSample.FileTimeUpperValue);
                        }
                    }

                    handled = true;
                    break;
            }

            return IntPtr.Zero;
        }
        #endregion HwndHook

        #region InitializeAsync
        public void Initialize()
        {
            ApplicationConfiguration.ImportUserSettings();
            AudioAgent.BeginCapturing();

            LoadAudioSamples();
            LoadConfig();
        }
        #endregion InitializeAsync

        #region LoadAudioDevices
        private void LoadAudioDevices()
        {
            var ActiveAudioDevices = AudioAgent.GetWindowsAudioDevices().ToList();

            // Capture Devices
            if (ApplicationConfiguration.DefaultCaptureDeviceIds.Any())
            {
                var activeCaptureDevices = from activeDevice in ActiveAudioDevices
                                           join audioDeviceId in ApplicationConfiguration.DefaultCaptureDeviceIds
                                              on activeDevice.DeviceId equals audioDeviceId
                                           select activeDevice;

                SelectedCaptureDevicesCollection = new ObservableCollection<AudioDevice>(activeCaptureDevices);
            }

            // Output Devices
            if (ApplicationConfiguration.DefaultOutputDeviceIds.Any())
            {
                var activeOutputDevices = from activeDevice in ActiveAudioDevices
                                          join audioDeviceId in ApplicationConfiguration.DefaultOutputDeviceIds
                                             on activeDevice.DeviceId equals audioDeviceId
                                          select activeDevice;

                activeOutputDevices.ToList().ForEach(activeDevice =>
                {
                    try
                    {
                        activeDevice.PlaybackType = ApplicationConfiguration.OutputDevicePlaybackTypeCollection[activeDevice.DeviceId];
                    }
                    catch { }
                });

                SelectedOutputDevicesCollection = new ObservableCollection<AudioDevice>(activeOutputDevices);
            }
        }
        #endregion LoadAudioDevices

        #region LoadAudioSamples
        public void LoadAudioSamples()
        {
            if (Directory.Exists(ApplicationConfiguration.SoundboardSampleDirectory))
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

                    _soundboardSampleCollection.Add(new SoundboardSample(audioSamplePath)
                    {
                        GroupName = groupName,
                        FileTimeMax = totalSeconds,
                        FileTimeMin = 0,
                        FileTimeUpperValue = totalSeconds,
                        FileTimeLowerValue = 0,
                        HotkeyId = _soundboardSampleCollection.Count
                    });
                }
            }
        }
        #endregion LoadAudioSamples

        #region LoadConfig
        public void LoadConfig()
        {
            LoadAudioDevices();
            RegisterHotKeysAndHooks();
        }
        #endregion LoadConfig

        #region PlayAudioSample
        public void PlayAudioSample(SoundboardSample soundboardSample, PlaybackType playbackType)
        {
            if (SelectedOutputDevicesCollection.Any())
            {
                // Move the playback cursor
                int playbackTimerInterval = 200;
                var playbackTimer = new System.Timers.Timer(playbackTimerInterval);
                playbackTimer.Elapsed += (sender, e) =>
                {
                    soundboardSample.PlaybackCursorValue = soundboardSample.PlaybackCursorValue + (playbackTimerInterval / 1000.0);
                };

                SelectedOutputDevicesCollection[0].DirectSoundOutInstance.PlaybackStopped += (sender, e) =>
                {
                    playbackTimer.Stop();

                    // Reset the playback cursor position
                    soundboardSample.PlaybackCursorValue = soundboardSample.FileTimeLowerValue;
                };

                playbackTimer.Start();
                AudioAgent.BeginAudioPlayback(soundboardSample.FilePath, SelectedOutputDevicesCollection.ToList(), playbackType, soundboardSample.FileTimeLowerValue, soundboardSample.FileTimeUpperValue);
            }
        }
        #endregion PlayAudioSample

        #region RegisterHotKeysAndHooks
        public void RegisterHotKeysAndHooks()
        {
            if (RegisterRecordHotKey(ApplicationConfiguration.RecordHotkey))
            {
                RecordHotkey = ApplicationConfiguration.RecordHotkey;
            }

            SoundboardSampleCollection.ToList().ForEach(soundboardSample =>
            {
                if (soundboardSample.Hotkey != Key.None)
                {
                    RegisterSoundboardSampleHotKey(soundboardSample.Hotkey, soundboardSample.HotkeyId);
                }
            });

            // Hooks
            _hwndSource = HwndSource.FromHwnd(WindowHandle);
            _hwndSource.AddHook(HwndHook);
        }
        #endregion RegisterHotKeysAndHooks

        #region RegisterSoundboardSampleHotKey
        public void RegisterSoundboardSampleHotKey(Key key, int keyId)
        {
            WindowsApi.RegisterHotKey(WindowHandle, key, keyId, ApplicationConfiguration.GlobalKeyModifer);
        }
        #endregion RegisterSoundboardSampleHotKey

        #region RegisterRecordHotKey
        public bool RegisterRecordHotKey(Key key)
        {
            bool registerResult = true;

            WindowsApi.UnregisterHotkey(WindowHandle, _recordHotkeyId);
            WindowsApi.RegisterHotKey(WindowHandle, key, _recordHotkeyId, ApplicationConfiguration.GlobalKeyModifer);

            return registerResult;
        }
        #endregion RegisterRecordHotKey

        #region SaveSample
        public void SaveSample(SoundboardSample soundboardSample)
        {
            SelectedOutputDevicesCollection.Where(x => x.DirectSoundOutInstance?.PlaybackState == PlaybackState.Playing).ToList().ForEach(x => x.DirectSoundOutInstance.Stop());

            // File length
            if (soundboardSample.FileTimeUpperValue != soundboardSample.FileTimeMax || soundboardSample.FileTimeLowerValue != soundboardSample.FileTimeMin)
            {
                AudioAgent.TrimFile(soundboardSample.FilePath, soundboardSample.FileTimeLowerValue, soundboardSample.FileTimeUpperValue);

                soundboardSample.FileTimeMin = 0;
                soundboardSample.FileTimeMax = AudioAgent.GetFileAudioDuration(soundboardSample.FilePath).Seconds;
                soundboardSample.FileTimeLowerValue = 0;
                soundboardSample.FileTimeUpperValue = soundboardSample.FileTimeMax;
            }

            // File name
            if (Path.GetFileNameWithoutExtension(soundboardSample.FilePath) != soundboardSample.Name)
            {
                string newFilePath = Path.Combine(ApplicationConfiguration.SoundboardSampleDirectory, soundboardSample.GroupName, $"{soundboardSample.Name}.wav");
                File.Move(soundboardSample.FilePath, newFilePath);

                soundboardSample.FilePath = newFilePath;
            }

            soundboardSample.SaveMetadataProperties();
        }
        #endregion SaveSample

        #region SetAudioDevices
        public void SetAudioDevices(AudioDeviceType audioDeviceType)
        {
            using (AudioDeviceDialog audioDeviceDialog = new AudioDeviceDialog(audioDeviceType))
            {
                if (audioDeviceDialog.ShowDialog().Value)
                    switch (audioDeviceType)
                    {
                        case AudioDeviceType.Capture:
                            SelectedCaptureDevicesCollection = new ObservableCollection<AudioDevice>(audioDeviceDialog.SelectedAudioDevices);
                            break;
                        case AudioDeviceType.Render:
                            SelectedOutputDevicesCollection = new ObservableCollection<AudioDevice>(audioDeviceDialog.SelectedAudioDevices);
                            break;
                    }
            }
        }
        #endregion SetAudioDevices

        #region SetEvents
        private void SetEvents()
        {
            AudioAgent.FileWritten += OnFileWritten;

            // Audio Meter Polling Rate
            var audioMeterUpdateTimer = new System.Timers.Timer(100);
            audioMeterUpdateTimer.Start();

            audioMeterUpdateTimer.Elapsed += OnAudioMeterTimerElapsed;
        }
        #endregion SetEvents

        #region StopAudioPlayback
        public void StopAudioPlayback()
        {
            AudioAgent.StopAudioPlayback(SelectedOutputDevicesCollection.ToList());
        }
        #endregion StopAudioPlayback

        #region UnregisterRecordHotKey
        public void UnregisterRecordHotKey()
        {
            WindowsApi.UnregisterHotkey(WindowHandle, _recordHotkeyId);
        }
        #endregion UnregisterRecordHotKey

        #region UnregisterSoundboardSampleHotKey
        public void UnregisterSoundboardSampleHotKey(int keyId)
        {
            WindowsApi.UnregisterHotkey(WindowHandle, keyId);
        }
        #endregion UnregisterSoundboardSampleHotKey

        #region UnregisterHotKeysAndHooks
        public void UnregisterHotKeysAndHooks()
        {
            UnregisterRecordHotKey();
            SoundboardSampleCollection.ToList().ForEach(soundboardSample =>
            {
                UnregisterSoundboardSampleHotKey(soundboardSample.HotkeyId);
            });

            // Hooks
            _hwndSource?.RemoveHook(HwndHook);
            _hwndSource?.Dispose();
        }
        #endregion UnregisterHotKeysAndHooks
        #endregion Methods..
    }
}
