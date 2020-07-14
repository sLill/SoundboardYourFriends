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

namespace SoundboardYourFriends.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        #region Member Variables..
        private AudioAgent _audioAgent;
        private HwndSource _hwndSource;
        private Key? _recordHotKey;
        private ObservableCollection<AudioDevice> _selectedListeningDevicesCollection = new ObservableCollection<AudioDevice>();
        private ObservableCollection<AudioDevice> _selectedOutputDevicesCollection = new ObservableCollection<AudioDevice>();
        private ObservableCollection<SoundboardSample> _soundboardSampleCollection = new ObservableCollection<SoundboardSample>();
        private SoundboardSample _selectedSoundboardSample;
        private string _audioSampleDirectory;
        private string _recordHotKeyDisplay = "Unassigned";

        private const int RECORD_HOTKEY_ID = 9000;
        #endregion Member Variables..

        #region Properties..
        #region SelectedListeningDevicesCollection
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

        #region RecordHotkey
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

        #region SelectedSoundboardSample
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
            GetApplicationConfiguration();
            LoadAudioSamples();

            _audioAgent = new AudioAgent();
            _audioAgent.RecordingStopped += AudioAgent_OnRecordingStopped;
        }
        #endregion MainWindowViewModel
        #endregion Constructors..

        #region Methods..
        #region Events..
        #region AudioAgent_OnRecordingStopped
        private void AudioAgent_OnRecordingStopped(object sender, EventArgs e)
        {
            string outputFilePath = (string)sender;
            string fileName = Path.GetFileNameWithoutExtension(outputFilePath);

            SoundboardSample TestRecording = new SoundboardSample()
            {
                Name = fileName,
                FilePath = outputFilePath
            };

            SoundboardSampleCollection.Add(TestRecording);
        }
        #endregion AudioAgent_OnRecordingStopped
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
                                if (_audioAgent.AudioState == AudioState.Idle)
                                {
                                    string fileName = $"AudioSample_{DateTime.Now.ToString("yyyyMMddHHmmss")}.wav";
                                    string outputFilePath = Path.Combine(_audioSampleDirectory, fileName);

                                    _audioAgent.BeginAudioRecording(outputFilePath);
                                }
                                else if (_audioAgent.AudioState == AudioState.Recording)
                                {
                                    _audioAgent.StopAudioRecording();
                                }
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
            if (_audioAgent.AudioState == AudioState.Recording)
            {
                _audioAgent.StopAudioRecording();
            }
        }
        #endregion Closing

        #region GetApplicationConfiguration
        private void GetApplicationConfiguration()
        {
            SelectedListeningDevicesCollection.Add(new AudioDevice() { FriendlyName = "No device(s) selected" });
            SelectedOutputDevicesCollection.Add(new AudioDevice() { FriendlyName = "No device(s) selected" });
        }
        #endregion GetApplicationConfiguration

        #region LoadAudioSamples
        private void LoadAudioSamples()
        {
            _audioSampleDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"SoundboardYourFriendsAudioSamples");
            foreach(string audioSamplePath in Directory.GetFiles(_audioSampleDirectory))
            {
                _soundboardSampleCollection.Add(new SoundboardSample() 
                { 
                    Name = Path.GetFileNameWithoutExtension(audioSamplePath), 
                    FilePath = audioSamplePath 
                });
            }
        }
        #endregion LoadAudioSamples

        #region PlayAudioSample
        public void PlayAudioSample(SoundboardSample soundboardSample)
        {
            _audioAgent.PlayAudio(soundboardSample.FilePath);
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

        #region SetAudioDevice
        public void SetAudioDevice(AudioDeviceType audioDeviceType)
        {
            using (AudioDeviceDialog audioDeviceDialog = new AudioDeviceDialog(audioDeviceType))
            {
                audioDeviceDialog.ShowDialog();

                ObservableCollection<AudioDevice> AudioDeviceCollection = audioDeviceDialog.SelectedAudioDevices == null ? null : new ObservableCollection<AudioDevice>(audioDeviceDialog.SelectedAudioDevices);

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
