using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace SoundboardYourFriends.Core.Windows
{
    public static class WindowsApi
    {
        #region Member Variables..
        #endregion Member Variables..

        #region Methods..
        #region Windows..
        #region RegisterHotKey
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vlc);
        #endregion RegisterHotKey

        #region UnregisterHotKey
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        #endregion UnregisterHotKey
        #endregion Windows..

        #region RegisterHotKey
        public static void RegisterHotKey(IntPtr viewHandle, Key key, int keyId, KeyModifier modifier)
        {
            if (key != Key.None)
            {
                uint keyCode = Convert.ToUInt32(KeyInterop.VirtualKeyFromKey(key).ToString("X"), 16);
                RegisterHotKey(viewHandle, keyId, (uint)modifier, keyCode);
            }
        }
        #endregion RegisterHotKey

        #region UnregisterHotkey
        public static void UnregisterHotkey(IntPtr viewHandle, int keyId)
        {
            UnregisterHotKey(viewHandle, keyId);
        }
        #endregion UnregisterHotkey
        #endregion Methods..
    }
}
