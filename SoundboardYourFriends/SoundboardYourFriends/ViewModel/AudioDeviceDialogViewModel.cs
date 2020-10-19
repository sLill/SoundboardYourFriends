using SoundboardYourFriends.Core;
using SoundboardYourFriends.Model;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;

namespace SoundboardYourFriends.ViewModel
{
    public class AudioDeviceDialogViewModel : ObservableObject
    {
        #region Member Variables..
        private ObservableCollection<AudioDeviceBase> _audioDevices;
        #endregion Member Variables..

        #region Properties..
        #region AudioDevices
        public ObservableCollection<AudioDeviceBase> AudioDevices
        {
            get { return _audioDevices; }
            private set
            {
                _audioDevices = value;
                RaisePropertyChanged();
            }
        }
        #endregion AudioDevices

        #region AudioDeviceType
        public AudioDeviceType AudioDeviceType { get; private set; }
        #endregion AudioDeviceType
        #endregion Properties..

        #region Constructors..
        #region AudioDeviceDialogViewModel
        public AudioDeviceDialogViewModel(AudioDeviceType audioDeviceType)
        {
            AudioDeviceType = audioDeviceType;
            AudioDevices = new ObservableCollection<AudioDeviceBase>(AudioAgent.GetWindowsAudioDevices());
        }
        #endregion AudioDeviceDialogViewModel
        #endregion Constructors..

        #region Methods..
        #endregion Methods..
    }
}
