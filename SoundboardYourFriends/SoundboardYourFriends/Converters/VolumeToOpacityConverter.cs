using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace SoundboardYourFriends.Converters
{
    public class VolumeToOpacityConverter : IValueConverter
    {
        #region Methods..
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value > 0 ? 1.0 : 0.6;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion Methods..
    }
}
