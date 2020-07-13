using SoundboardYourFriends.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoundboardYourFriends.Model
{
    public class SoundboardRecording : ObservableObject
    {
        #region Member Variables..
        private string _name;
        #endregion Member Variables..

        #region Properties..
        #region Name
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
        #endregion Properties..

        #region Constructors..
        #region SoundboardRecording
        public SoundboardRecording() { }
        #endregion SoundboardRecording
        #endregion Constructors..

        #region Methods..
        #endregion Methods..
    }
}
