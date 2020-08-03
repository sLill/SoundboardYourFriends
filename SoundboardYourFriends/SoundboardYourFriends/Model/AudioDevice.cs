using SoundboardYourFriends.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoundboardYourFriends.Model
{
    public class AudioDevice : ObservableObject
    {
        #region Member Variables..
        #endregion Member Variables..

        #region Properties..
        #region DeviceId
        private int _deviceId;
        public int DeviceId
        {
            get { return _deviceId; }
            set { _deviceId = value; }
        }
        #endregion DeviceId

        #region FriendlyName
        private string _friendlyName;
        public string FriendlyName
        {
            get { return _friendlyName; } 
            set 
            { 
                _friendlyName = value;
                RaisePropertyChanged();
            }
        }
        #endregion FriendlyName

        #region NameGuid
        private Guid _nameGuid;
        public Guid NameGuid
        {
            get { return _nameGuid; }
            set
            {
                _nameGuid = value;
                RaisePropertyChanged();
            }
        }
        #endregion NameGuid
        #endregion Properties..
    }
}
