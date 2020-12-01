using System;
using System.Collections.Generic;
using System.Text;

namespace SoundboardYourFriends.Core.Extensions
{
    public static class StringExtensions
    {
        #region Methods..
        public static string ToCamelCase(this string text)
        {
            return string.IsNullOrWhiteSpace(text) ? string.Empty : $"{text[0].ToString().ToLowerInvariant()}{text.Substring(1)}";
        }
        #endregion Methods..
    }
}
