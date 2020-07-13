using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SoundboardYourFriends.Core
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        #region Events..
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion Events..

        #region Methods..
        #region RaisePropertyChanged
        protected void RaisePropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion RaisePropertyChanged 
        #endregion Methods..
    }
}
