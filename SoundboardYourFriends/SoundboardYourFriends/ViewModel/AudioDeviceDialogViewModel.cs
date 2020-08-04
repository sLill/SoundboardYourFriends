using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using NAudio;
using NAudio.Wave;
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

            using (var deviceEnumerator = new NAudio.CoreAudioApi.MMDeviceEnumerator())
            {
                foreach (var audioDevice in deviceEnumerator.EnumerateAudioEndPoints(NAudio.CoreAudioApi.DataFlow.All, NAudio.CoreAudioApi.DeviceState.Active))
                {
                    List<object> properties = new List<object>();
                    for (int i = 0; i < audioDevice.Properties.Count; i++)
                    {
                        try
                        {
                            properties.Add(audioDevice.Properties[i].Value);
                        }
                        catch { }
                    }
                }
            }
        }
        #endregion GetWindowsAudioDevices
        #endregion Methods..
    }
}
