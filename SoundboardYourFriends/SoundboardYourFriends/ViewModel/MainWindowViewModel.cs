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

namespace SoundboardYourFriends.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        #region Member Variables..
        private Key? _recordHotKey;
        private string _recordHotKeyDisplay = "N/A";
        private HwndSource _hwndSource;

        private const int RECORD_HOTKEY_ID = 9000;
        #endregion Member Variables..

        #region Properties..
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
        #endregion Properties..

        #region Constructors..
        #region MainWindowViewModel
        public MainWindowViewModel()
        {

        }
        #endregion MainWindowViewModel
        #endregion Constructors..

        #region Methods..
        #region Events..
        #endregion Events..

        #region Windows..
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // CTRL
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:

                    RecordHotkeyDisplay = "Detected";
                    switch (wParam.ToInt32())
                    {
                        case RECORD_HOTKEY_ID:
                            int vkey = (((int)lParam >> 16) & 0xFFFF);
                            if (vkey == KeyInterop.VirtualKeyFromKey(Key.F7))
                            {
                                _recordHotKeyDisplay = "Detected";
                            }
                            handled = true;
                            break;
                    }
                    break;
            }

            return IntPtr.Zero;
        }
        #endregion Windows..

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
            const int MOD_NONE = 0; // (none)
            //const int MOD_ALT = 1; // ALT
            //const int MOD_CONTROL = 2; // CTRL
            //const int MOD_SHIFT = 4; // SHIFT
            //const int MOD_WIN = 8; // WINDOWS

            if (!RegisterHotKey(viewHandle, RECORD_HOTKEY_ID, MOD_NONE, (int)key))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
        #endregion SetRecordHotKey
        #endregion Methods..
    }
}
