using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;

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
        private static List<Guid> _defaultCaptureDeviceIds;
        public static List<Guid> DefaultCaptureDeviceIds
        {
            get { return _defaultCaptureDeviceIds; }
            set { _defaultCaptureDeviceIds = value; }
        }
        #endregion DefaultCaptureDeviceIds

        #region DefaultOutputDeviceIds
        private static List<Guid> _defaultOutputDeviceIds;
        public static List<Guid> DefaultOutputDeviceIds
        {
            get { return _defaultOutputDeviceIds; }
            set { _defaultOutputDeviceIds = value; }
        }
        #endregion DefaultOutputDeviceIds

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
            ImportUserSettings();

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
        private static void ImportUserSettings()
        {
            // Audio Capture DeviceIds
            DefaultCaptureDeviceIds = new List<Guid>();
            Properties.Settings.Default.CaptureDeviceIds?.Cast<string>().ToList().ForEach(captureDeviceIdString => 
            {
                DefaultCaptureDeviceIds.Add(Guid.Parse(captureDeviceIdString));
            });

            // Audio Output DeviceIds
            DefaultOutputDeviceIds = new List<Guid>();
            Properties.Settings.Default.OutputDeviceIds?.Cast<string>().ToList().ForEach(outputDeviceIdString =>
            {
                DefaultOutputDeviceIds.Add(Guid.Parse(outputDeviceIdString));
            });

            // Soundboard Sample Directory
            SoundboardSampleDirectory = GetSoundboardSampleDirectory();
            ByteSampleSize = 7112000;
        }
        #endregion ImportUserSettings

        #region SaveUserSettings
        public static void SaveUserSettings()
        {
            // Audio Capture DeviceIds
            Properties.Settings.Default.CaptureDeviceIds = new StringCollection();
            DefaultCaptureDeviceIds.ForEach(captureDeviceId =>
            {
                Properties.Settings.Default.CaptureDeviceIds.Add(captureDeviceId.ToString());
            });

            // Audio Output DeviceIds
            Properties.Settings.Default.OutputDeviceIds = new StringCollection();
            DefaultOutputDeviceIds.ForEach(outputDeviceId => 
            {
                Properties.Settings.Default.OutputDeviceIds.Add(outputDeviceId.ToString());
            });

            Properties.Settings.Default.SoundboardSampleDirectory = SoundboardSampleDirectory;
            Properties.Settings.Default.Save();
        }
        #endregion SaveUserSettings
        #endregion Methods..
    }
}
