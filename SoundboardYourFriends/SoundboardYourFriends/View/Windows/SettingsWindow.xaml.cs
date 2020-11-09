using SoundboardYourFriends.ViewModel;
using System.Windows;

namespace SoundboardYourFriends.View.Windows
{
    public partial class SettingsWindow : Window
    {
        #region Member Variables..
        SettingsWindowViewModel _settingsWindowViewModel;
        #endregion Member Variables..

        #region Properties..
        #endregion Properties..

        #region Constructors..
        #region SettingsWindow
        public SettingsWindow()
        {
            InitializeComponent();

            _settingsWindowViewModel = new SettingsWindowViewModel();
            DataContext = _settingsWindowViewModel;
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
            _settingsWindowViewModel.Save();
            this.DialogResult = true;

            this.Close();
        }
        #endregion btnSave_Click
        #endregion Event Handlers..
        #endregion Methods..    
    }
}
