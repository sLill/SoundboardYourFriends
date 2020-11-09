using System;
using System.Globalization;
using System.Windows.Data;

namespace SoundboardYourFriends.Core.Converters
{
    public class VolumeToOpacityConverter : IValueConverter
    {
        #region Methods..
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value > 0 ? 1.0 : 0.85;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion Methods..
    }
}
