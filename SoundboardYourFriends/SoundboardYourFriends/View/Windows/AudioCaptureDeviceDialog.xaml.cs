using SoundboardYourFriends.Model;
using SoundboardYourFriends.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SoundboardYourFriends.View.Windows
{
    public partial class AudioCaptureDeviceDialog : Window, IDisposable
    {
        #region Member Variables..
        private AudioCaptureDeviceDialogViewModel _AudioCaptureDeviceDialogViewModel;
        #endregion Member Variables..

        #region Properties..
        #region SelectedAudioCaptureDevices
        public IEnumerable<AudioCaptureDevice> SelectedAudioCaptureDevices => lstAudioDevices.Items.Cast<AudioCaptureDevice>().Where(x => x.DeviceActive);
        #endregion SelectedAudioCaptureDevices
        #endregion Properties..

        #region Constructors..
        #region AudioCaptureDeviceDialog
        public AudioCaptureDeviceDialog(IEnumerable<AudioCaptureDevice> audioCaptureDevices)
        {
            InitializeComponent();

            _AudioCaptureDeviceDialogViewModel = new AudioCaptureDeviceDialogViewModel(audioCaptureDevices);
            DataContext = _AudioCaptureDeviceDialogViewModel;
        }
        #endregion AudioCaptureDeviceDialog
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
        { }
        #endregion Dispose
        #endregion Methods..
    }
}
