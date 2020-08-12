using NAudio.CoreAudioApi;
using NAudio.Wave;
using SharpDX.DirectSound;
using SoundboardYourFriends.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoundboardYourFriends.Model
{
    public class AudioDevice : ObservableObject, IDisposable
    {
        #region Member Variables..
        #endregion Member Variables..

        #region Properties..
        #region AudioPeak
        private int _AudioPeak;
        public int AudioPeak
        {
            get { return _AudioPeak; }
            set
            {
                _AudioPeak = value;
                RaisePropertyChanged();
            }
        }
        #endregion AudioPeak

        #region DirectSoundOutInstance
        public DirectSoundOut DirectSoundOutInstance { get; set; }
        #endregion DirectSoundOutInstance

        #region DeviceId
        private Guid _deviceId;
        public Guid DeviceId
        {
            get { return _deviceId; }
            set { _deviceId = value; }
        }
        #endregion DeviceId

        #region FriendlyName
        private string _friendlyName;
        public string FriendlyName
        {
            get { return _friendlyName; } 
            set 
            { 
                _friendlyName = value;
                RaisePropertyChanged();
            }
        }
        #endregion FriendlyName

        #region AudioMeterInformation
        private AudioMeterInformation _audioMeterInformation = null;
        public AudioMeterInformation AudioMeterInformation
        {
            get { return _audioMeterInformation; }
            set { _audioMeterInformation = value; }
        }
        #endregion AudioMeterInformation
        #endregion Properties..

        #region Methods..
        #region Dispose
        public void Dispose()
        {
            DirectSoundOutInstance?.Dispose();
        }
        #endregion Dispose
        #endregion Methods..
    }
}
