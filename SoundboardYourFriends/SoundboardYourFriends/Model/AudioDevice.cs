﻿using SoundboardYourFriends.Core;
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

        #region Constructors..
        #endregion Constructors..

        #region Methods..
        #endregion Methods..    
    }
}