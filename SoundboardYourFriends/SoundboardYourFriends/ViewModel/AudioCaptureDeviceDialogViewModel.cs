using SoundboardYourFriends.Core;
using SoundboardYourFriends.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;

namespace SoundboardYourFriends.ViewModel
{
    public class AudioCaptureDeviceDialogViewModel : ObservableObject
    {
        #region Member Variables..
        private ObservableCollection<AudioCaptureDevice> _audioCaptureDevices;
        #endregion Member Variables..

        #region Properties..
        #region AudioCaptureDevices
        public ObservableCollection<AudioCaptureDevice> AudioCaptureDevices
        {
            get { return _audioCaptureDevices; }
            private set
            {
                _audioCaptureDevices = value;
                RaisePropertyChanged();
            }
        }
        #endregion AudioCaptureDevices
        #endregion Properties..

        #region Constructors..
        #region AudioCaptureDeviceDialogViewModel
        public AudioCaptureDeviceDialogViewModel(IEnumerable<AudioCaptureDevice> audioCaptureDevices)
        {
            AudioCaptureDevices = new ObservableCollection<AudioCaptureDevice>(audioCaptureDevices);

            var allWindowsAudioDevices = new ObservableCollection<AudioCaptureDevice>(AudioAgent.GetWindowsAudioDevices()
                .Select(device => new AudioCaptureDevice(device)));

            allWindowsAudioDevices.Where(device => !AudioCaptureDevices.Any(x => x.DeviceId == device.DeviceId)).ToList().ForEach(device => AudioCaptureDevices.Add(device));
        }
        #endregion AudioCaptureDeviceDialogViewModel
        #endregion Constructors..

        #region Methods..
        #endregion Methods..
    }
}
