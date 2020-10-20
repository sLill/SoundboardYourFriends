using DSOFile;
using NAudio.Wave;
using SoundboardYourFriends.Core;
using System;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows.Input;

namespace SoundboardYourFriends.Model
{
    public class SoundboardSample : ObservableObject
    {
        #region Member Variables..
        private int _playbackTimerInterval = 200;
        private Timer _playbackTimer = null;
        #endregion Member Variables..

        #region Properties..
        #region FileLocked
        private bool _fileLocked;
        public bool FileLocked
        {
            get { return _fileLocked; }
            set
            {
                _fileLocked = value;
                RaisePropertyChanged();
            }
        }
        #endregion FileLocked

        #region FilePath
        public string FilePath { get; set; }
        #endregion FilePath

        #region FileTimeMax
        /// <summary>
        /// Seconds
        /// </summary>
        private double _fileTimeMax = 0;
        public double FileTimeMax
        {
            get { return _fileTimeMax; }
            set
            {
                _fileTimeMax = value;
                RaisePropertyChanged();
            }
        }
        #endregion FileTimeMax

        #region FileTimeMin
        /// <summary>
        /// Seconds
        /// </summary>
        private double _fileTimeMin = 0;
        public double FileTimeMin
        {
            get { return _fileTimeMin; }
            set
            {
                _fileTimeMin = value;
                RaisePropertyChanged();
            }
        }
        #endregion FileTimeMin

        #region FileTimeUpperValue
        /// <summary>
        /// Seconds
        /// </summary>
        private double _fileTimeUpperValue = 0;
        public double FileTimeUpperValue
        {
            get { return _fileTimeUpperValue; }
            set
            {
                _fileTimeUpperValue = value;
                RaisePropertyChanged();
            }
        }
        #endregion FileTimeUpperValue

        #region FileTimeLowerValue
        /// <summary>
        /// Seconds
        /// </summary>
        private double _fileTimeLowerValue = 0;
        public double FileTimeLowerValue
        {
            get { return _fileTimeLowerValue; }
            set
            {
                _fileTimeLowerValue = value;
                RaisePropertyChanged();
            }
        }
        #endregion FileTimeLowerValue

        #region FileUniqueId
        private Guid _fileUniqueId;
        public Guid FileUniqueId
        {
            get { return _fileUniqueId; }
            set { _fileUniqueId = value; }
        }
        #endregion FileUniqueId

        #region GroupName
        private string _groupName;
        public string GroupName
        {
            get { return _groupName; }
            set
            {
                _groupName = value;
                RaisePropertyChanged();
            }
        }
        #endregion GroupName

        #region Hotkey
        private Key _hotkey;
        public Key Hotkey
        {
            get { return _hotkey; }
            set
            {
                _hotkey = value;
                HotkeyDisplay = _hotkey.ToString();
                RaisePropertyChanged();
            }
        }
        #endregion Hotkey

        #region HotkeyDisplay
        private string _hotkeyDisplay;
        public string HotkeyDisplay
        {
            get { return $"{ApplicationConfiguration.Instance.SampleKeyModifier} + {_hotkeyDisplay}"; }
            set
            {
                _hotkeyDisplay = value;
                RaisePropertyChanged();
            }
        }
        #endregion HotkeyDisplay

        #region HotkeyId
        private int _hotkeyId;
        public int HotkeyId
        {
            get { return _hotkeyId; }
            set
            {
                _hotkeyId = value;
                RaisePropertyChanged();
            }
        }
        #endregion HotkeyId

        #region Name
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }
        #endregion Name

        #region PlaybackCursorValue
        private double _playbackCursorValue = 0;
        public double PlaybackCursorValue
        {
            get { return _playbackCursorValue; }
            set
            {
                _playbackCursorValue = value;
                RaisePropertyChanged();
            }
        }
        #endregion PlaybackCursorValue
        #endregion Properties..

        #region Constructors..
        #region SoundboardSample
        public SoundboardSample(string filePath)
        {
            this.FilePath = filePath;
            this.Name = Path.GetFileNameWithoutExtension(filePath);

            _playbackTimer = new Timer(_playbackTimerInterval);
            _playbackTimer.Elapsed += OnPlaybackTimerElapsed;

            InitializePropertiesFromMetaData();
        }
        #endregion SoundboardSample
        #endregion Constructors..

        #region Methods..
        #region Event Handlers..
        #region OnPlaybackTimerElapsed
        private void OnPlaybackTimerElapsed(object sender, EventArgs e)
        {
            PlaybackCursorValue = PlaybackCursorValue + (_playbackTimerInterval / 1000.0);
        }
        #endregion OnPlaybackTimerElapsed
        #endregion Event Handlers..

        #region InitializePropertiesFromMetaData
        private void InitializePropertiesFromMetaData()
        {
            // Pull custom metadata properties from the file. Generate them if this is a new file and none are found
            OleDocumentProperties documentProperties = new OleDocumentProperties();
            documentProperties.Open(this.FilePath, false, dsoFileOpenOptions.dsoOptionDefault);

            var FileCustomPropertyCollection = documentProperties.CustomProperties.Cast<CustomProperty>()
                .Where(property => property.Name.Contains("SoundboardSample")).ToDictionary(x => x.Name, x => x);

            if (!FileCustomPropertyCollection.Any())
            {
                FileUniqueId = Guid.NewGuid();
                documentProperties.CustomProperties.Add("SoundboardSample_FileUniqueIdentifier", FileUniqueId.ToString());

                Hotkey = Key.None;
                documentProperties.CustomProperties.Add("SoundboardSample_Hotkey", Hotkey.ToString());

                documentProperties.Save();
            }
            else
            {
                FileUniqueId = Guid.Parse((string)FileCustomPropertyCollection["SoundboardSample_FileUniqueIdentifier"].get_Value());
                Enum.TryParse(typeof(Key), (string)FileCustomPropertyCollection["SoundboardSample_Hotkey"].get_Value(), out object hotkey);

                hotkey = hotkey ?? Key.None;
                Hotkey = (Key)hotkey;
            }

            documentProperties.Close();
        }
        #endregion InitializePropertiesFromMetaData

        #region SaveMetadataProperties
        public void SaveMetadataProperties()
        {
            OleDocumentProperties documentProperties = new OleDocumentProperties();
            documentProperties.Open(this.FilePath, false, dsoFileOpenOptions.dsoOptionDefault);

            var FileCustomPropertyCollection = documentProperties.CustomProperties.Cast<CustomProperty>()
                .Where(property => property.Name.Contains("SoundboardSample")).ToDictionary(x => x.Name, x => x);

            if (FileCustomPropertyCollection.ContainsKey("SoundboardSample_Hotkey"))
            {
                FileCustomPropertyCollection["SoundboardSample_Hotkey"].set_Value(Hotkey.ToString());
            }

            documentProperties.Save();
            documentProperties.Close();
        }
        #endregion SaveMetadataProperties

        #region StartPlaybackTimer
        public void StartPlaybackTimer()
        {
            StopPlaybackTimer();
            _playbackTimer.Start();

            FileLocked = true;
        }
        #endregion StartPlaybackTimer

        #region StopPlaybackTimer
        public void StopPlaybackTimer()
        {
            _playbackTimer.Stop();
            PlaybackCursorValue = FileTimeLowerValue;

            FileLocked = false;
        }
        #endregion StopPlaybackTimer
        #endregion Methods..
    }
}
