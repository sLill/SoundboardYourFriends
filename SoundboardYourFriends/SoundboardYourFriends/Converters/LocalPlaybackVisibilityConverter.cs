using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SoundboardYourFriends.Converters
{
    public class LocalPlaybackVisibilityConverter : IValueConverter
    {
        #region Methods..
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (PlaybackScope)value >= PlaybackScope.Local ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion Methods..
    }
}
