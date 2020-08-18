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
            return (PlaybackType)value == PlaybackType.Local;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? PlaybackType.Local : PlaybackType.Global;
        }
        #endregion Methods..
    }
}
