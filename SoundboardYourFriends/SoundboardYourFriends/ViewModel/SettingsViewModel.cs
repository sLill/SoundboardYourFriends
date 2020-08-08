using SoundboardYourFriends.Core;
using SoundboardYourFriends.Core.Windows;
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
        }
        #endregion LoadApplicationSettings

        #region Save
        public void Save()
        {
            ApplicationConfiguration.GlobalKeyModifer = GlobalKeyModifier;
            ApplicationConfiguration.SoundboardSampleDirectory = SoundboardSampleDirectory;
        }
        #endregion Save
        #endregion Methods..
    }
}
