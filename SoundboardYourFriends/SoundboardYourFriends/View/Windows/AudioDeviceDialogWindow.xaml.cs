using SoundboardYourFriends.Model;
using SoundboardYourFriends.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SoundboardYourFriends.View.Windows
{
    public partial class AudioDeviceDialog : Window, IDisposable
    {
        #region Member Variables..
        private AudioDeviceDialogViewModel _audioDeviceDialogViewModel;
        #endregion Member Variables..

        #region Properties..
        #region SelectedAudioDevices
        public IEnumerable<AudioDevice> SelectedAudioDevices { get { return lstAudioDevices.SelectedItems.Cast<AudioDevice>(); } }
        #endregion SelectedAudioDevices
        #endregion Properties..

        #region Constructors..
        public AudioDeviceDialog(AudioDeviceType audioDeviceType)
        {
            InitializeComponent();

            _audioDeviceDialogViewModel = new AudioDeviceDialogViewModel(audioDeviceType);
            DataContext = _audioDeviceDialogViewModel;
        }
        #endregion Constructors..

        #region Methods..
        #region Events..
        #region Button_PreviewMouseLeftButtonDown
        private void btnOK_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
        #endregion Button_PreviewMouseLeftButtonDown
        #endregion Events..

        #region Dispose
        public void Dispose()
        {

        }
        #endregion Dispose
        #endregion Methods..
    }
}
