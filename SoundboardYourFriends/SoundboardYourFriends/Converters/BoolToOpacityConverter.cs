﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace SoundboardYourFriends.Converters
{ 
    public class BoolToOpacityConverter : IValueConverter
    {
        #region Methods..
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 0.6 : 1.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion Methods..
    }
}
