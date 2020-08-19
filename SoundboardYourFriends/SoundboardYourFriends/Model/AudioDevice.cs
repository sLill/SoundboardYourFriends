using Microsoft.Windows.Themes;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using SoundboardYourFriends.Core;
using System;

namespace SoundboardYourFriends.Model
{
    public class AudioDevice : ObservableObject, IDisposable
    {
        #region Member Variables..
        private DirectSoundOut _directSoundOutInstance { get; set; }
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

        #region PlaybackState
        private PlaybackState _playbackState = PlaybackState.Stopped;
        public PlaybackState PlaybackState
        {
            get { return _playbackState; }
            set
            {
                _playbackState = value;
                RaisePropertyChanged();
            }
        }
        #endregion PlaybackState

        #region PlaybackType
        private PlaybackType _playbackType;
        public PlaybackType PlaybackType
        {
            get { return _playbackType; }
            set { _playbackType = value; }
        }
        #endregion PlaybackType
        #endregion Properties..

        #region Events..
        public event EventHandler PlaybackStopped;
        #endregion Events..

        #region Constructors..
        #region AudioDevice
        public AudioDevice(Guid deviceId)
        {
            DeviceId = deviceId;
            Initialize();
        }
        #endregion AudioDevice
        #endregion Constructors..

        #region Methods..
        #region Dispose
        public void Dispose()
        {
            _directSoundOutInstance?.Dispose();
        }
        #endregion Dispose

        #region Initialize
        public void Initialize()
        {
            _directSoundOutInstance = new DirectSoundOut(DeviceId);
            _directSoundOutInstance.PlaybackStopped += (sender, e) =>
            {
                this.PlaybackStopped?.Invoke(this, EventArgs.Empty);
                PlaybackState = PlaybackState.Stopped;
            };
        }
        #endregion Initialize

        #region InitializeAndPlay
        public void InitializeAndPlay(MixingSampleProvider mixer)
        {
            _directSoundOutInstance.Init(mixer);
            _directSoundOutInstance.Play();

            PlaybackState = PlaybackState.Playing;
        }
        #endregion InitializeAndPlay

        #region Stop
        public void Stop()
        {
            _directSoundOutInstance.Stop();
        }
        #endregion Stop
        #endregion Methods..
    }
}
