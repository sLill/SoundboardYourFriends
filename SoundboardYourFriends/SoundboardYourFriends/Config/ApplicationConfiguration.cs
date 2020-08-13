using SoundboardYourFriends.Core.Windows;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SoundboardYourFriends
{
    public static class ApplicationConfiguration
    {
        #region Member Variables..
        #endregion Member Variables..

        #region Properties..
        #region ByteSampleSize
        private static int _byteSampleSize;
        public static int ByteSampleSize
        {
            get { return _byteSampleSize; }
            set { _byteSampleSize = value; }
        }
        #endregion ByteSampleSize

        #region DefaultCaptureDeviceIds
        private static IEnumerable<Guid> _defaultCaptureDeviceIds;
        public static IEnumerable<Guid> DefaultCaptureDeviceIds
        {
            get { return _defaultCaptureDeviceIds; }
            set { _defaultCaptureDeviceIds = value; }
        }
        #endregion DefaultCaptureDeviceIds

        #region DefaultOutputDeviceIds
        private static IEnumerable<Guid> _defaultOutputDeviceIds;
        public static IEnumerable<Guid> DefaultOutputDeviceIds
        {
            get { return _defaultOutputDeviceIds; }
            set { _defaultOutputDeviceIds = value; }
        }
        #endregion DefaultOutputDeviceIds

        #region GlobalKeyModifer
        private static KeyModifier _globalKeyModifier = KeyModifier.None;
        public static KeyModifier GlobalKeyModifer
        {
            get { return _globalKeyModifier; }
            set { _globalKeyModifier = value; }
        }
        #endregion GlobalKeyModifer

        #region RecordHotkey
        private static Key _recordHotkey = Key.None;
        public static Key RecordHotkey
        {
            get { return _recordHotkey; }
            set { _recordHotkey = value; }
        }
        #endregion RecordHotkey

        #region SoundboardSampleDirectory
        private static string _soundboardSampleDirectory;
        public static string SoundboardSampleDirectory
        {
            get { return _soundboardSampleDirectory; }
            set { _soundboardSampleDirectory = value; }
        }
        #endregion SoundboardSampleDirectory
        #endregion Properties..

        #region Constructors..
        #region ApplicationConfiguration
        static ApplicationConfiguration()
        {
            // Save settings on application closing
            Application.Current.Exit += (sender, e) =>
            {
                SaveUserSettings();
            };
        }
        #endregion ApplicationConfiguration
        #endregion Constructors..

        #region Methods..
        #region GetSoundboardSampleDirectory
        private static string GetSoundboardSampleDirectory()
        {
            string soundboardSampleDirectory = string.IsNullOrEmpty(Properties.Settings.Default.SoundboardSampleDirectory)
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"SoundboardYourFriendsAudioSamples")
                : Properties.Settings.Default.SoundboardSampleDirectory;

            return soundboardSampleDirectory;
        }
        #endregion GetSoundboardSampleDirectory

        #region ImportUserSettings
        public static void ImportUserSettings()
        {
            // Audio Capture DeviceIds
            DefaultCaptureDeviceIds = new List<Guid>();
            Properties.Settings.Default.CaptureDeviceIds?.Cast<string>().ToList().ForEach(captureDeviceIdString =>
            {
                ((List<Guid>)DefaultCaptureDeviceIds).Add(Guid.Parse(captureDeviceIdString));
            });

            // Audio Output DeviceIds
            DefaultOutputDeviceIds = new List<Guid>();
            Properties.Settings.Default.OutputDeviceIds?.Cast<string>().ToList().ForEach(outputDeviceIdString =>
            {
                ((List<Guid>)DefaultOutputDeviceIds).Add(Guid.Parse(outputDeviceIdString));
            });

            // Soundboard Sample Directory
            SoundboardSampleDirectory = GetSoundboardSampleDirectory();

            // Record Hotkey
            Enum.TryParse(typeof(Key), Properties.Settings.Default.RecordHotKey, out object hotkey);
            hotkey = hotkey ?? Key.None;
            RecordHotkey = (Key)hotkey;

            // Global Key Modifier
            Enum.TryParse(typeof(KeyModifier), Properties.Settings.Default.GlobalKeyModifier, out object keyModifer);
            keyModifer = keyModifer ?? KeyModifier.None;
            GlobalKeyModifer = (KeyModifier)keyModifer;

            // Byte sample size
            ByteSampleSize = Properties.Settings.Default.ByteSampleSize;
        }
        #endregion ImportUserSettings

        #region SaveUserSettings
        public static void SaveUserSettings()
        {
            // Audio Capture DeviceIds
            Properties.Settings.Default.CaptureDeviceIds = new StringCollection();
            DefaultCaptureDeviceIds.ToList().ForEach(captureDeviceId =>
            {
                Properties.Settings.Default.CaptureDeviceIds.Add(captureDeviceId.ToString());
            });

            // Audio Output DeviceIds
            Properties.Settings.Default.OutputDeviceIds = new StringCollection();
            DefaultOutputDeviceIds.ToList().ForEach(outputDeviceId =>
            {
                Properties.Settings.Default.OutputDeviceIds.Add(outputDeviceId.ToString());
            });

            // Soundboard Sample Directory
            Properties.Settings.Default.SoundboardSampleDirectory = SoundboardSampleDirectory;

            // Record Hotkey
            Properties.Settings.Default.RecordHotKey = RecordHotkey.ToString();

            // Global modifier
            Properties.Settings.Default.GlobalKeyModifier = GlobalKeyModifer.ToString();

            // Byte sample size
            Properties.Settings.Default.ByteSampleSize = ByteSampleSize;

            Properties.Settings.Default.Save();
        }
        #endregion SaveUserSettings
        #endregion Methods..
    }
}
