using NAudio.Wave;
using System;
using System.Globalization;
using System.Windows.Data;

namespace SoundboardYourFriends.Core.Converters
{
    public class InverseBooleanConverter : IValueConverter
    {
        #region Methods..
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion Methods..
    }
}
