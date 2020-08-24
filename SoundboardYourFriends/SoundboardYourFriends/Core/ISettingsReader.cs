using System;
using System.Collections.Generic;
using System.Text;

namespace SoundboardYourFriends.Core
{
    public interface ISettingsReader
    {
        #region Methods..
        T Load<T>() where T : class, new();

        T LoadSection<T>() where T : class, new();

        object Load(Type type);

        object LoadSection(Type type);
        #endregion Methods..
    }
}
