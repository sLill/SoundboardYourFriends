using SoundboardYourFriends.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoundboardYourFriends.Model
{
    public class SoundboardSample : ObservableObject
    {
        #region Member Variables..
        private string _name;
        #endregion Member Variables..

        #region Properties..
        #region FilePath
        public string FilePath { get; set; }
        #endregion FilePath

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
        #region SoundboardSample
        public SoundboardSample() { }
        #endregion SoundboardSample
        #endregion Constructors..

        #region Methods..
        #endregion Methods..
    }
}
