using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;
//using SharpDX.DirectSound;
using System.Linq;

namespace SoundboardYourFriends.Core
{
    public static class AudioAgent
    {
        #region Member Variables..
        private static BufferedWaveProvider _bufferedWaveProvider;
        private static WasapiLoopbackCapture _wasapiLoopbackCapture;
        #endregion Member Variables..

        #region Properties..
        #region AudioState
        public static AudioState AudioState { get; set; } = AudioState.Idle;
        #endregion AudioState
        #endregion Properties..

        #region EventHandlers/Delegates
        public static event EventHandler RecordingStopped;
        #endregion EventHandlers/Delegates

        #region Constructors..
        #region AudioAgent
        static AudioAgent() 
        {
            BeginListening();
        }
        #endregion AudioAgent
        #endregion Constructors..

        #region Methods..
        #region BeginListening
        private static void BeginListening()
        {
            MMDeviceEnumerator deviceEnumerator = new MMDeviceEnumerator();
            foreach (var device in deviceEnumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active))
            {

            }

            _wasapiLoopbackCapture = new WasapiLoopbackCapture();
            _bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat()) 
            { 
                BufferDuration = new TimeSpan(0, 2, 0), 
                DiscardOnBufferOverflow = true 
            };

            // When the capturer receives audio, start writing the buffer into the mentioned file
            _wasapiLoopbackCapture.DataAvailable += (s, a) =>
            {
                _bufferedWaveProvider.AddSamples(a.Buffer, 0, a.BytesRecorded);

                // Note: Highest value here can be used for audio visualization
            };

            _wasapiLoopbackCapture.StartRecording();
        }
        #endregion BeginListening

        #region PlayAudio
        public static void PlayAudio(string filePath)
        {
            WaveStream waveStream = new WaveFileReader(filePath);
            WaveChannel32 volumeStream = new WaveChannel32(waveStream);

            //WaveOutEvent player = new WaveOutEvent();

            //var devices = DirectSound.GetDevices();
            //var g = DirectSoundCapture.GetDevices();
            //DirectSoundCapture dsc = new DirectSoundCapture();

            using (DirectSoundOut directSoundOut = new DirectSoundOut(new Guid("af78ec66-3a04-4f77-a633-6b298601d14c")))
            {
                directSoundOut.Init(volumeStream);
                directSoundOut.Play();
            }

            //player.Init(volumeStream);
            //player.Play();
        }
        #endregion PlayAudio

        #region StopListening
        public static void StopListening()
        {
            _wasapiLoopbackCapture.Dispose();
            _wasapiLoopbackCapture = null;
        }
        #endregion StopListening

        #region WriteAudioBuffer
        public static void WriteAudioBuffer(string outputFilePath)
        {
            using (WaveFileWriter RecordedAudioWriter = new WaveFileWriter(outputFilePath, _wasapiLoopbackCapture.WaveFormat))
            {
                byte[] byteBuffer = new byte[_bufferedWaveProvider.BufferedBytes];
                _bufferedWaveProvider.Read(byteBuffer, 0, _bufferedWaveProvider.BufferedBytes);

                RecordedAudioWriter.Write(byteBuffer, 0, byteBuffer.Length);
                _bufferedWaveProvider.ClearBuffer();
            }

            RecordingStopped?.Invoke(outputFilePath, EventArgs.Empty);
        }
        #endregion WriteAudioBuffer
        #endregion Methods..
    }
}
