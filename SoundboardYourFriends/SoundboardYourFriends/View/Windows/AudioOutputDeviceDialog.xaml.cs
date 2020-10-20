using SoundboardYourFriends.Model;
using SoundboardYourFriends.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SoundboardYourFriends.View.Windows
{
    public partial class AudioOutputDeviceDialog : Window, IDisposable
    {
        #region Member Variables..
        private AudioOutputDeviceDialogViewModel _audioOutputDeviceDialogViewModel;
        #endregion Member Variables..

        #region Properties..
        #region SelectedAudioOutputDevices
        public IEnumerable<AudioOutputDevice> SelectedAudioOutputDevices => lstAudioDevices.Items.Cast<AudioOutputDevice>().Where(x => x.DeviceActive);
        #endregion SelectedAudioOutputDevices
        #endregion Properties..

        #region Constructors..
        #region AudioOutputDeviceDialog
        public AudioOutputDeviceDialog(IEnumerable<AudioOutputDevice> audioOutputDevices)
        {
            InitializeComponent();

            _audioOutputDeviceDialogViewModel = new AudioOutputDeviceDialogViewModel(audioOutputDevices);
            DataContext = _audioOutputDeviceDialogViewModel;
        }
        #endregion AudioOutputDeviceDialog
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
