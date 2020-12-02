using System;
using System.Globalization;
using System.Windows.Data;

namespace SoundboardYourFriends.Core.Converters
{
    public class DoubleToIntegerConverter : IValueConverter
    {
        #region Methods..
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)((double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion Methods..
    }
}
