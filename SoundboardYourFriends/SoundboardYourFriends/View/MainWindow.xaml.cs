using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using SoundboardYourFriends.ViewModel;

namespace SoundboardYourFriends.View
{
    public partial class MainWindow : Window
    {
        #region Member Variables..
        MainWindowViewModel _mainWindowViewModel;
        #endregion Member Variables..

        #region Properties..
        #endregion Properties..

        #region Constructors..
        #region MainWindow
        public MainWindow() 
        {
            InitializeComponent();

            _mainWindowViewModel = new MainWindowViewModel();
            DataContext = _mainWindowViewModel;
        }
        #endregion MainWindow
        #endregion Constructors..

        #region Methods..
        #region Events..
        #region OnKeyPressed
        public void OnKeyPressed(object sender, KeyEventArgs e)
        {
            _mainWindowViewModel.SetRecordHotKey(new WindowInteropHelper(this).Handle, e.Key);
            this.KeyDown -= OnKeyPressed;
        }
        #endregion OnKeyPressed

        #region btnListeningDevice_MouseUp
        private void btnListeningDevice_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _mainWindowViewModel.SetAudioDevice(AudioDeviceType.Output);
        }
        #endregion btnListeningDevice_MouseUp

        #region btnRecordingDevice_MouseUp
        private void btnRecordingDevice_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _mainWindowViewModel.SetAudioDevice(AudioDeviceType.Input);
        }
        #endregion btnRecordingDevice_MouseUp

        #region btnRecordButton_MouseUp
        private void btnRecordButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.KeyDown += OnKeyPressed;
            _mainWindowViewModel.RecordHotkeyDisplay = "Press any key..";
        }
        #endregion btnRecordButton_MouseUp
        #endregion Events..

        #region OnClosed
        protected override void OnClosed(EventArgs e)
        {
            _mainWindowViewModel.UnregisterRecordHotkey(new WindowInteropHelper(this).Handle);
            base.OnClosed(e);
        }
        #endregion OnClosed
        #endregion Methods..
    }
}
