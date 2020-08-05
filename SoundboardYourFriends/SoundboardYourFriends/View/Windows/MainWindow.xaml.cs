﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using SoundboardYourFriends.Model;
using SoundboardYourFriends.View.UserControls;
using SoundboardYourFriends.ViewModel;

namespace SoundboardYourFriends.View.Windows
{
    public partial class MainWindow : Window
    {
        #region Member Variables..
        #endregion Member Variables..

        #region Properties..

        #region MainWindowViewModel
        private MainWindowViewModel _mainWindowViewModel;
        public MainWindowViewModel MainWindowViewModel
        {
            get { return _mainWindowViewModel; }
            set { _mainWindowViewModel = value; }
        }
        #endregion MainWindowViewModel

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

        }
        #endregion lstSoundboardSamples_MouseDoubleClick

        #region OnKeyPressed
        public void OnKeyPressed(object sender, KeyEventArgs e)
        {
            _mainWindowViewModel.UnregisterRecordHotkey(new WindowInteropHelper(this).Handle);
            _mainWindowViewModel.RegisterRecordHotKey(new WindowInteropHelper(this).Handle, e.Key);
            this.KeyDown -= OnKeyPressed;
        }
        #endregion OnKeyPressed

        #region btnDelete_Click
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var soundboardSample = (SoundboardSample)(((Button)sender).DataContext);

            if (MessageBox.Show(this, "Confirm Delete", string.Empty, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _mainWindowViewModel.DeleteSample(soundboardSample);
            };
        }
        #endregion btnDelete_Click

        #region btnPlayButtonGlobal_Clicked
        private void btnPlayButtonGlobal_Click(object sender, EventArgs e)
        {
            var soundboardSample = (SoundboardSample)(((Button)sender).DataContext);
            _mainWindowViewModel.PlayAudioSample(soundboardSample, PlaybackType.Global);
        }
        #endregion btnPlayButtonGlobal_Clicked

        #region btnPlayButtonLocal_Clicked
        private void btnPlayButtonLocal_Click(object sender, EventArgs e)
        {
            var soundboardSample = (SoundboardSample)(((Button)sender).DataContext);
            _mainWindowViewModel.PlayAudioSample(soundboardSample, PlaybackType.Local);
        }
        #endregion btnPlayButtonLocal_Clicked

        #region btnListeningDevices_MouseUp
        private void btnListeningDevices_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _mainWindowViewModel.SetAudioDevices(AudioDeviceType.Capture);
        }
        #endregion btnListeningDevices_MouseUp

        #region btnOutputDevices_MouseUp
        private void btnOutputDevices_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _mainWindowViewModel.SetAudioDevices(AudioDeviceType.Render);
        }
        #endregion btnOutputDevices_MouseUp

        #region btnRecord_PreviewMouseButtonDown
        private void btnRecord_PreviewMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.KeyDown += OnKeyPressed;
            _mainWindowViewModel.RecordHotkeyDisplay = "Press any key..";
        }
        #endregion btnRecord_PreviewMouseButtonDown

        #region btnSave_Click
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var soundboardSample = (SoundboardSample)(((Button)sender).DataContext);
            if (MessageBox.Show(this, "Confirm Save", string.Empty, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _mainWindowViewModel.SaveSample(soundboardSample);
            };
        }
        #endregion btnSave_Click

        #region btnSetting_MouseUp
        private void btnSettings_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }
        #endregion btnSetting_MouseUp

        #region btnStopButton_Clicked
        private void btnStopButton_Click(object sender, EventArgs e)
        {
            _mainWindowViewModel.StopAudioPlayback();
        }
        #endregion btnStopButton_Clicked

        #region Window_Closing
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _mainWindowViewModel.Closing();
        }
        #endregion Window_Closing

        #region Window_Loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CollectionView collectionView = (CollectionView)CollectionViewSource.GetDefaultView(lstSoundboardSamples.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("GroupName");
            collectionView.GroupDescriptions.Add(groupDescription);

            _mainWindowViewModel.LoadConfig(this);
            _mainWindowViewModel.LoadAudioSamples();
        }
        #endregion Window_Loaded

        #region OnClosed
        protected override void OnClosed(EventArgs e)
        {
            _mainWindowViewModel.UnregisterRecordHotkey(new WindowInteropHelper(this).Handle);
            base.OnClosed(e);
        }
        #endregion OnClosed
        #endregion Events..
        #endregion Methods..
    }
}
