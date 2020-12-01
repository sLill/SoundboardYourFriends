using SoundboardYourFriends.Core.Config;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace SoundboardYourFriends.Core.Converters
{
    public class GroupExpansionConverter : IValueConverter
    {
        #region Methods..
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ApplicationConfiguration.Instance.SoundboardSampleGroupExpansionStates[(string)parameter];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ApplicationConfiguration.Instance.SoundboardSampleGroupExpansionStates[(string)parameter] = (bool)value;
            return true;
        }
        #endregion Methods..

    }
}
