using System;
using System.Collections.Generic;
using System.IO;

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
            ImportSettings();
        }
        #endregion ApplicationConfiguration
        #endregion Constructors..

        #region Methods..
        #region ImportSettings
        private static void ImportSettings()
        {
            DefaultCaptureDeviceIds = new List<Guid>();
            DefaultOutputDeviceIds = new List<Guid>();
            SoundboardSampleDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"SoundboardYourFriendsAudioSamples");
            ByteSampleSize = 7112000;
        }
        #endregion ImportSettings
        #endregion Methods..
    }
}
