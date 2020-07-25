using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NAudio.Wave.SampleProviders;
using System.IO;
using SoundboardYourFriends.Settings;

namespace SoundboardYourFriends.Core
{
    public static class AudioAgent
    {
        #region Member Variables..
        private static List<byte> _audioByteBuffer = new List<byte>();
        private static WaveOutEvent _waveOutEvent;
        #endregion Member Variables..

        #region Properties..
        #region WasapiLoopbackCapture
        public static WasapiLoopbackCapture WasapiLoopbackCapture { get; set; }
        #endregion WasapiLoopbackCapture
        #endregion Properties..

        #region EventHandlers/Delegates
        public static event EventHandler FileWritten;
        #endregion EventHandlers/Delegates

        #region Constructors..
        #region AudioAgent
        static AudioAgent() 
        {
            BeginListening();

            _waveOutEvent = new WaveOutEvent();
            _waveOutEvent.PlaybackStopped += OnPlaybackStopped;
        }
        #endregion AudioAgent
        #endregion Constructors..

        #region Methods..
        #region Event Handlers..
        #region OnPlaybackStopped
        private static void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            _waveOutEvent.PlaybackStopped -= OnPlaybackStopped;
        }
        #endregion OnPlaybackStopped
        #endregion Event Handlers..

        #region BeginListening
        private static void BeginListening()
        {
            // Copies about once every 1.5 min when set to 7112000 * 4 (Gives ~20 sec audio clip)
            int audioBufferMax = SettingsManager.ByteSampleSize * 4;
            WasapiLoopbackCapture = new WasapiLoopbackCapture();

            WasapiLoopbackCapture.DataAvailable += (s, a) =>
            {
                // Copy a clip-sized chunk of audio to a new byte array upon filling this one up
                if (_audioByteBuffer.Count + a.BytesRecorded > audioBufferMax)
                {
                    List<byte> retainedBytes = _audioByteBuffer.GetRange(_audioByteBuffer.Count - SettingsManager.ByteSampleSize, SettingsManager.ByteSampleSize);
                    _audioByteBuffer.Clear();
                    _audioByteBuffer.AddRange(retainedBytes);
                }

                byte[] capturedBytes = new byte[a.BytesRecorded];
                Array.Copy(a.Buffer, 0, capturedBytes, 0, a.BytesRecorded);
                _audioByteBuffer.AddRange(capturedBytes);

                // Note: Highest value here can be used for audio visualization
            };

            WasapiLoopbackCapture.StartRecording();
        }
        #endregion BeginListening

        #region ConvertToMixerChannelCount
        private static ISampleProvider ConvertToMixerChannelCount(ISampleProvider mixer, ISampleProvider input)
        {
            return input.WaveFormat.Channels < mixer.WaveFormat.Channels ? new MonoToStereoSampleProvider(input) : input;
        }
        #endregion ConvertToMixerChannelCount

        #region ConvertToMixerSampleRate
        private static ISampleProvider ConvertToMixerSampleRate(ISampleProvider mixer, ISampleProvider input)
        {
            return input.WaveFormat.SampleRate != mixer.WaveFormat.SampleRate ? new WdlResamplingSampleProvider(input, mixer.WaveFormat.SampleRate) : input;
        }
        #endregion ConvertToMixerSampleRate

        #region PlayAudio
        public static void PlayAudio(string filePath)
        {
            MixingSampleProvider mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));
            AudioFileReader audioFileReader = new AudioFileReader(filePath);
            VolumeSampleProvider volumeSampleProvider = new VolumeSampleProvider(audioFileReader) { Volume = 1.0f };

            ISampleProvider convertedSampleProvider = ConvertToMixerSampleRate(mixer, ConvertToMixerChannelCount(mixer, volumeSampleProvider));
            mixer.AddMixerInput(convertedSampleProvider);

            int vbCableDeviceNumberr = -1;          
            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                var audioDevice = WaveOut.GetCapabilities(i);
                if (audioDevice.ProductName.Contains("CABLE"))
                {
                    vbCableDeviceNumberr = i;
                }
            }

            _waveOutEvent.DeviceNumber = vbCableDeviceNumberr;
            
            _waveOutEvent.Init(mixer);
            _waveOutEvent.Play();
        }
        #endregion PlayAudio

        #region StopListening
        public static void StopListening()
        {
            WasapiLoopbackCapture.Dispose();
            WasapiLoopbackCapture = null;

            _waveOutEvent.Dispose();
            _waveOutEvent = null;
        }
        #endregion StopListening

        #region WriteAudioBuffer
        public static void WriteAudioBuffer()
        {
            string fileName = $"AudioSample_{DateTime.Now.ToString("yyyyMMddHHmmss")}.wav";
            string fileNameFull = Path.Combine(SettingsManager.SoundboardSampleDirectory, fileName);

            WaveFormat waveFormat = WasapiLoopbackCapture.WaveFormat;
            using (WaveFileWriter waveFileWriter= new WaveFileWriter(fileNameFull, WasapiLoopbackCapture.WaveFormat))
            {
                var bytesToWrite = SettingsManager.ByteSampleSize > _audioByteBuffer.Count ? _audioByteBuffer.ToArray() : _audioByteBuffer.GetRange(_audioByteBuffer.Count - SettingsManager.ByteSampleSize, SettingsManager.ByteSampleSize).ToArray();
                waveFileWriter.Write(bytesToWrite, 0, bytesToWrite.Length);
            }

            _audioByteBuffer.Clear();
            AudioAgent.FileWritten?.Invoke(fileNameFull, EventArgs.Empty);
        }
        #endregion WriteAudioBuffer
        #endregion Methods..
    }
}
