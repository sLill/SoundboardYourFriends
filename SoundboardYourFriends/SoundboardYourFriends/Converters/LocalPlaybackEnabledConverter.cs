using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SoundboardYourFriends.Converters
{
    public class LocalPlaybackEnabledConverter : IValueConverter
    {
        #region Methods..
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (PlaybackScope)value == PlaybackScope.Local;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? PlaybackScope.Local : PlaybackScope.Global;
        }
        #endregion Methods..
    }
}
