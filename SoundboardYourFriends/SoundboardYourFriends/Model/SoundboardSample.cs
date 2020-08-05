using SoundboardYourFriends.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoundboardYourFriends.Model
{
    public class SoundboardSample : ObservableObject
    {
        #region Member Variables..
        #endregion Member Variables..

        #region Properties..
        #region FilePath
        public string FilePath { get; set; }
        #endregion FilePath

        #region FileTimeMax
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

        #region PlaybackCursor
        private double _playbackCursor = 0;
        public double PlaybackCursor
        {
            get { return _playbackCursor; }
            set
            {
                _playbackCursor = value;
                RaisePropertyChanged();
            }
        }
        #endregion PlaybackCursorValue
        #endregion Properties..

        #region Constructors..
        #region SoundboardSample
        public SoundboardSample() { }
        #endregion SoundboardSample
        #endregion Constructors..

        #region Methods..
        #endregion Methods..
    }
}
