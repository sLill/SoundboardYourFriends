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
     
        #region GetChildrenOfType
        public static List<T> GetChildrenOfType<T>(this DependencyObject dependencyObject) where T : DependencyObject
        {
            List<T> children = new List<T>();

            if (dependencyObject != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
                {
                    var child = VisualTreeHelper.GetChild(dependencyObject, i);

                    if (child is T)
                    {
                        children.Add((T)child);
                    }
                        
                    var grandChildren = child.GetChildrenOfType<T>();
                    children.AddRange(grandChildren);
                }
            }

            return children;
        }
        #endregion GetChildrenOfType
        #endregion Methods..
    }
}
