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
    public class ApplicationConfiguration : ObservableObject
    {
        #region Member Variables..
        private static string _localAppDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        private const string RECORD_HOTKEY = "RECORD_HOTKEY";
        private const string RECORD_HOTKEY_MODIFIER = "RECORD_HOTKEY_MODIFIER";   
        private const string SAMPLE_DIRECTORY = "SAMPLE_DIRECTORY";
        private const string SAMPLE_HOTKEY_MODIFIER = "SAMPLE_HOTKEY_MODIFIER";
        private const string SAMPLE_LENGTH = "SAMPLE_LENGTH";
        private const string AUDIO_CAPTURE_DEVICES = "AUDIO_CAPTURE_DEVICES";
        private const string AUDIO_OUTPUT_DEVICES = "AUDIO_OUTPUT_DEVICES";

        private static string ApplicationSettingsDirectory => Path.Combine(_localAppDataDirectory, "SoundboardYourFriends");

        private static string SettingsFilePath => Path.Combine(ApplicationSettingsDirectory, "settings.json");
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
        private static ApplicationConfiguration _instance;
        [JsonIgnore]
        public static ApplicationConfiguration Instance
        {
            get 
            {
                _instance = _instance ?? LoadApplicationConfiguration();
                return _instance; 
            }
            private set { _instance = value; }
        }
        #endregion Instance

        #region MainWindowHeight
        private int _mainWindowHeight = 550;
        public int MainWindowHeight
        {
            get { return _mainWindowHeight; }
            set 
            { 
                _mainWindowHeight = value;
                RaisePropertyChanged();
            }
        }
        #endregion MainWindowHeight

        #region MainWindowWidth
        private int _mainWindowWidth = 1050;
        public int MainWindowWidth
        {
            get { return _mainWindowWidth; }
            set 
            {
                _mainWindowWidth = value;
                RaisePropertyChanged();
            }
        }
        #endregion MainWindowWidth

        #region RecordHotkey
        private Key _recordHotkey = Key.Up;
        public Key RecordHotkey
        {
            get { return _recordHotkey; }
            set { _recordHotkey = value; }
        }
        #endregion RecordHotkey

        #region RecordKeyModifer
        private KeyModifier _recordKeyModifier = KeyModifier.Ctrl;
        public KeyModifier RecordKeyModifer
        {
            get { return _recordKeyModifier; }
            set { _recordKeyModifier = value; }
        }
        #endregion RecordKeyModifer

        #region SampleKeyModifier
        private KeyModifier _sampleKeyModifier = KeyModifier.Ctrl;
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
                SaveApplicationConfiguration();
            };
        }
        #endregion ApplicationConfiguration
        #endregion Constructors..

        #region Methods..
        #region LoadApplicationConfiguration
        public static ApplicationConfiguration LoadApplicationConfiguration()
        {
            var applicationConfiguration = new ApplicationConfiguration();

            try
            {
                if (File.Exists(SettingsFilePath))
                {
                    var jsonSerializerSettings = new JsonSerializerSettings();
                    jsonSerializerSettings.Converters.Add(new AudioOutputDeviceJsonConverter());
                    jsonSerializerSettings.Converters.Add(new AudioCaptureDeviceJsonConverter());

                    applicationConfiguration = JsonConvert.DeserializeObject<ApplicationConfiguration>(File.ReadAllText(SettingsFilePath), jsonSerializerSettings);
                }
            } 
            catch (Exception ex)
            {
                ApplicationLogger.Log(ex.Message, ex.StackTrace);
            }

            return applicationConfiguration;
        }
        #endregion LoadApplicationConfiguration

        #region SaveApplicationConfiguration
        public void SaveApplicationConfiguration()
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
        #endregion SaveApplicationConfiguration
        #endregion Methods..
    }
}
