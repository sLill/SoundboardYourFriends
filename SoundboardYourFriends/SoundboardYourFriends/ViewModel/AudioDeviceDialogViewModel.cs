using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using NAudio;
using SoundboardYourFriends.Core;
using SoundboardYourFriends.Model;

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

            GetWindowsAudioDevices();
        }
        #endregion AudioDeviceDialogViewModel
        #endregion Constructors..

        #region Methods..
        #region GetWindowsAudioDevices
        private void GetWindowsAudioDevices()
        {
            AudioDevices = new ObservableCollection<AudioDevice>();

            var deviceEnumerator = new NAudio.CoreAudioApi.MMDeviceEnumerator();
            NAudio.CoreAudioApi.DataFlow dataFlow = NAudio.CoreAudioApi.DataFlow.All;

            //switch (AudioDeviceType)
            //{
            //    case AudioDeviceType.Input:
            //        dataFlow = NAudio.CoreAudioApi.DataFlow.Capture;
            //        break;
            //    case AudioDeviceType.Output:
            //        dataFlow = NAudio.CoreAudioApi.DataFlow.Render;
            //        break;
            //}

            foreach (var endpoint in deviceEnumerator.EnumerateAudioEndPoints(dataFlow, NAudio.CoreAudioApi.DeviceState.Active))
            {
                AudioDevices.Add(new AudioDevice() { FriendlyName = endpoint.FriendlyName });
            }
        }
        #endregion GetWindowsAudioDevices
        #endregion Methods..
    }
}
