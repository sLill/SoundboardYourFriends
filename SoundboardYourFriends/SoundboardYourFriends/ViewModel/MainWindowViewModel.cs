using NAudio.Wave;
using SoundboardYourFriends.Core;
using SoundboardYourFriends.Core.Config;
using SoundboardYourFriends.Core.Windows;
using SoundboardYourFriends.Model;
using SoundboardYourFriends.View.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
            get { return $"{ApplicationConfiguration.Instance.RecordKeyModifer} + {_recordHotKeyDisplay}"; }
            set
            {
                _recordHotKeyDisplay = value;
                RaisePropertyChanged();
            }
        }
        #endregion RecordHotkeyDisplay

        #region SelectedCaptureDevicesCollection
        private ObservableCollection<AudioCaptureDevice> _selectedCaptureDevicesCollection = new ObservableCollection<AudioCaptureDevice>();
        public ObservableCollection<AudioCaptureDevice> SelectedCaptureDevicesCollection
        {
            get { return _selectedCaptureDevicesCollection; }
            set
            {
                _selectedCaptureDevicesCollection = value;
                ApplicationConfiguration.Instance.AudioCaptureDevices = value;

                BeginAudioCapture();
                RaisePropertyChanged();
            }
        }
        #endregion SelectedCaptureDevicesCollection

        #region SelectedOutputDevicesCollection
        private ObservableCollection<AudioOutputDevice> _selectedOutputDevicesCollection = new ObservableCollection<AudioOutputDevice>();
        public ObservableCollection<AudioOutputDevice> SelectedOutputDevicesCollection
        {
            get { return _selectedOutputDevicesCollection; }
            set
            {
                _selectedOutputDevicesCollection = value;
                ApplicationConfiguration.Instance.AudioOutputDevices = value;

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
        #region Event Handlers..
        #region OnAudioMeterTimerElapsed
        private void OnAudioMeterTimerElapsed(object sender, EventArgs e)
        {
            try
            {
                if (SelectedCaptureDevicesCollection.Any())
                {
                    SelectedCaptureDevicesCollection[0].AudioPeak = (int)(WasapiLoopbackCapture.GetDefaultLoopbackCaptureDevice().AudioMeterInformation.MasterPeakValue * 100);
                }

                SelectedOutputDevicesCollection.ToList().ForEach(outputDevice =>
                {
                    outputDevice.AudioPeak = (int)(outputDevice.AudioMeterInformation?.MasterPeakValue ?? 0 * 100);
                });
            }
            catch (Exception ex)
            {
                ApplicationLogger.Log(ex.Message, ex.StackTrace);
            }
        }
        #endregion OnAudioMeterTimerElapsed

        #region OnFileWritten
        public void OnFileWritten(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                ApplicationLogger.Log(ex.Message, ex.StackTrace);
            }
        }
        #endregion OnFileWritten

        #region OnAudioPlaybackStopped
        public void OnAudioPlaybackStopped(object sender, EventArgs e)
        {
            SoundboardSampleCollection.ToList().ForEach(x => x.StopPlaybackTimer());
            ((AudioOutputDevice)sender).PlaybackStopped -= OnAudioPlaybackStopped;
        }
        #endregion OnAudioPlaybackStopped

        #region OnWindowClosing
        public void OnWindowClosing(object sender, EventArgs e)
        {
            AudioAgent.StopAudioCapture();
            UnregisterHotKeysAndHooks();

            ApplicationConfiguration.Instance.RecordHotkey = RecordHotkey.Value;
            SoundboardSampleCollection.ToList().ForEach(x => x.SaveMetadataProperties());
        }
        #endregion OnWindowClosing
        #endregion Event Handlers..

        #region BeginAudioCapture
        public void BeginAudioCapture()
        {
            try
            {
                AudioAgent.StopAudioCapture();

                if (SelectedCaptureDevicesCollection.Any())
                {
                    AudioAgent.BeginAudioCapture(SelectedCaptureDevicesCollection.First());
                }
            }
            catch (Exception ex)
            {
                ApplicationLogger.Log(ex.Message, ex.StackTrace);
            }
        }
        #endregion BeginAudioCapture

        #region ClearAllSoundboardSampleHotkeys
        public void ClearAllSoundboardSampleHotkeys()
        {
            SoundboardSampleCollection.ToList().ForEach(soundboardSample => 
            {
                soundboardSample.Hotkey = Key.None;
                UnregisterSoundboardSampleHotKey(soundboardSample.HotkeyId);

                SaveSample(soundboardSample);
            });
        }
        #endregion ClearAllSoundboardSampleHotkeys

        #region CreateDuplicate
        public void CreateDuplicate(IEnumerable<SoundboardSample> soundboardSamples)
        {
            foreach (var soundboardSample in soundboardSamples)
            {
                var soundboardSampleClone = soundboardSample.GetShallowCopy();

                // Increment filename if one already exists with the same name
                int sameNameIndex = 1;
                while (File.Exists(Path.Combine(ApplicationConfiguration.Instance.SoundboardSampleDirectory, soundboardSampleClone.GroupName, $"{soundboardSampleClone.Name} Copy {sameNameIndex}.wav")))
                {
                    sameNameIndex++;
                }

                // Change the name and filepath
                soundboardSampleClone.Name = $"{soundboardSampleClone.Name} Copy {sameNameIndex}";
                soundboardSampleClone.FilePath = soundboardSampleClone.GetVirtualFilePath();

                File.Copy(soundboardSample.FilePath, soundboardSampleClone.FilePath);
                SaveSample(soundboardSampleClone);

                SoundboardSampleCollection.Add(soundboardSampleClone);
            }
        }
        #endregion CreateDuplicate

        #region CreateNewGroup
        public void CreateNewGroup(IEnumerable<SoundboardSample> soundboardSamples)
        {
            // Create new directory
            string defaultGroupName = "New Soundboard Sample Group";
            string proposedDirectory = Path.Combine(ApplicationConfiguration.Instance.SoundboardSampleDirectory, defaultGroupName);

            // Increment directory name if one already exists with the same name
            int sameNameIndex = 1;
            while (Directory.Exists(Path.Combine(ApplicationConfiguration.Instance.SoundboardSampleDirectory, $"{defaultGroupName} {sameNameIndex}")))
            {
                sameNameIndex++;
            }

            Directory.CreateDirectory(Path.Combine(ApplicationConfiguration.Instance.SoundboardSampleDirectory, $"{defaultGroupName} {sameNameIndex}"));

            // Change selected soundboard sample directories
            foreach (var soundboardSample in soundboardSamples)
            {
                string newFilePath = Path.Combine(proposedDirectory, $"{soundboardSample.Name}.wav");
                soundboardSample.GroupName = $"{defaultGroupName} {sameNameIndex}";

                File.Move(soundboardSample.FilePath, newFilePath);
                soundboardSample.FilePath = newFilePath;
            }
        }
        #endregion CreateNewGroup

        #region DeleteSampleAsync
        public async Task DeleteSampleAsync(SoundboardSample soundboardSample)
        {
            try
            {
                // Recursively try to acquire file lock. This can sometimes take a few seconds to release after 
                // an audio playback event has ended
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
                    catch
                    {
                        await Task.Delay(500);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationLogger.Log(ex.Message, ex.StackTrace);
            }
        }
        #endregion DeleteSampleAsync

        #region HwndHook
        /// <summary>
        /// Provides application shortcut functions for shortcut events from Windows
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
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
                            PlayAudioSample(soundboardSample, PlaybackScope.Global);
                        }
                    }

                    handled = true;
                    break;
            }

            return IntPtr.Zero;
        }
        #endregion HwndHook

        #region Initialize
        public void Initialize()
        {
            LoadAudioSamples();
            LoadConfig();

            BeginAudioCapture();
        }
        #endregion Initialize

        #region LoadAudioDevices
        private void LoadAudioDevices()
        {
            try
            {
                var activeWindowsAudioDevices = AudioAgent.GetWindowsAudioDevices().ToList();

                // Capture Devices
                if (ApplicationConfiguration.Instance.AudioCaptureDevices?.Any() ?? false)
                {
                    var activeCaptureDevices = from activeDevice in activeWindowsAudioDevices
                                               join audioDeviceId in ApplicationConfiguration.Instance.AudioCaptureDevices.Select(x => x.DeviceId)
                                                on activeDevice.DeviceId equals audioDeviceId
                                               select new AudioCaptureDevice(activeDevice) { DeviceActive = true };

                    SelectedCaptureDevicesCollection = new ObservableCollection<AudioCaptureDevice>(activeCaptureDevices);
                }

                // Output Devices
                if (ApplicationConfiguration.Instance.AudioOutputDevices?.Any() ?? false)
                {
                    var activeOutputDevices = from activeDevice in activeWindowsAudioDevices
                                              join audioDeviceId in ApplicationConfiguration.Instance.AudioOutputDevices.Select(x => x.DeviceId)
                                                  on activeDevice.DeviceId equals audioDeviceId
                                              select new AudioOutputDevice(activeDevice) { DeviceActive = true, PlaybackScope = ApplicationConfiguration.Instance.AudioOutputDevices.First(x => x.DeviceId == activeDevice.DeviceId).PlaybackScope };

                    SelectedOutputDevicesCollection = new ObservableCollection<AudioOutputDevice>(activeOutputDevices);
                }
            }
            catch (Exception ex)
            {
                ApplicationLogger.Log(ex.Message, ex.StackTrace);
            }
        }
        #endregion LoadAudioDevices

        #region LoadAudioSamples
        public void LoadAudioSamples()
        {
            try
            {
                if (Directory.Exists(ApplicationConfiguration.Instance.SoundboardSampleDirectory))
                {
                    foreach (string audioSamplePath in Directory.GetFiles(ApplicationConfiguration.Instance.SoundboardSampleDirectory, "*", SearchOption.AllDirectories))
                    {
                        string relativePath = Path.GetRelativePath(ApplicationConfiguration.Instance.SoundboardSampleDirectory, audioSamplePath);
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
            catch (Exception ex)
            {
                ApplicationLogger.Log(ex.Message, ex.StackTrace);
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
        public void PlayAudioSample(SoundboardSample soundboardSample, PlaybackScope playbackScope)
        {
            try
            {
                var outputDevices = SelectedOutputDevicesCollection.Where(x => x.PlaybackScope >= playbackScope).ToList();
                if (outputDevices.Any())
                {
                    outputDevices[0].PlaybackStopped += OnAudioPlaybackStopped;
                    soundboardSample.StartPlaybackTimer();
                }

                outputDevices.ForEach(outputDevice =>
                {
                    AudioAgent.BeginAudioPlayback(soundboardSample.FilePath, outputDevice, (float)soundboardSample.Volume / (float)100, soundboardSample.FileTimeLowerValue, soundboardSample.FileTimeUpperValue);
                });
            }
            catch (Exception ex)
            {
                ApplicationLogger.Log(ex.Message, ex.StackTrace);
            }
        }
        #endregion PlayAudioSample

        #region RegisterHotKeysAndHooks
        public void RegisterHotKeysAndHooks()
        {
            try
            {
                if (RegisterRecordHotKey(ApplicationConfiguration.Instance.RecordHotkey))
                {
                    RecordHotkey = ApplicationConfiguration.Instance.RecordHotkey;
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
            catch (Exception ex)
            {
                ApplicationLogger.Log(ex.Message, ex.StackTrace);
            }
        }
        #endregion RegisterHotKeysAndHooks

        #region RegisterSoundboardSampleHotKey
        public void RegisterSoundboardSampleHotKey(Key key, int keyId)
        {
            WindowsApi.RegisterHotKey(WindowHandle, key, keyId, ApplicationConfiguration.Instance.SampleKeyModifier);
        }
        #endregion RegisterSoundboardSampleHotKey

        #region RegisterRecordHotKey
        public bool RegisterRecordHotKey(Key key)
        {
            bool registerResult = true;

            WindowsApi.UnregisterHotkey(WindowHandle, _recordHotkeyId);
            if (WindowsApi.RegisterHotKey(WindowHandle, key, _recordHotkeyId, ApplicationConfiguration.Instance.RecordKeyModifer))
            {
                RecordHotkey = key;
            }

            return registerResult;
        }
        #endregion RegisterRecordHotKey

        #region SaveSample
        public void SaveSample(SoundboardSample soundboardSample)
        {
            try
            {
                // Trimming
                if (soundboardSample.FileTimeUpperValue != soundboardSample.FileTimeMax || soundboardSample.FileTimeLowerValue != soundboardSample.FileTimeMin)
                {
                    AudioAgent.TrimFile(soundboardSample.FilePath, soundboardSample.FileTimeLowerValue * 1000, soundboardSample.FileTimeUpperValue * 1000);

                    soundboardSample.FileTimeMin = 0;
                    soundboardSample.FileTimeMax = AudioAgent.GetFileAudioDuration(soundboardSample.FilePath).Seconds;
                    soundboardSample.FileTimeLowerValue = 0;
                    soundboardSample.FileTimeUpperValue = soundboardSample.FileTimeMax;
                }

                // File name
                if (Path.GetFileNameWithoutExtension(soundboardSample.FilePath) != soundboardSample.Name)
                {
                    string targetDirectory = Path.Combine(ApplicationConfiguration.Instance.SoundboardSampleDirectory, soundboardSample.GroupName == "Ungrouped" ? string.Empty : soundboardSample.GroupName);
                    string newFilePath = soundboardSample.GetVirtualFilePath();

                    Directory.CreateDirectory(targetDirectory);
                    File.Move(soundboardSample.FilePath, newFilePath);

                    soundboardSample.FilePath = newFilePath;
                }

                soundboardSample.SaveMetadataProperties();
            }
            catch (Exception ex)
            {
                ApplicationLogger.Log(ex.Message, ex.StackTrace);
            }
        }
        #endregion SaveSample

        #region SetSelectedAudioDevices
        public void SetSelectedAudioDevices(AudioDeviceType audioDeviceType)
        {
            try
            {
                switch (audioDeviceType)
                {
                    case AudioDeviceType.Capture:
                        using (AudioCaptureDeviceDialog audioCaptureDeviceDialog = new AudioCaptureDeviceDialog(SelectedCaptureDevicesCollection))
                        {
                            if (audioCaptureDeviceDialog.ShowDialog().Value)
                            {
                                SelectedCaptureDevicesCollection = new ObservableCollection<AudioCaptureDevice>(audioCaptureDeviceDialog.SelectedAudioCaptureDevices);
                            }
                        }
                        break;
                    case AudioDeviceType.Render:
                        using (AudioOutputDeviceDialog audioOutputDeviceDialog = new AudioOutputDeviceDialog(SelectedOutputDevicesCollection))
                        {
                            if (audioOutputDeviceDialog.ShowDialog().Value)
                            {
                                SelectedOutputDevicesCollection = new ObservableCollection<AudioOutputDevice>(audioOutputDeviceDialog.SelectedAudioOutputDevices);
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ApplicationLogger.Log(ex.Message, ex.StackTrace);
            }
        }
        #endregion SetSelectedAudioDevices

        #region SetEvents
        private void SetEvents()
        {
            try
            {
                AudioAgent.FileWritten += OnFileWritten;

                // Audio Meter Polling Rate
                var audioMeterUpdateTimer = new System.Timers.Timer(100);
                audioMeterUpdateTimer.Start();

                audioMeterUpdateTimer.Elapsed += OnAudioMeterTimerElapsed;
            }
            catch (Exception ex)
            {
                ApplicationLogger.Log(ex.Message, ex.StackTrace);
            }
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
