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

namespace SoundboardYourFriends.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        #region Member Variables..
        private AudioDevice _selectedListeningDevice;
        private AudioDevice _selectedRecordingDevice;
        private Key? _recordHotKey;
        private ObservableCollection<AudioDevice> _selectedListeningDevicesCollection = new ObservableCollection<AudioDevice>();
        private ObservableCollection<AudioDevice> _selectedOutputDevicesCollection = new ObservableCollection<AudioDevice>();
        private ObservableCollection<SoundboardRecording> _soundboardRecordingCollection = new ObservableCollection<SoundboardRecording>();
        private SoundboardRecording _selectedSoundboardRecording;
        private string _recordHotKeyDisplay = "N/A";
        private HwndSource _hwndSource;

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
            get { return _recordHotKeyDisplay; }
            set
            {
                _recordHotKeyDisplay = value;
                RaisePropertyChanged();
            }
        }
        #endregion RecordHotkeyDisplay

        #region SelectedSoundboardRecording
        public SoundboardRecording SelectedSoundboardRecording 
        { 
            get { return _selectedSoundboardRecording; }
            set 
            { 
                _selectedSoundboardRecording = value;
                RaisePropertyChanged();
            }
        }
        #endregion SelectedSoundboardRecording

        #region SoundboardRecordingCollection
        public ObservableCollection<SoundboardRecording> SoundboardRecordingCollection 
        { 
            get { return _soundboardRecordingCollection; }
            set 
            { 
                _soundboardRecordingCollection = value;
                RaisePropertyChanged();
            } 
        }
        #endregion SoundboardRecordingCollection
        #endregion Properties..

        #region Constructors..
        #region MainWindowViewModel
        public MainWindowViewModel()
        {
            SelectedListeningDevicesCollection.Add(new AudioDevice() { FriendlyName = "Logitech Wireless G3310" });
            SelectedOutputDevicesCollection.Add(new AudioDevice() { FriendlyName = "Realtek HD Audio Speakers" });
        }
        #endregion MainWindowViewModel
        #endregion Constructors..

        #region Methods..
        #region Events..
        #endregion Events..

        #region Windows..
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vlc);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

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
                                BeginAudioRecording();
                            }

                            handled = true;
                            break;
                    }
                    break;
            }

            return IntPtr.Zero;
        }
        #endregion Windows..

        #region BeginAudioRecording
        private void BeginAudioRecording()
        {
            SoundboardRecording TestRecording = new SoundboardRecording() { Name = "Test Recording" };
            SoundboardRecordingCollection.Add(TestRecording);
        }
        #endregion BeginAudioRecording

        #region SetAudioDevice
        public void SetAudioDevice(AudioDeviceType audioDeviceType)
        {
            using (AudioDeviceDialog audioDeviceDialog = new AudioDeviceDialog(audioDeviceType))
            {
                audioDeviceDialog.ShowDialog();
            }
        }
        #endregion SetAudioDevice

        #region SetRecordHotKey
        public void SetRecordHotKey(IntPtr viewHandle, Key key)
        {
            RecordHotkey = key;

            _hwndSource = HwndSource.FromHwnd(viewHandle);
            _hwndSource.AddHook(HwndHook);

            // Modifiers
            const uint MOD_NONE = 0x0000; // (none)
            //const uint MOD_ALT = 0x0001; // ALT
            //const uint MOD_CONTROL = 0x0002; // CTRL
            //const uint MOD_SHIFT = 0x0004; // SHIFT
            //const uint MOD_WIN = 0x0008; // WINDOWS

            uint keyCode = Convert.ToUInt32(KeyInterop.VirtualKeyFromKey(key).ToString("X"), 16);
            if (!RegisterHotKey(viewHandle, RECORD_HOTKEY_ID, MOD_NONE, keyCode))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
        #endregion SetRecordHotKey

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
