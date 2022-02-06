using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using SharpDX.DirectSound;
using SoundboardYourFriends.Core.Config;
using SoundboardYourFriends.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

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
        #endregion Constructors..

        #region Methods..
        #region Event Handlers..
        #endregion Event Handlers..

        #region BeginAudioPlayback
        public static void BeginAudioPlayback(string filePath, AudioOutputDevice audioOutputDevice, float volume, double beginTime, double endTime)
        {
            try
            {
                AudioFileReader audioFileReader = new AudioFileReader(filePath);

                // Configure mixer
                MixingSampleProvider mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(audioFileReader.WaveFormat.SampleRate, 2));

                // Force audio to stereo 2-channel
                MultiplexingWaveProvider waveProvider = new MultiplexingWaveProvider(new IWaveProvider[] { audioFileReader.ToWaveProvider() }, audioFileReader.WaveFormat.Channels > 1 ? 2 : 1);
                
                // Apply volume preferences
                VolumeSampleProvider volumeSampleProvider = new VolumeSampleProvider(waveProvider.ToSampleProvider()) { Volume = volume };
                
                // Force sample rate
                ISampleProvider convertedSampleProvider = ConvertToMixerSampleRate(mixer, ConvertToMixerChannelCount(mixer, volumeSampleProvider));

                // Seek
                OffsetSampleProvider offsetSampleProvider = new OffsetSampleProvider(convertedSampleProvider);
                offsetSampleProvider.SkipOver = TimeSpan.FromSeconds(beginTime);
                offsetSampleProvider.Take = TimeSpan.FromSeconds(endTime) - offsetSampleProvider.SkipOver;

                // Configure mixer
                mixer.AddMixerInput(offsetSampleProvider);

                audioOutputDevice.InitializeAndPlay(mixer);
                audioOutputDevice.PlaybackStopped += (sender, e) =>
                {
                    //audioFileReader.Close();
                    audioFileReader.Dispose();
                };
            }
            catch (Exception ex)
            {
                ApplicationLogger.Log(ex.Message, ex.StackTrace);
            }
        }
        #endregion BeginAudioPlayback

        #region BeginAudioCapture
        public static void BeginAudioCapture(AudioCaptureDevice audioCaptureDevice)
        {
            try
            {
                StopAudioCapture();

                // Determines the audio buffer size in relation to the parent buffer. Bigger multiplier = smaller proportion. 
                // Larger multiplier also means dumping and copying to a new parent buffer less frequently at the cost of using a larger chunk of virtual memory
                // ex. If multiplier is 4, the audio buffer for a single soundboard sample will takeup 25% of the larger buffer
                int audioBufferMultiplier = 5;

                if (audioCaptureDevice.MMDeviceInstance != null)
                {
                    WasapiLoopbackCapture = new WasapiLoopbackCapture(audioCaptureDevice.MMDeviceInstance);

                    int audioSampleSize = ApplicationConfiguration.Instance.SoundboardSampleSeconds * WasapiLoopbackCapture.WaveFormat.AverageBytesPerSecond;
                    int audioBufferMax = audioSampleSize * audioBufferMultiplier;

                    WasapiLoopbackCapture.DataAvailable += (sender, e) =>
                    {
                        // Copy a clip-sized chunk of audio to a new large buffer upon filling this one up
                        if (_audioByteBuffer.Count + e.BytesRecorded > audioBufferMax)
                        {
                            List<byte> retainedBytes = _audioByteBuffer.GetRange(_audioByteBuffer.Count - audioSampleSize, audioSampleSize);
                            _audioByteBuffer.Clear();
                            _audioByteBuffer.AddRange(retainedBytes);
                        }

                        byte[] capturedBytes = new byte[e.BytesRecorded];
                        Array.Copy(e.Buffer, 0, capturedBytes, 0, e.BytesRecorded);
                        _audioByteBuffer.AddRange(capturedBytes);
                    };

                    WasapiLoopbackCapture.StartRecording();
                }
            }
            catch (Exception ex)
            {
                ApplicationLogger.Log(ex.Message, ex.StackTrace);
            }
        }
        #endregion BeginAudioCapture

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

            try
            {
                using (var shell = ShellObject.FromParsingName(filePath))
                {
                    IShellProperty mediaDurationProperty = shell.Properties.System.Media.Duration;
                    var mediaDurationPropertyValue = (ulong)mediaDurationProperty.ValueAsObject;
                    result = TimeSpan.FromTicks((long)mediaDurationPropertyValue);
                }
            }
            catch (Exception ex)
            {
                ApplicationLogger.Log(ex.Message, ex.StackTrace);
            }

            return result;
        }
        #endregion GetFileAudioDuration

        #region GetWindowsAudioDevices
        public static IEnumerable<AudioDeviceBase> GetWindowsAudioDevices()
        {
            var audioDevices = new List<AudioDeviceBase>();

            try
            {
                Dictionary<Guid, MMDevice> systemMMDeviceCollection = new Dictionary<Guid, MMDevice>();
                using (var deviceEnumerator = new MMDeviceEnumerator())
                {
                    foreach (var device in deviceEnumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active))
                    {
                        try
                        {
                            var deviceIdString = device.Properties[WindowsDevicePropertyKeys.PKEY_AudioEndpoint_GUID.ToNAudioPropertyKey()]?.Value.ToString();

                            Regex guidRegex = new Regex(@"{(?<Guid>.{8}-.{4}-.{4}-.{4}-.{12})}");
                            deviceIdString = guidRegex.Match(deviceIdString).Groups["Guid"].Value;
                            Guid deviceId = Guid.Parse(deviceIdString);

                            systemMMDeviceCollection[deviceId] = device;
                        }
                        catch { }
                    }
                }

                DirectSound.GetDevices().ForEach(audioDevice =>
                {
                    var mmDeviceInstance = systemMMDeviceCollection.ContainsKey(audioDevice.DriverGuid) ? systemMMDeviceCollection[audioDevice.DriverGuid] : null;

                    audioDevices.Add(new AudioDeviceBase(audioDevice.DriverGuid)
                    {
                        FriendlyName = audioDevice.Description,
                        MMDeviceInstance = mmDeviceInstance,
                        AudioMeterInformation = mmDeviceInstance?.AudioMeterInformation
                    });
                });
            }
            catch (Exception ex)
            {
                ApplicationLogger.Log(ex.Message, ex.StackTrace);
            }

            return audioDevices;
        }
        #endregion GetWindowsAudioDevices

        #region StopAudioPlayback
        public static void StopAudioPlayback(List<AudioOutputDevice> audioDeviceCollection)
        {
            audioDeviceCollection.ForEach(audioDevice =>
            {
                if (audioDevice.PlaybackState == PlaybackState.Playing)
                {
                    audioDevice.Stop();
                }
            });
        }
        #endregion StopAudioPlayback

        #region StopAudioCapture
        public static void StopAudioCapture()
        {
            if (WasapiLoopbackCapture?.CaptureState == CaptureState.Capturing)
            {
                WasapiLoopbackCapture.StopRecording();
            }

            WasapiLoopbackCapture?.Dispose();
            WasapiLoopbackCapture = null;
        }
        #endregion StopAudioCapture

        #region TrimFile
        public static void TrimFile(string filepath, double beginTimeMilliseconds, double endTimeMilliseconds)
        {
            try
            {
                List<byte> inputBuffer = new List<byte>();
                WaveFormat waveFormat = null;

                using (WaveFileReader reader = new WaveFileReader(filepath))
                {
                    waveFormat = reader.WaveFormat;

                    int bytesPerMillisecond = reader.WaveFormat.AverageBytesPerSecond / 1000;

                    int startByte = (int)beginTimeMilliseconds * bytesPerMillisecond;
                    startByte = startByte - startByte % reader.WaveFormat.BlockAlign;

                    int endByte = (int)endTimeMilliseconds * bytesPerMillisecond;
                    endByte = endByte - endByte % reader.WaveFormat.BlockAlign;

                    reader.Position = startByte;
                    byte[] buffer = new byte[1024];

                    while (reader.Position < endByte)
                    {
                        int bytesRequired = (int)(endByte - reader.Position);
                        if (bytesRequired > 0)
                        {
                            int bytesToRead = Math.Min(bytesRequired, buffer.Length);
                            int bytesRead = reader.Read(buffer, 0, bytesToRead);

                            if (bytesRead > 0)
                            {
                                inputBuffer.AddRange(buffer.ToList().GetRange(0, bytesRead));
                            }
                        }
                    }

                }

                using (WaveFileWriter writer = new WaveFileWriter(filepath, waveFormat))
                {
                    writer.Write(inputBuffer.ToArray(), 0, inputBuffer.Count);
                }
            }
            catch (Exception ex)
            {
                ApplicationLogger.Log(ex.Message, ex.StackTrace);
            }
        }
        #endregion TrimFile

        #region WriteAudioBufferToFile
        public static void WriteAudioBufferToFile()
        {
            try
            {
                if (WasapiLoopbackCapture != null)
                {
                    Directory.CreateDirectory(ApplicationConfiguration.Instance.SoundboardSampleDirectory);

                    string fileName = $"AudioSample_{DateTime.Now.ToString("yyyyMMddHHmmss")}.wav";
                    string fileNameFull = Path.Combine(ApplicationConfiguration.Instance.SoundboardSampleDirectory, fileName);

                    using (WaveFileWriter waveFileWriter = new WaveFileWriter(fileNameFull, WasapiLoopbackCapture.WaveFormat))
                    {
                        int audioSampleSize = ApplicationConfiguration.Instance.SoundboardSampleSeconds * WasapiLoopbackCapture.WaveFormat.AverageBytesPerSecond;
                        var bytesToWrite = audioSampleSize > _audioByteBuffer.Count ? _audioByteBuffer.ToArray() : _audioByteBuffer.GetRange(_audioByteBuffer.Count - audioSampleSize, audioSampleSize).ToArray();
                        waveFileWriter.Write(bytesToWrite, 0, bytesToWrite.Length);
                    }

                    _audioByteBuffer.Clear();
                    AudioAgent.FileWritten?.Invoke(fileNameFull, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                ApplicationLogger.Log(ex.Message, ex.StackTrace);
            }
        }
        #endregion WriteAudioBufferToFile
        #endregion Methods..
    }
}
