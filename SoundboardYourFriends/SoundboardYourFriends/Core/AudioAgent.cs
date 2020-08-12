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
using SharpDX.DirectSound;
using SoundboardYourFriends.Model;
using System.Text.RegularExpressions;
using SoundboardYourFriends.Core;

namespace SoundboardYourFriends.Core
{
    public static class AudioAgent
    {
        #region Member Variables..
        private static List<byte> _audioByteBuffer = new List<byte>();
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
            BeginCapturing();
        }
        #endregion AudioAgent
        #endregion Constructors..

        #region Methods..
        #region Event Handlers..
        #endregion Event Handlers..

        #region BeginAudioPlayback
        public static void BeginAudioPlayback(string filePath, List<AudioDevice> audioDeviceCollection, PlaybackType playbackType, double beginTime, double endTime)
        {
            audioDeviceCollection.ForEach(audioDevice =>
            {
                AudioFileReader audioFileReader = new AudioFileReader(filePath);
                MixingSampleProvider mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));

                VolumeSampleProvider volumeSampleProvider = new VolumeSampleProvider(audioFileReader) { Volume = 1.0f };
                ISampleProvider convertedSampleProvider = ConvertToMixerSampleRate(mixer, ConvertToMixerChannelCount(mixer, volumeSampleProvider));

                OffsetSampleProvider offsetSampleProvider = new OffsetSampleProvider(convertedSampleProvider);
                offsetSampleProvider.SkipOver = TimeSpan.FromSeconds(beginTime);
                offsetSampleProvider.Take = TimeSpan.FromSeconds(endTime) - offsetSampleProvider.SkipOver;

                mixer.AddMixerInput(offsetSampleProvider);

                audioDevice.DirectSoundOutInstance.Init(mixer);
                audioDevice.DirectSoundOutInstance.Play();
                audioDevice.DirectSoundOutInstance.PlaybackStopped += (sender, e) =>
                {
                         //audioFileReader.Close();
                         audioFileReader.Dispose();
                };
            });
        }
        #endregion BeginAudioPlayback

        #region BeginCapturing
        private static void BeginCapturing()
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
            };

            WasapiLoopbackCapture.StartRecording();
        }
        #endregion BeginCapturing

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

        #region GetWindowsAudioDevices
        public static IEnumerable<AudioDevice> GetWindowsAudioDevices()
        {
            var audioDevices = new List<AudioDevice>();

            Dictionary<Guid, MMDevice> systemMMDeviceCollection = new Dictionary<Guid, MMDevice>();
            using (var deviceEnumerator = new MMDeviceEnumerator())
            {
                foreach (var device in deviceEnumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active))
                {
                    try
                    {
                        var deviceIdString = device.Properties[PropertyKeyLookup.PKEY_AudioEndpoint_GUID.NAudioPropertyKey]?.Value.ToString();

                        Regex guidRegex = new Regex(@"{(?<Guid>.{8}-.{4}-.{4}-.{4}-.{12})}");
                        deviceIdString = guidRegex.Match(deviceIdString).Groups["Guid"].Value;
                        Guid deviceId = Guid.Parse(deviceIdString);

                        systemMMDeviceCollection[deviceId] = device;
                    }
                    catch (Exception ex)
                    { }
                }
            }

            DirectSound.GetDevices().ForEach(audioDevice =>
            {
                var mmDeviceInstance = systemMMDeviceCollection.ContainsKey(audioDevice.DriverGuid) ? systemMMDeviceCollection[audioDevice.DriverGuid] : null;

                audioDevices.Add(new AudioDevice()
                {
                    FriendlyName = audioDevice.Description,
                    DeviceId = audioDevice.DriverGuid,
                    DirectSoundOutInstance = new DirectSoundOut(audioDevice.DriverGuid),
                    AudioMeterInformation = mmDeviceInstance?.AudioMeterInformation
                });
            });

            return audioDevices;
        }
        #endregion GetWindowsAudioDevices

        #region StopAudioPlayback
        public static void StopAudioPlayback(List<AudioDevice> audioDeviceCollection)
        {
            audioDeviceCollection.ForEach(audioDevice =>
            {
                if (audioDevice.DirectSoundOutInstance.PlaybackState == PlaybackState.Playing)
                {
                    audioDevice.DirectSoundOutInstance.Stop();
                }
            });
        }
        #endregion StopAudioPlayback

        #region StopListening
        public static void StopListening()
        {
            WasapiLoopbackCapture.Dispose();
            WasapiLoopbackCapture = null;
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

                int beginByteIndex = (int)(waveFileReader.Length * beginTimeAsPercent);
                int endByteIndex = (int)(waveFileReader.Length * endTimeAsPercent);

                // Round to the nearest block
                beginByteIndex = (int)Math.Round((double)beginByteIndex / waveFileReader.BlockAlign) * waveFileReader.BlockAlign;
                endByteIndex = (int)Math.Round((double)endByteIndex / waveFileReader.BlockAlign) * waveFileReader.BlockAlign - 8;

                audioInputBuffer = new byte[endByteIndex - beginByteIndex];

                waveFileReader.Seek(beginByteIndex, SeekOrigin.Begin);
                waveFileReader.Read(audioInputBuffer, 0, audioInputBuffer.Length);
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
            Directory.CreateDirectory(ApplicationConfiguration.SoundboardSampleDirectory);

            string fileName = $"AudioSample_{DateTime.Now.ToString("yyyyMMddHHmmss")}.wav";
            string fileNameFull = Path.Combine(ApplicationConfiguration.SoundboardSampleDirectory, fileName);

            WaveFormat waveFormat = WasapiLoopbackCapture.WaveFormat;
            using (WaveFileWriter waveFileWriter = new WaveFileWriter(fileNameFull, WasapiLoopbackCapture.WaveFormat))
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
