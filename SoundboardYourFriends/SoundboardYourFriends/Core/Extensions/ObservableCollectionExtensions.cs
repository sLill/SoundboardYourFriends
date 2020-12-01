using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SoundboardYourFriends.Core.Extensions
{
    public static class ObservableCollectionExtensions
    {
        #region Methods..
        #region Swap
        public static ObservableCollection<T> Swap<T>(this ObservableCollection<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];

            list[indexA] = list[indexB];
            list[indexB] = tmp;

            return list;
        } 
        #endregion Swap
        #endregion Methods..
    }
}
