using Microsoft.Win32;
using SoundboardYourFriends.View.UserControls;
using SoundboardYourFriends.ViewModel;
using System.Windows;

namespace SoundboardYourFriends.View.Windows
{
    public partial class SettingsWindow : Window
    {
        #region Member Variables..
        SettingsViewModel _settingsViewModel;
        #endregion Member Variables..

        #region Properties..
        #endregion Properties..

        #region Constructors..
        #region SettingsWindow
        public SettingsWindow() 
        {
            InitializeComponent();

            _settingsViewModel = new SettingsViewModel();
            DataContext = _settingsViewModel;
        }
        #endregion SettingsWindow

        #endregion Constructors..

        #region Methods..
        #region Event Handlers..
        #region btnCancel_Click
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion btnCancel_Click

        #region btnSave_Click
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            _settingsViewModel.Save();
        }
        #endregion btnSave_Click
        #endregion Event Handlers..
        #endregion Methods..    
    }
}
