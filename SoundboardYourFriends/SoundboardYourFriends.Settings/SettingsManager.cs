using System;
using System.IO;

namespace SoundboardYourFriends.Settings
{
    public static class SettingsManager
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
        static SettingsManager()
        {
            // Load settings
            SoundboardSampleDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"SoundboardYourFriendsAudioSamples");
            ByteSampleSize = 7112000;
        }
        #endregion Constructors..

        #region Methods..
        #endregion Methods..
    }
}
