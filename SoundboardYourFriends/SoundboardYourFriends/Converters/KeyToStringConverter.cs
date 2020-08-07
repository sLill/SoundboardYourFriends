using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace SoundboardYourFriends.Converters
{
    public class KeyToStringConverter : IValueConverter
    {
        #region Methods..
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Key)value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion Methods..
    }
}
