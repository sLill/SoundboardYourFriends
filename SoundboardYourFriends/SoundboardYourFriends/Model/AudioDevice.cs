using SoundboardYourFriends.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoundboardYourFriends.Model
{
    public class AudioDevice : ObservableObject
    {
        #region Member Variables..
        private string _friendlyName;
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
        #endregion Properties..
    }
}
