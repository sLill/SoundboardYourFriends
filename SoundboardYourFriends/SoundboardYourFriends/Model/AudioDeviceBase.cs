using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using SoundboardYourFriends.Core;
using System;

namespace SoundboardYourFriends.Model
{
    public class AudioDeviceBase : ObservableObject, IDisposable
    {
        #region Member Variables..
        #endregion Member Variables..

        #region Properties..
        #region AudioMeterInformation
        private AudioMeterInformation _audioMeterInformation = null;
        public AudioMeterInformation AudioMeterInformation
        {
            get { return _audioMeterInformation; }
            set { _audioMeterInformation = value; }
        }
        #endregion AudioMeterInformation

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

        #region DeviceActive
        private bool _deviceActive;
        public bool DeviceActive
        {
            get { return _deviceActive; }
            set { _deviceActive = value; }
        }
        #endregion DeviceActive

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

        #region MMDeviceInstance
        private MMDevice _mmDeviceInstance = null;
        public MMDevice MMDeviceInstance
        {
            get { return _mmDeviceInstance; }
            set { _mmDeviceInstance = value; }
        }
        #endregion MMDeviceInstance
        #endregion Properties..

        #region Constructors..
        #region AudioDevice
        public AudioDeviceBase(Guid deviceId)
        {
            DeviceId = deviceId;
            Initialize();
        }
        #endregion AudioDevice

        #region AudioDevice
        public AudioDeviceBase(AudioDeviceBase audioDeviceBase)
        {
            AudioMeterInformation = audioDeviceBase.AudioMeterInformation;
            AudioPeak = audioDeviceBase.AudioPeak;
            DeviceActive = audioDeviceBase.DeviceActive;
            DeviceId = audioDeviceBase.DeviceId;
            FriendlyName = audioDeviceBase.FriendlyName;
            MMDeviceInstance = audioDeviceBase.MMDeviceInstance;

            Initialize();
        }
        #endregion AudioDevice
        #endregion Constructors..

        #region Methods..
        #region Event Handlers..
        #endregion Event Handlers..

        #region Dispose
        public virtual void Dispose()
        {
            MMDeviceInstance?.Dispose();
            MMDeviceInstance = default;
        }
        #endregion Dispose

        #region Initialize
        public virtual void Initialize() { }
        #endregion Initialize 
        #endregion Methods..
    }
}
