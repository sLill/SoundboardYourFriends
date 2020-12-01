using SoundboardYourFriends.Core;
using SoundboardYourFriends.Core.Config;
using SoundboardYourFriends.Core.Windows;

namespace SoundboardYourFriends.ViewModel
{
    public class SettingsWindowViewModel : ObservableObject
    {
        #region Member Variables..
        #endregion Member Variables..

        #region Properties..
        #region RecordKeyModifier
        private KeyModifier _recordKeyModifier;
        public KeyModifier RecordKeyModifier
        {
            get { return _recordKeyModifier; }
            set
            {
                _recordKeyModifier = value;
                RaisePropertyChanged();
            }
        }
        #endregion RecordKeyModifier

        #region SampleKeyModifier
        private KeyModifier _sampleKeyModifier;
        public KeyModifier SampleKeyModifier
        {
            get { return _sampleKeyModifier; }
            set
            {
                _sampleKeyModifier = value;
                RaisePropertyChanged();
            }
        }
        #endregion SampleKeyModifier

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

        #region SoundboardSampleSeconds
        private int _soundboardSampleSeconds;
        public int SoundboardSampleSeconds
        {
            get { return _soundboardSampleSeconds; }
            set
            {
                _soundboardSampleSeconds = value;
                RaisePropertyChanged();
            }
        }
        #endregion SoundboardSampleSeconds
        #endregion Properties..

        #region Constructors..
        #region SettingsWindowViewModel
        public SettingsWindowViewModel()
        {
            LoadApplicationSettings();
        }
        #endregion SettingsWindowViewModel
        #endregion Constructors..

        #region Methods..
        #region LoadApplicationSettings
        private void LoadApplicationSettings()
        {
            RecordKeyModifier = ApplicationConfiguration.Instance.RecordKeyModifer;
            SampleKeyModifier = ApplicationConfiguration.Instance.SampleKeyModifier;
            SoundboardSampleDirectory = ApplicationConfiguration.Instance.SoundboardSampleDirectory;
            SoundboardSampleSeconds = ApplicationConfiguration.Instance.SoundboardSampleSeconds;
        }
        #endregion LoadApplicationSettings

        #region Save
        public void Save()
        {
            ApplicationConfiguration.Instance.RecordKeyModifer = RecordKeyModifier;
            ApplicationConfiguration.Instance.SampleKeyModifier = SampleKeyModifier;
            ApplicationConfiguration.Instance.SoundboardSampleDirectory = SoundboardSampleDirectory;
            ApplicationConfiguration.Instance.SoundboardSampleSeconds = SoundboardSampleSeconds;
        }
        #endregion Save
        #endregion Methods..
    }
}
