using System;
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
            SoundboardSampleDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"SoundboardYourFriendsAudioSamples");
            ByteSampleSize = 7112000;
        }
        #endregion ImportSettings
        #endregion Methods..
    }
}
