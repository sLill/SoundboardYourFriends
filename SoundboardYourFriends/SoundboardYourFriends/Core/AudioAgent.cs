using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoundboardYourFriends.Core
{
    public class AudioAgent
    {
        #region Member Variables..
        private WasapiLoopbackCapture _wasapiLoopbackCapture;
        #endregion Member Variables..

        #region Properties..
        #region AudioState
        public AudioState AudioState { get; set; } = AudioState.Idle;
        #endregion AudioState
        #endregion Properties..

        #region EventHandlers/Delegates
        public event EventHandler RecordingStopped;
        #endregion EventHandlers/Delegates

        #region Constructors..
        #region AudioAgent
        public AudioAgent() { }
        #endregion AudioAgent
        #endregion Constructors..

        #region Methods..
        #region BeginAudioRecording
        public void BeginAudioRecording(string outputFilePath)
        {
            this.AudioState = AudioState.Recording;

            _wasapiLoopbackCapture = new WasapiLoopbackCapture();
            WaveFileWriter RecordedAudioWriter = new WaveFileWriter(outputFilePath, _wasapiLoopbackCapture.WaveFormat);

            // When the capturer receives audio, start writing the buffer into the mentioned file
            _wasapiLoopbackCapture.DataAvailable += (s, a) =>
            {
                // Write buffer into the file of the writer instance
                RecordedAudioWriter.Write(a.Buffer, 0, a.BytesRecorded);

                // Note: Highest value here can be used for audio visualization
            };

            // When the capture stops, dispose instances of the capturer and writer
            _wasapiLoopbackCapture.RecordingStopped += (s, a) =>
            {
                RecordedAudioWriter.Dispose();
                RecordedAudioWriter = null;

                _wasapiLoopbackCapture.Dispose();
                _wasapiLoopbackCapture = null;

                this.RecordingStopped?.Invoke(outputFilePath, EventArgs.Empty);
            };

            _wasapiLoopbackCapture.StartRecording();
        }
        #endregion BeginAudioRecording

        #region PlayAudio
        public void PlayAudio(string filePath)
        {
            WaveStream waveStream = new WaveFileReader(filePath);
            WaveChannel32 volumeStream = new WaveChannel32(waveStream);

            WaveOutEvent player = new WaveOutEvent();

            player.Init(volumeStream);
            player.Play();
        }
        #endregion PlayAudio

        #region StopAudioRecording
        public void StopAudioRecording()
        {
            _wasapiLoopbackCapture.StopRecording();

            this.AudioState = AudioState.Idle;
        }
        #endregion StopAudioRecording
        #endregion Methods..
    }
}
