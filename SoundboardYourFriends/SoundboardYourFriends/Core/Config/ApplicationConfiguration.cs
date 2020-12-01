using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SoundboardYourFriends.Core;
using SoundboardYourFriends.Core.Windows;
using SoundboardYourFriends.Model;
using SoundboardYourFriends.Model.JsonConverters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace SoundboardYourFriends.Core.Config
{
    public class ApplicationConfiguration
    {
        #region Member Variables..
        private string _localAppDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        private const string RECORD_HOTKEY = "RECORD_HOTKEY";
        private const string RECORD_HOTKEY_MODIFIER = "RECORD_HOTKEY_MODIFIER";   
        private const string SAMPLE_DIRECTORY = "SAMPLE_DIRECTORY";
        private const string SAMPLE_HOTKEY_MODIFIER = "SAMPLE_HOTKEY_MODIFIER";
        private const string SAMPLE_LENGTH = "SAMPLE_LENGTH";
        private const string AUDIO_CAPTURE_DEVICES = "AUDIO_CAPTURE_DEVICES";
        private const string AUDIO_OUTPUT_DEVICES = "AUDIO_OUTPUT_DEVICES";

        private string ApplicationSettingsDirectory => Path.Combine(_localAppDataDirectory, "SoundboardYourFriends");

        private string SettingsFilePath => Path.Combine(ApplicationSettingsDirectory, "settings.json");
        #endregion Member Variables..

        #region Properties..
        #region AudioCaptureDevices
        private IEnumerable<AudioCaptureDevice> _audioCaptureDevices;
        public IEnumerable<AudioCaptureDevice> AudioCaptureDevices
        {
            get { return _audioCaptureDevices; }
            set { _audioCaptureDevices = value; }
        }
        #endregion AudioCaptureDevices

        #region AudioOutputDevices
        private IEnumerable<AudioOutputDevice> _audioOutputDevices;
        public IEnumerable<AudioOutputDevice> AudioOutputDevices
        {
            get { return _audioOutputDevices; }
            set { _audioOutputDevices = value; }
        }
        #endregion AudioOutputDevices

        #region Instance
        private static ApplicationConfiguration _Instance;
        [JsonIgnore]
        public static ApplicationConfiguration Instance
        {
            get 
            {
                _Instance = _Instance ?? new ApplicationConfiguration();
                return _Instance; 
            }
            private set { _Instance = value; }
        }
        #endregion Instance

        #region RecordHotkey
        private Key _recordHotkey = Key.None;
        public Key RecordHotkey
        {
            get { return _recordHotkey; }
            set { _recordHotkey = value; }
        }
        #endregion RecordHotkey

        #region RecordKeyModifer
        private KeyModifier _recordKeyModifier = KeyModifier.None;
        public KeyModifier RecordKeyModifer
        {
            get { return _recordKeyModifier; }
            set { _recordKeyModifier = value; }
        }
        #endregion RecordKeyModifer

        #region SampleKeyModifier
        private KeyModifier _sampleKeyModifier = KeyModifier.None;
        public KeyModifier SampleKeyModifier
        {
            get { return _sampleKeyModifier; }
            set { _sampleKeyModifier = value; }
        }
        #endregion SampleKeyModifier

        #region SoundboardSampleDirectory
        private string _soundboardSampleDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"SoundboardYourFriendsAudioSamples");
        public string SoundboardSampleDirectory
        {
            get { return _soundboardSampleDirectory; }
            set { _soundboardSampleDirectory = value; }
        }
        #endregion SoundboardSampleDirectory

        #region SoundboardSampleGroupExpansionStates
        private Dictionary<string, bool> _soundboardSampleGroupExpansionStates;
        public Dictionary<string, bool> SoundboardSampleGroupExpansionStates
        {
            get { return _soundboardSampleGroupExpansionStates; }
            set
            {
                _soundboardSampleGroupExpansionStates = value;
            }
        }
        #endregion SoundboardSampleGroupExpansions

        #region SoundboardSampleSeconds
        private int _soundboardSampleSeconds = 20;
        public int SoundboardSampleSeconds
        {
            get { return _soundboardSampleSeconds; }
            set { _soundboardSampleSeconds = value; }
        }
        #endregion SoundboardSampleSeconds
        #endregion Properties..

        #region Constructors..
        #region ApplicationConfiguration
        public ApplicationConfiguration()
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
        public void ImportUserSettings()
        {
            try
            {
                if (File.Exists(SettingsFilePath))
                {
                    var jsonSerializerSettings = new JsonSerializerSettings();
                    jsonSerializerSettings.Converters.Add(new AudioOutputDeviceJsonConverter());
                    jsonSerializerSettings.Converters.Add(new AudioCaptureDeviceJsonConverter());

                    var applicationConfigurationFromFile = JsonConvert.DeserializeObject<ApplicationConfiguration>(File.ReadAllText(SettingsFilePath), jsonSerializerSettings);
                    ApplicationConfiguration.Instance = applicationConfigurationFromFile;
                }
            } 
            catch (Exception ex)
            {
                ApplicationLogger.Log(ex.Message, ex.StackTrace);
            }
        }
        #endregion ImportUserSettings

        #region SaveUserSettings
        public void SaveUserSettings()
        {
            try
            {
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.Converters.Add(new AudioOutputDeviceJsonConverter());
                jsonSerializerSettings.Converters.Add(new AudioCaptureDeviceJsonConverter());

                string settingsJson = JsonConvert.SerializeObject(ApplicationConfiguration.Instance, Formatting.Indented, jsonSerializerSettings);

                if (!Directory.Exists(ApplicationSettingsDirectory))
                {
                    Directory.CreateDirectory(ApplicationSettingsDirectory);
                }

                File.WriteAllText(SettingsFilePath, settingsJson);
            }
            catch (Exception ex)
            {
                ApplicationLogger.Log(ex.Message, ex.StackTrace);
            }
        }
        #endregion SaveUserSettings
        #endregion Methods..
    }
}
