using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;

namespace SoundboardYourFriends.Model
{
    public class AudioOutputDevice : AudioDeviceBase
    {
        #region Member Variables..
        private DirectSoundOut _directSoundOutInstance { get; set; }
        #endregion Member Variables..

        #region Events..
        public event EventHandler PlaybackStopped;
        #endregion Events..

        #region Properties..
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

        #region PlaybackScope
        private PlaybackScope _playbackScope;
        public PlaybackScope PlaybackScope
        {
            get { return _playbackScope; }
            set { _playbackScope = value; }
        }
        #endregion PlaybackScope
        #endregion Properties..

        #region Constructors..
        #region AudioOutputDevice
        public AudioOutputDevice(Guid deviceId) 
            : base(deviceId) { }
        #endregion AudioOutputDevice

        #region AudioOutputDevice
        public AudioOutputDevice(AudioDeviceBase audioDeviceBase)
            : base(audioDeviceBase) { }
        #endregion AudioOutputDevice
        #endregion Constructors..

        #region Methods..
        #region Event Handlers..
        #region OnDirectSoundPlaybackStopped
        private void OnDirectSoundPlaybackStopped(object sender, EventArgs e)
        {
            this.PlaybackStopped?.Invoke(this, EventArgs.Empty);
            PlaybackState = PlaybackState.Stopped;
        }
        #endregion OnDirectSoundPlaybackStopped
        #endregion Event Handlers..	

        #region Dispose
        public override void Dispose()
        {
            _directSoundOutInstance?.Dispose();
            _directSoundOutInstance = default;

            base.Dispose();
        } 
        #endregion Dispose

        #region Initialize
        public override void Initialize()
        {
            _directSoundOutInstance = new DirectSoundOut(DeviceId);
            _directSoundOutInstance.PlaybackStopped += OnDirectSoundPlaybackStopped;

            base.Initialize();
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
