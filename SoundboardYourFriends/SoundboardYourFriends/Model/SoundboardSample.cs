﻿using SoundboardYourFriends.Core;
using System;
using System.Collections.Generic;
using System.Text;
using DSOFile;
using System.Reflection.Metadata;
using System.Linq;
using System.IO;

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

            InitializeFileMetaData();
        }
        #endregion SoundboardSample
        #endregion Constructors..

        #region Methods..
        #region InitializeFileMetaData
        private void InitializeFileMetaData()
        {
            string FileUniqueIdentifierPropertyKey = "SoundboardSample_FileUniqueIdentifier";

            // Pull custom metadata properties from the file. Generate them if this is a new file and none are found
            OleDocumentProperties documentProperties = new OleDocumentProperties();
            documentProperties.Open(this.FilePath, false, dsoFileOpenOptions.dsoOptionDefault);

            var FileCustomProperties = documentProperties.CustomProperties.Cast<CustomProperty>()
                .Where(property => property.Name.Contains("SoundboardSample")).ToDictionary(x => x.Name, x => x.get_Value());

            if (!FileCustomProperties.Any())
            {
                FileUniqueId = Guid.NewGuid();
                documentProperties.CustomProperties.Add(FileUniqueIdentifierPropertyKey, FileUniqueId.ToString());

                documentProperties.Save();
            }
            else
            {
                FileUniqueId = Guid.Parse((string)FileCustomProperties[FileUniqueIdentifierPropertyKey]);
            }
        }
        #endregion InitializeFileMetaData
        #endregion Methods..
    }
}
