using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace SoundboardYourFriends.Core.Extensions
{
    public static class DependencyObjectExtensions
    {
        #region Methods..
        #region FindAnchestor
        public static T FindAnchestor<T>(this DependencyObject dependencyObject) where T : DependencyObject
        {
            do
            {
                if (dependencyObject is T)
                {
                    return (T)dependencyObject;
                }

                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }
            while (dependencyObject != null);

            return null;
        }
        #endregion FindAnchestor
        #endregion Methods..
    }
}
