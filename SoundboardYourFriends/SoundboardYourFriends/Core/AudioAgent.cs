using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NAudio.Wave.SampleProviders;
using System.IO;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace SoundboardYourFriends.Core
{
    public static class AudioAgent
    {
        #region Member Variables..
        private static List<byte> _audioByteBuffer = new List<byte>();
        private static List<WaveOutEvent> _waveOutEventCollection;
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
        }
        #endregion AudioAgent
        #endregion Constructors..

        #region Methods..
        #region Event Handlers..
        #region OnPlaybackStopped
        private static void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            ((WaveOutEvent)sender).PlaybackStopped -= OnPlaybackStopped;
        }
        #endregion OnPlaybackStopped
        #endregion Event Handlers..

        #region BeginListening
        private static void BeginListening()
        {
            // Copies about once every 1.5 min when set to 7112000 * 4 (Gives ~20 sec audio clip)
            int audioBufferMax = ApplicationConfiguration.ByteSampleSize * 4;
            WasapiLoopbackCapture = new WasapiLoopbackCapture();

            WasapiLoopbackCapture.DataAvailable += (sender, e) =>
            {
                // Copy a clip-sized chunk of audio to a new byte array upon filling this one up
                if (_audioByteBuffer.Count + e.BytesRecorded > audioBufferMax)
                {
                    List<byte> retainedBytes = _audioByteBuffer.GetRange(_audioByteBuffer.Count - ApplicationConfiguration.ByteSampleSize, ApplicationConfiguration.ByteSampleSize);
                    _audioByteBuffer.Clear();
                    _audioByteBuffer.AddRange(retainedBytes);
                }

                byte[] capturedBytes = new byte[e.BytesRecorded];
                Array.Copy(e.Buffer, 0, capturedBytes, 0, e.BytesRecorded);
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

        #region GetFileAudioDuration
        public static TimeSpan GetFileAudioDuration(string filePath)
        {
            TimeSpan result = TimeSpan.Zero;

            using (var shell = ShellObject.FromParsingName(filePath))
            {
                IShellProperty mediaDurationProperty = shell.Properties.System.Media.Duration;
                var mediaDurationPropertyValue = (ulong)mediaDurationProperty.ValueAsObject;
                result = TimeSpan.FromTicks((long)mediaDurationPropertyValue);
            }

            return result;
        }
        #endregion GetFileAudioDuration

        #region InitializeOutputDevices
        public static void InitializeOutputDevices(IEnumerable<int> outputDeviceIds)
        {
            _waveOutEventCollection = new List<WaveOutEvent>();

            foreach (var deviceId in outputDeviceIds)
            {
                WaveOutEvent waveOutEvent = new WaveOutEvent() { DeviceNumber = deviceId };
                waveOutEvent.PlaybackStopped += OnPlaybackStopped;

                _waveOutEventCollection.Add(waveOutEvent);
            }
        }
        #endregion InitializeOutputDevices

        #region StartAudioPlayback
        public static void StartAudioPlayback(string filePath, PlaybackType playbackType, double beginTime, double endTime)
        {
            StopAudioPlayback();

            _waveOutEventCollection?.ForEach(waveOutEvent =>
            {
                MixingSampleProvider mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));
                AudioFileReader audioFileReader = new AudioFileReader(filePath);

                VolumeSampleProvider volumeSampleProvider = new VolumeSampleProvider(audioFileReader) { Volume = 1.0f };
                ISampleProvider convertedSampleProvider = ConvertToMixerSampleRate(mixer, ConvertToMixerChannelCount(mixer, volumeSampleProvider));

                OffsetSampleProvider offsetSampleProvider = new OffsetSampleProvider(convertedSampleProvider);
                offsetSampleProvider.SkipOver = TimeSpan.FromSeconds(beginTime);
                offsetSampleProvider.Take = TimeSpan.FromSeconds(endTime) - offsetSampleProvider.SkipOver;

                mixer.AddMixerInput(offsetSampleProvider);

                waveOutEvent.Init(mixer);
                waveOutEvent.Play();
            });
        }
        #endregion StartAudioPlayback

        #region StopAudioPlayback
        public static void StopAudioPlayback()
        {
            _waveOutEventCollection?.ForEach(waveOutEvent =>
            {
                if (waveOutEvent.PlaybackState == PlaybackState.Playing)
                {
                    waveOutEvent.Stop();
                }
            });
        }
        #endregion StopAudioPlayback

        #region StopListening
        public static void StopListening()
        {
            WasapiLoopbackCapture.Dispose();
            WasapiLoopbackCapture = null;

            _waveOutEventCollection?.ForEach(waveOutEvent =>
            {
                waveOutEvent.Dispose();
                waveOutEvent = null;
            });
        }
        #endregion StopListening

        #region TrimFile
        public static void TrimFile(string filePath, double beginTime, double endTime)
        {
            byte[] audioInputBuffer;
            using (var waveFileReader = new WaveFileReader(filePath))
            {
                double beginTimeAsPercent = beginTime / GetFileAudioDuration(filePath).Seconds;
                double endTimeAsPercent = endTime / GetFileAudioDuration(filePath).Seconds;

                var fileInfo = new FileInfo(filePath);
                int beginByteIndex = (int)(fileInfo.Length * beginTimeAsPercent);
                int endByteIndex = (int)(fileInfo.Length * endTimeAsPercent);

                // Round to the nearest block
                beginByteIndex = (int)Math.Round((double)beginByteIndex / waveFileReader.BlockAlign) * waveFileReader.BlockAlign;
                endByteIndex = (int)Math.Round((double)endByteIndex / waveFileReader.BlockAlign) * waveFileReader.BlockAlign - 8;

                audioInputBuffer = new byte[endByteIndex - beginByteIndex];
                waveFileReader.Read(audioInputBuffer, beginByteIndex, audioInputBuffer.Length);
            }

            using (var waveFileWriter = new WaveFileWriter(filePath, WasapiLoopbackCapture.WaveFormat))
            {
                waveFileWriter.Write(audioInputBuffer, 0, audioInputBuffer.Length);
            }
        }
        #endregion TrimFile

        #region WriteAudioBufferToFile
        public static void WriteAudioBufferToFile()
        {
            string fileName = $"AudioSample_{DateTime.Now.ToString("yyyyMMddHHmmss")}.wav";
            string fileNameFull = Path.Combine(ApplicationConfiguration.SoundboardSampleDirectory, fileName);

            WaveFormat waveFormat = WasapiLoopbackCapture.WaveFormat;
            using (WaveFileWriter waveFileWriter= new WaveFileWriter(fileNameFull, WasapiLoopbackCapture.WaveFormat))
            {
                var bytesToWrite = ApplicationConfiguration.ByteSampleSize > _audioByteBuffer.Count ? _audioByteBuffer.ToArray() : _audioByteBuffer.GetRange(_audioByteBuffer.Count - ApplicationConfiguration.ByteSampleSize, ApplicationConfiguration.ByteSampleSize).ToArray();
                waveFileWriter.Write(bytesToWrite, 0, bytesToWrite.Length);
            }

            _audioByteBuffer.Clear();
            AudioAgent.FileWritten?.Invoke(fileNameFull, EventArgs.Empty);
        }
        #endregion WriteAudioBufferToFile
        #endregion Methods..
    }
}
