using SoundboardYourFriends.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoundboardYourFriends.ViewModel
{
    public class SettingsViewModel : ObservableObject
    {
        #region Member Variables..
        #endregion Member Variables..

        #region Properties..
        #region SoundboardSampleDirectory
        private string _soundboardSampleDirectory;
        public string SoundboardSampleDirectory
        {
            get { return _soundboardSampleDirectory; }
            set { _soundboardSampleDirectory = value; }
        }
        #endregion SoundboardSampleDirectory

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
            SoundboardSampleDirectory = ApplicationConfiguration.SoundboardSampleDirectory;
        }
        #endregion LoadApplicationSettings

        #region Save
        public void Save()
        {
            ApplicationConfiguration.SoundboardSampleDirectory = SoundboardSampleDirectory;
        }
        #endregion Save
        #endregion Methods..
    }
}
