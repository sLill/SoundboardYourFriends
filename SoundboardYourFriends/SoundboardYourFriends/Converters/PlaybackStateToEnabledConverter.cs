using NAudio.Wave;
using System;
using System.Globalization;
using System.Windows.Data;

namespace SoundboardYourFriends.Converters
{
    public class PlaybackStateToEnabledConverter : IValueConverter
    {
        #region Methods..
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((PlaybackState)value) != PlaybackState.Playing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion Methods..
    }
}
