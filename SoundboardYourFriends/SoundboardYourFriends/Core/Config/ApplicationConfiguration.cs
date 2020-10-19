using NAudio.Wave;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SoundboardYourFriends.Core;
using SoundboardYourFriends.Core.Windows;
using SoundboardYourFriends.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Management;
using System.Windows;
using System.Windows.Input;
using System.Windows.Xps;

namespace SoundboardYourFriends
{
    public static class ApplicationConfiguration
    {
        #region Member Variables..
        private const string CONFIG_FILEPATH = "config.json";

        private const string RECORD_HOTKEY = "RECORD_HOTKEY";
        private const string RECORD_HOTKEY_MODIFIER = "RECORD_HOTKEY_MODIFIER";   
        private const string SAMPLE_DIRECTORY = "SAMPLE_DIRECTORY";
        private const string SAMPLE_HOTKEY_MODIFIER = "SAMPLE_HOTKEY_MODIFIER";
        private const string SAMPLE_LENGTH = "SAMPLE_LENGTH";
        private const string AUDIO_CAPTURE_DEVICES = "AUDIO_CAPTURE_DEVICES";
        private const string AUDIO_OUTPUT_DEVICES = "AUDIO_OUTPUT_DEVICES";
        #endregion Member Variables..

        #region Properties..
        #region SoundboardSampleSeconds
        private static int _soundboardSampleSeconds = 20;
        public static int SoundboardSampleSeconds
        {
            get { return _soundboardSampleSeconds; }
            set { _soundboardSampleSeconds = value; }
        }
        #endregion SoundboardSampleSeconds

        #region AudioCaptureDevices
        private static IEnumerable<AudioCaptureDevice> _audioCaptureDevices;
        public static IEnumerable<AudioCaptureDevice> AudioCaptureDevices
        {
            get { return _audioCaptureDevices; }
            set { _audioCaptureDevices = value; }
        }
        #endregion AudioCaptureDevices

        #region AudioOutputDevices
        private static IEnumerable<AudioOutputDevice> _audioOutputDevices;
        public static IEnumerable<AudioOutputDevice> AudioOutputDevices
        {
            get { return _audioOutputDevices; }
            set { _audioOutputDevices = value; }
        }
        #endregion AudioOutputDevices

        #region RecordKeyModifer
        private static KeyModifier _recordKeyModifier = KeyModifier.None;
        public static KeyModifier RecordKeyModifer
        {
            get { return _recordKeyModifier; }
            set { _recordKeyModifier = value; }
        }
        #endregion RecordKeyModifer

        #region SampleKeyModifier
        private static KeyModifier _sampleKeyModifier = KeyModifier.None;
        public static KeyModifier SampleKeyModifier
        {
            get { return _sampleKeyModifier; }
            set { _sampleKeyModifier = value; }
        }
        #endregion SampleKeyModifier

        #region RecordHotkey
        private static Key _recordHotkey = Key.None;
        public static Key RecordHotkey
        {
            get { return _recordHotkey; }
            set { _recordHotkey = value; }
        }
        #endregion RecordHotkey

        #region SoundboardSampleDirectory
        private static string _soundboardSampleDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"SoundboardYourFriendsAudioSamples");
        public static string SoundboardSampleDirectory
        {
            get { return _soundboardSampleDirectory; }
            set { _soundboardSampleDirectory = value; }
        }
        #endregion SoundboardSampleDirectory
        #endregion Properties..

        #region Constructors..
        #region ApplicationConfiguration
        static ApplicationConfiguration()
        {
            // Save settings on application closing
            Application.Current.Exit += (sender, e) =>
            {
                SaveUserSettings();
            };
        }
        #endregion ApplicationConfiguration
        #endregion Constructors..

