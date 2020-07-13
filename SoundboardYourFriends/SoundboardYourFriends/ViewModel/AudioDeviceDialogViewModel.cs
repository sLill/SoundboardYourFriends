using System;
using System.Collections.Generic;
using System.Text;
using NAudio;
using SoundboardYourFriends.Core;

namespace SoundboardYourFriends.ViewModel
{
    public class AudioDeviceDialogViewModel : ObservableObject
    {
        #region Member Variables..
        #endregion Member Variables..

        #region Properties..
        #region AudioDevices
        public List<string> AudioDevices { get; private set; }
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
            AudioDevices = new List<string>();

            var deviceEnumerator = new NAudio.CoreAudioApi.MMDeviceEnumerator();
            NAudio.CoreAudioApi.DataFlow dataFlow = NAudio.CoreAudioApi.DataFlow.All;

            switch (AudioDeviceType)
            {
                case AudioDeviceType.Input:
                    dataFlow = NAudio.CoreAudioApi.DataFlow.Capture;
                    break;
                case AudioDeviceType.Output:
                    dataFlow = NAudio.CoreAudioApi.DataFlow.Render;
                    break;
            }

            foreach (var endpoint in deviceEnumerator.EnumerateAudioEndPoints(dataFlow, NAudio.CoreAudioApi.DeviceState.Active))
            {
                AudioDevices.Add(endpoint.FriendlyName);
            }
        }
        #endregion GetWindowsAudioDevices
        #endregion Methods..
    }
}
