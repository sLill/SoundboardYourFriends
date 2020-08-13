﻿using NAudio.CoreAudioApi;
using NAudio.Wave;
using SoundboardYourFriends.Core;
using System;

namespace SoundboardYourFriends.Model
{
    public class AudioDevice : ObservableObject, IDisposable
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

        #region GlobalPlaybackEnabled
        private bool _globalPlaybackEnabled;
        public bool GlobalPlaybackEnabled
        {
            get { return _globalPlaybackEnabled; }
            set
            {
                _globalPlaybackEnabled = value;
                RaisePropertyChanged();
            }
        }
        #endregion GlobalPlaybackEnabled

        #region LocalPlaybackEnabled
        private bool _localPlaybackEnabled;
        public bool LocalPlaybackEnabled
        {
            get { return _localPlaybackEnabled; }
            set
            {
                _localPlaybackEnabled = value;
                RaisePropertyChanged();
            }
        }
        #endregion LocalPlaybackEnabled
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
