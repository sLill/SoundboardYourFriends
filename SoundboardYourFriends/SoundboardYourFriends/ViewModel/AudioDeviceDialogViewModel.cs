using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using NAudio;
using NAudio.Wave;
using SoundboardYourFriends.Core;
using SoundboardYourFriends.Model;
using SharpDX.DirectSound;

namespace SoundboardYourFriends.ViewModel
{
    public class AudioDeviceDialogViewModel : ObservableObject
    {
        #region Member Variables..
        private ObservableCollection<AudioDevice> _audioDevices;
        #endregion Member Variables..

        #region Properties..
        #region AudioDevices
        public ObservableCollection<AudioDevice> AudioDevices
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
            AudioDevices = new ObservableCollection<AudioDevice>(AudioAgent.GetWindowsAudioDevices());
        }
        #endregion AudioDeviceDialogViewModel
        #endregion Constructors..

        #region Methods..
        #endregion Methods..
    }
}
