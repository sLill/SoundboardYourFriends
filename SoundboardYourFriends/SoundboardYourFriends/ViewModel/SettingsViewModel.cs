using SoundboardYourFriends.Core;
using SoundboardYourFriends.Core.Windows;

namespace SoundboardYourFriends.ViewModel
{
    public class SettingsViewModel : ObservableObject
    {
        #region Member Variables..
        #endregion Member Variables..

        #region Properties..
        #region DeviceAverageBytesPerSecond
        private int _deviceAverageBytesPerSecond = AudioAgent.WasapiLoopbackCapture.WaveFormat.AverageBytesPerSecond;
        public int DeviceAverageBytesPerSecond
        {
            get { return _deviceAverageBytesPerSecond; }
        }
        #endregion DeviceAverageBytesPerSecond

        #region GlobalKeyModifier
        private KeyModifier _globalKeyModifier;
        public KeyModifier GlobalKeyModifier
        {
            get { return _globalKeyModifier; }
            set
            {
                _globalKeyModifier = value;
                RaisePropertyChanged();
            }
        }
        #endregion GlobalKeyModifier

        #region SoundboardSampleDirectory
        private string _soundboardSampleDirectory;
        public string SoundboardSampleDirectory
        {
            get { return _soundboardSampleDirectory; }
            set
            {
                _soundboardSampleDirectory = value;
                RaisePropertyChanged();
            }
        }
        #endregion SoundboardSampleDirectory

        #region ByteSampleSize
        private int _byteSampleSize;
        public int ByteSampleSize
        {
            get { return _byteSampleSize; }
            set
            {
                _byteSampleSize = value;
                RaisePropertyChanged();
            }
        }
        #endregion ByteSampleSize
        #endregion Properties..

        #region Constructors..
        #region SettingsViewModel
        public SettingsViewModel()
        {
            LoadApplicationSettings();
        }
        #endregion SettingsViewModel
        #endregion Constructors..

        #region Methods..
        #region LoadApplicationSettings
        private void LoadApplicationSettings()
        {
            GlobalKeyModifier = ApplicationConfiguration.GlobalKeyModifer;
            SoundboardSampleDirectory = ApplicationConfiguration.SoundboardSampleDirectory;
            ByteSampleSize = ApplicationConfiguration.ByteSampleSize;
        }
        #endregion LoadApplicationSettings

        #region Save
        public void Save()
        {
            ApplicationConfiguration.GlobalKeyModifer = GlobalKeyModifier;
            ApplicationConfiguration.SoundboardSampleDirectory = SoundboardSampleDirectory;
            ApplicationConfiguration.ByteSampleSize = ByteSampleSize;
        }
        #endregion Save
        #endregion Methods..
    }
}
