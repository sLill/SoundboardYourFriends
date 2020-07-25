using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using SoundboardYourFriends.Model;
using SoundboardYourFriends.ViewModel;
using WPF.JoshSmith.ServiceProviders.UI;

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
        #region lstSoundboardSamples_MouseDoubleClick
        private void lstSoundboardSamples_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _mainWindowViewModel.PlayAudioSample((SoundboardSample)lstSoundboardSamples.SelectedItem);
        }
        #endregion lstSoundboardSamples_MouseDoubleClick

        #region OnKeyPressed
        public void OnKeyPressed(object sender, KeyEventArgs e)
        {
            _mainWindowViewModel.RegisterRecordHotKey(new WindowInteropHelper(this).Handle, e.Key);
            this.KeyDown -= OnKeyPressed;
        }
        #endregion OnKeyPressed

        #region btnListeningDevices_MouseUp
        private void btnListeningDevices_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _mainWindowViewModel.SetAudioDevice(AudioDeviceType.Input);
        }
        #endregion btnListeningDevices_MouseUp

        #region btnOutputDevices_MouseUp
        private void btnOutputDevices_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _mainWindowViewModel.SetAudioDevice(AudioDeviceType.Output);
        }
        #endregion btnOutputDevices_MouseUp

        #region btnRecordButton_MouseUp
        private void btnRecordButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.KeyDown += OnKeyPressed;
            _mainWindowViewModel.RecordHotkeyDisplay = "Press any key..";
        }
        #endregion btnRecordButton_MouseUp

        #region btnSetting_MouseUp
        private void btnSettings_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }
        #endregion btnSetting_MouseUp

        #region Window_Closing
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _mainWindowViewModel.Closing();
        }
        #endregion Window_Closing

        #region Window_Loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            new ListViewDragDropManager<SoundboardSample>(this.lstSoundboardSamples);

            CollectionView collectionView = (CollectionView)CollectionViewSource.GetDefaultView(lstSoundboardSamples.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("GroupName");
            collectionView.GroupDescriptions.Add(groupDescription);
        }
        #endregion Window_Loaded
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