        #region Methods..
        #region ImportUserSettings
        public static void ImportUserSettings()
        {
            if (File.Exists(CONFIG_FILEPATH))
            {
                //JObject jsonObject = JObject.Parse(File.ReadAllText(CONFIG_FILEPATH));

                //// Audio Capture Devices
                //AudioCaptureDevices = new List<CaptureDevice>();
                //Properties.Settings.Default.CaptureDeviceIds?.Cast<string>().ToList().ForEach(captureDeviceIdString =>
                //{
                //    ((List<Guid>)AudioCaptureDevices).Add(Guid.Parse(captureDeviceIdString));
                //});

                //// Audio Output Devices
                //AudioOutputDevices = new List<OutputDevice>();
                //Properties.Settings.Default.OutputDeviceIds?.Cast<string>().ToList().ForEach(outputDeviceIdString =>
                //{
                //    ((List<Guid>)AudioOutputDevices).Add(Guid.Parse(outputDeviceIdString));
                //});

                //// Soundboard Sample Directory
                //SoundboardSampleDirectory = Properties.Settings.Default.SoundboardSampleDirectory;

                //// Record Hotkey
                //Enum.TryParse(typeof(Key), Properties.Settings.Default.RecordHotKey, out object hotkey);
                //hotkey = hotkey ?? Key.None;
                //RecordHotkey = (Key)hotkey;

                //// Global Key Modifier
                //Enum.TryParse(typeof(KeyModifier), Properties.Settings.Default.GlobalKeyModifier, out object keyModifer);
                //keyModifer = keyModifer ?? KeyModifier.None;
                //RecordKeyModifer = (KeyModifier)keyModifer;

                //// Soundboard sample size (seconds)
                ////SoundboardSampleSeconds = Properties.Settings.Default.;

                //// Output device playback type
                //OutputDevicePlaybackTypeCollection = new Dictionary<Guid, PlaybackScope>();
                //Properties.Settings.Default.OutputDevicePlaybackTypeCollection?.Cast<string>().ToList().ForEach(outputDevicePlaybackType =>
                //{
                //    try
                //    {
                //        Guid deviceId = Guid.Parse(outputDevicePlaybackType.Split(',')[0]);
                //        PlaybackScope playbackType = (PlaybackScope)Enum.Parse(typeof(PlaybackScope), outputDevicePlaybackType.Split(',')[1]);
                //        OutputDevicePlaybackTypeCollection[deviceId] = playbackType;
                //    }
                //    catch { }
                //});
            }
        }
        #endregion ImportUserSettings

        #region SaveUserSettings
        public static void SaveUserSettings()
        {
            using (var streamWriter = new StreamWriter(CONFIG_FILEPATH))
            using (var jsonWriter = new JsonTextWriter(streamWriter))
            {
                try
                {
                    jsonWriter.CloseOutput = true;
                    jsonWriter.AutoCompleteOnClose = true;

                    // StartOfFile
                    jsonWriter.WriteStartObject();

                    // Record Hotkey
                    jsonWriter.WritePropertyName(RECORD_HOTKEY);
                    jsonWriter.WriteValue(RecordHotkey);

                    // Record Hotkey Modifier
                    jsonWriter.WritePropertyName(RECORD_HOTKEY_MODIFIER);
                    jsonWriter.WriteValue(RecordKeyModifer);

                    // Soundboard Sample Directory
                    jsonWriter.WritePropertyName(SAMPLE_DIRECTORY);
                    jsonWriter.WriteValue(SoundboardSampleDirectory);

                    // Soundboard Sample Key Modifier
                    jsonWriter.WritePropertyName(SAMPLE_HOTKEY_MODIFIER);
                    jsonWriter.WriteValue(SampleKeyModifier);

                    // Soundboard Sample Length (Seconds)
                    jsonWriter.WritePropertyName(SAMPLE_LENGTH);
                    jsonWriter.WriteValue(SoundboardSampleSeconds);

                    // Audio Capture Devices
                    jsonWriter.WritePropertyName(AUDIO_CAPTURE_DEVICES);
                    jsonWriter.WriteStartArray();
                    
                    AudioCaptureDevices.ToList().ForEach(captureDevice =>
                    {
                        jsonWriter.WriteStartObject();
                        
                        // Device Id
                        jsonWriter.WritePropertyName("DEVICE_ID");
                        jsonWriter.WriteValue(captureDevice.DeviceId);
                        
                        jsonWriter.WriteEndObject();
                    });

                    jsonWriter.WriteEndArray();

                    // Audio Output Devices
                    jsonWriter.WritePropertyName(AUDIO_OUTPUT_DEVICES);
                    jsonWriter.WriteStartArray();

                    AudioOutputDevices.ToList().ForEach(outputDevice =>
                    {
                        jsonWriter.WriteStartObject();
                        
                        // Device Id
                        jsonWriter.WritePropertyName("DEVICE_ID");
                        jsonWriter.WriteValue(outputDevice.DeviceId);

                        // Playback Scope
                        jsonWriter.WritePropertyName("PLAYBACK_SCOPE");
                        jsonWriter.WriteValue(outputDevice.PlaybackScope);

                        jsonWriter.WriteEndObject();
                    });

                    jsonWriter.WriteEndArray();

                    // EndOfFile
                    jsonWriter.WriteEndObject();
                }
                catch { }
                finally
                {
                    jsonWriter.Close();
                }
            }
        }
        #endregion SaveUserSettings
        #endregion Methods..
    }
}
