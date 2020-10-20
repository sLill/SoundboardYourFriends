using SoundboardYourFriends.Core;
using SoundboardYourFriends.Model;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;

namespace SoundboardYourFriends.ViewModel
{
    public class AudioOutputDeviceDialogViewModel : ObservableObject
    {
        #region Member Variables..
        private ObservableCollection<AudioOutputDevice> _audioOutputDevices;
        #endregion Member Variables..

        #region Properties..
        #region AudioOutputDevices
        public ObservableCollection<AudioOutputDevice> AudioOutputDevices
        {
            get { return _audioOutputDevices; }
            private set
            {
                _audioOutputDevices = value;
                RaisePropertyChanged();
            }
        }
        #endregion AudioOutputDevices
        #endregion Properties..

        #region Constructors..
        #region AudioDeviceDialogViewModel
        public AudioOutputDeviceDialogViewModel(IEnumerable<AudioOutputDevice> audioOutputDevices)
        {
            AudioOutputDevices = new ObservableCollection<AudioOutputDevice>(audioOutputDevices);
            
            var allWindowsAudioDevices = new ObservableCollection<AudioOutputDevice>(AudioAgent.GetWindowsAudioDevices()
                .Select(device => new AudioOutputDevice(device)));
           
            allWindowsAudioDevices.Where(device => !AudioOutputDevices.Any(x => x.DeviceId == device.DeviceId)).ToList().ForEach(device => AudioOutputDevices.Add(device));
        }
        #endregion AudioDeviceDialogViewModel
        #endregion Constructors..

        #region Methods..
        #endregion Methods..
    }
}
