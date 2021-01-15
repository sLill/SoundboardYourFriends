using SoundboardYourFriends.Core;
using SoundboardYourFriends.Core.Config;
using SoundboardYourFriends.Core.Extensions;
using SoundboardYourFriends.Model;
using SoundboardYourFriends.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace SoundboardYourFriends.View.Windows
{
    public partial class MainWindow : Window
    {
        #region Member Variables..
        private CollectionView _collectionView;
        List<SoundboardSample> _soundboardSamplesInFocus = new List<SoundboardSample>();
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

            LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(100, 100, 100), Color.FromRgb(80, 80, 80), new Point(0.5, 0), new Point(0.5, 1));
            Background = gradientBrush;

            _mainWindowViewModel = new MainWindowViewModel();
            DataContext = _mainWindowViewModel;
        }
        #endregion MainWindow
        #endregion Constructors..

        #region Methods..
        #region Events..
        #region ctxItemCreateDuplicate_PreviewMouseLeftButtonDown
        private void ctxItemCreateDuplicate_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _mainWindowViewModel.CreateDuplicate(lstSoundboardSamples.SelectedItems.Cast<SoundboardSample>());
            _collectionView.Refresh();
        }
        #endregion ctxItemCreateDuplicate_PreviewMouseLeftButtonDown

        #region ctxItemSendToNewGroup_PreviewMouseLeftButtonDown
        private void ctxItemSendToNewGroup_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _mainWindowViewModel.CreateNewGroup(lstSoundboardSamples.SelectedItems.Cast<SoundboardSample>());
            _collectionView.Refresh();
        } 
        #endregion ctxItemSendToNewGroup_PreviewMouseLeftButtonDown

        #region ctxItemDelete_PreviewMouseLeftButtonDown
        private async void ctxItemDelete_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show(this, "Delete Sample?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var selectedItems = new List<SoundboardSample>(lstSoundboardSamples.SelectedItems.Cast<SoundboardSample>());
                foreach (var soundboardSample in selectedItems)
                {
                    await _mainWindowViewModel.DeleteSampleAsync(soundboardSample);
                }
            };

        }
        #endregion ctxItemDelete_PreviewMouseLeftButtonDown

        #region soundboardSample_Drop
        private void soundboardSample_Drop(object sender, DragEventArgs e)
        {
            try
            {
                var sourceSoundboardSample = e.Data.GetData(typeof(SoundboardSample)) as SoundboardSample;

                if (sender is ListViewItem)
                {
                    if (e.Data.GetDataPresent(typeof(SoundboardSample)))
                    {
                        var dependencyObject = e.OriginalSource as DependencyObject;
                        var targetListViewItem = dependencyObject.FindAnchestor<ListViewItem>();
                        var targetSoundboardSample = (SoundboardSample)targetListViewItem.DataContext;

                        // Switch groups if necessary
                        sourceSoundboardSample.GroupName = targetSoundboardSample.GroupName;
                        string newFilePath = sourceSoundboardSample.GetVirtualFilePath();

                        File.Move(sourceSoundboardSample.FilePath, newFilePath);
                        sourceSoundboardSample.FilePath = newFilePath;

                        int sourceIndex = _mainWindowViewModel.SoundboardSampleCollection.IndexOf(sourceSoundboardSample);
                        int targetIndex = _mainWindowViewModel.SoundboardSampleCollection.IndexOf(targetSoundboardSample);

                        _mainWindowViewModel.SoundboardSampleCollection.Move(sourceIndex, targetIndex);
                        _mainWindowViewModel.SaveSample(sourceSoundboardSample);

                        _collectionView.Refresh();
                    }
                }

                if (sender is GroupItem groupItem)
                {
                    sourceSoundboardSample.GroupName = groupItem.GetChildrenOfType<TextBlock>().FirstOrDefault().Text;
                    string newFilePath = sourceSoundboardSample.GetVirtualFilePath();

                    File.Move(sourceSoundboardSample.FilePath, newFilePath);
                    sourceSoundboardSample.FilePath = newFilePath;

                    _collectionView.Refresh();
                }
            }
            catch { }
            finally
            {
                e.Handled = true;
            }
        }
        #endregion soundboardSample_Drop

        #region soundboardSample_PreviewMouseLeftButtonDown
        private void soundboardSample_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock draggedItem = sender as TextBlock;
            DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
        } 
        #endregion soundboardSample_PreviewMouseLeftButtonDown

        #region lstSoundboardSamples_MouseDoubleClick
        private void lstSoundboardSamples_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
        #endregion lstSoundboardSamples_MouseDoubleClick

        #region OnRegisterRecordKeyPressed
        public void OnRegisterRecordKeyPressed(object sender, KeyEventArgs e)
        {
            _mainWindowViewModel.RegisterRecordHotKey(e.Key);
            this.KeyDown -= OnRegisterRecordKeyPressed;
        }
        #endregion OnRegisterRecordKeyPressed

        #region OnRegisterSoundboardSampleKeyPressed
        public void OnRegisterSoundboardSampleKeyPressed(object sender, KeyEventArgs e)
        {
            _soundboardSamplesInFocus.ForEach(soundboardSample =>
            {
                if (e.Key == Key.Delete || e.Key == Key.Escape)
                {
                    soundboardSample.Hotkey = Key.None;
                    _mainWindowViewModel.UnregisterSoundboardSampleHotKey(soundboardSample.HotkeyId);
                }
                else 
                {
                    soundboardSample.Hotkey = e.Key;
                    _mainWindowViewModel.RegisterSoundboardSampleHotKey(e.Key, soundboardSample.HotkeyId);
                }
            });

            _soundboardSamplesInFocus.Clear();
            this.KeyDown -= OnRegisterSoundboardSampleKeyPressed;
        }
        #endregion OnRegisterSoundboardSampleKeyPressed

        #region btnDelete_Click
        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var soundboardSample = (SoundboardSample)(((Button)sender).DataContext);

            if (MessageBox.Show(this, "Delete Sample?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await _mainWindowViewModel.DeleteSampleAsync(soundboardSample);
            };
        }
        #endregion btnDelete_Click

        #region btnPlayButtonGlobal_Clicked
        private void btnPlayButtonGlobal_Click(object sender, EventArgs e)
        {
            var soundboardSample = (SoundboardSample)(((Button)sender).DataContext);
            _mainWindowViewModel.PlayAudioSample(soundboardSample, PlaybackScope.Global);
        }
        #endregion btnPlayButtonGlobal_Clicked

        #region btnPlayButtonLocal_Clicked
        private void btnPlayButtonLocal_Click(object sender, EventArgs e)
        {
            var soundboardSample = (SoundboardSample)(((Button)sender).DataContext);
            _mainWindowViewModel.PlayAudioSample(soundboardSample, PlaybackScope.Local);
        }
        #endregion btnPlayButtonLocal_Clicked

        #region btnListeningDevices_MouseUp
        private void btnListeningDevices_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _mainWindowViewModel.SetSelectedAudioDevices(AudioDeviceType.Capture);
        }
        #endregion btnListeningDevices_MouseUp

        #region btnOutputDevices_MouseUp
        private void btnOutputDevices_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _mainWindowViewModel.SetSelectedAudioDevices(AudioDeviceType.Render);
        }
        #endregion btnOutputDevices_MouseUp

        #region btnRecord_PreviewMouseButtonDown
        private void btnRecord_PreviewMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.KeyDown -= OnRegisterRecordKeyPressed;
            this.KeyDown += OnRegisterRecordKeyPressed;

            _mainWindowViewModel.RecordHotkeyDisplay = "Press any key..";
        }
        #endregion btnRecord_PreviewMouseButtonDown

        #region btnResetShortcuts_PreviewMouseLeftButtonUp
        private void btnResetShortcuts_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Clear all shortcuts", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                _mainWindowViewModel.ClearAllSoundboardSampleHotkeys();

            }
        } 
        #endregion btnResetShortcuts_PreviewMouseLeftButtonUp

        #region btnSave_Click
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var soundboardSample = (SoundboardSample)(((Button)sender).DataContext);
            if (MessageBox.Show(this, "Save Sample?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _mainWindowViewModel.SaveSample(soundboardSample);
            };
        }
        #endregion btnSave_Click

        #region btnSetting_MouseUp
        private void btnSettings_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            if (settingsWindow.ShowDialog().Value)
            {
                _mainWindowViewModel.RecordHotkeyDisplay = _mainWindowViewModel.RecordHotkey.ToString();

                _mainWindowViewModel.SoundboardSampleCollection.ToList().ForEach(x => 
                {
                    _mainWindowViewModel.RegisterSoundboardSampleHotKey(x.Hotkey, x.HotkeyId);

                    // I know this looks stupid. But it's really just to update the display
                    x.Hotkey = x.Hotkey;
                });

                _mainWindowViewModel.RegisterRecordHotKey(ApplicationConfiguration.Instance.RecordHotkey);

                // Re-intialize capture device
                if (_mainWindowViewModel.SelectedCaptureDevicesCollection.Any())
                {
                    AudioAgent.BeginAudioCapture(_mainWindowViewModel.SelectedCaptureDevicesCollection.First());
                }
            }
        }
        #endregion btnSetting_MouseUp

        #region btnStopButton_Clicked
        private void btnStopButton_Click(object sender, EventArgs e)
        {
            _mainWindowViewModel.StopAudioPlayback();
        }
        #endregion btnStopButton_Clicked

        #region txtPlaybackHotkey_PreviewMouseLeftButtonDown
        private void txtPlaybackHotkey_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var hotkeyControl = sender as TextBlock;
            var soundboardSample = (SoundboardSample)hotkeyControl.DataContext;

            _soundboardSamplesInFocus.Add(soundboardSample);

            this.KeyDown -= OnRegisterSoundboardSampleKeyPressed;
            this.KeyDown += OnRegisterSoundboardSampleKeyPressed;

            soundboardSample.HotkeyDisplay = "__";
        }
        #endregion txtPlaybackHotkey_PreviewMouseLeftButtonDown

        #region txtRecord_PreviewMouseButtonDown
        private void txtRecord_PreviewMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            //this.KeyDown -= OnRegisterRecordKeyPressed;
            //this.KeyDown += OnRegisterRecordKeyPressed;

            //_mainWindowViewModel.RecordHotkeyDisplay = "Press any key..";
        } 
        #endregion txtRecord_PreviewMouseButtonDown

        #region Window_Loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var windowInteropHelper = new WindowInteropHelper(this);

                IntPtr windowHandle = windowInteropHelper.Handle;
                _mainWindowViewModel.WindowHandle = windowHandle;

                _mainWindowViewModel.Initialize();
                this.Closing += _mainWindowViewModel.OnWindowClosing;

                InitializeControls();
            }
            catch (Exception ex)
            {
                ApplicationLogger.Log(ex.Message, ex.StackTrace);
            }

            ApplicationLogger.Log("MainWindow loaded");
        }
        #endregion Window_Loaded

        #region OnClosed
        protected override void OnClosed(EventArgs e)
        {
            _mainWindowViewModel.UnregisterHotKeysAndHooks();
            base.OnClosed(e);
        }
        #endregion OnClosed
        #endregion Events..

        #region InitializeControls
        private void InitializeControls()
        {
            try
            {
                _collectionView = (CollectionView)CollectionViewSource.GetDefaultView(lstSoundboardSamples.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("GroupName");
                _collectionView.GroupDescriptions.Add(groupDescription);
            }
            catch (Exception ex)
            {
                ApplicationLogger.Log(ex.Message, ex.StackTrace);
            }
        }
        #endregion InitializeControls
        #endregion Methods..
    }
}
