using System;
using System.Globalization;
using System.Windows.Data;

namespace SoundboardYourFriends.Core.Converters
{
    public class DoubleToTimeStringConverter : IValueConverter
    {
        #region Methods..
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int hours = (int)((double)value) / 60;
            int seconds = (int)((double)value) % 60;

            return $"{hours.ToString("00")}:{seconds.ToString("00")}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion Methods..
    }
}
