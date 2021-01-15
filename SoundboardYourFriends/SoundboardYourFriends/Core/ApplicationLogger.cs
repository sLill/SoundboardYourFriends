using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SoundboardYourFriends.Core
{
    public static class ApplicationLogger
    {
        #region Member Variables..
        private static string _applicationDataDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SoundboardYourFriends");

        private static string _logFileName => Path.Combine(_applicationDataDirectory, "Log.txt");
        #endregion Member Variables..

        #region Properties..
        #endregion Properties..

        #region Constructors..
        static ApplicationLogger()
        {
            if (!Directory.Exists(_applicationDataDirectory))
            {
                Directory.CreateDirectory(_applicationDataDirectory);
            }

            File.WriteAllText(_logFileName, string.Empty);
        }
        #endregion Constructors..

        #region Methods..
        #region Event Handlers..
        #endregion Event Handlers..
        
        #region Log
        public static void Log(string message)
        {
            File.AppendAllText(_logFileName, $"{Environment.NewLine}> {DateTime.Now} -- {message}");
        }
        #endregion Log

        #region Log
        public static void Log(string message, string stackTrace)
        {
            File.AppendAllText(_logFileName, $"{Environment.NewLine}> {DateTime.Now} -- {message} {Environment.NewLine}{Environment.NewLine}{stackTrace}{Environment.NewLine}");
        }
        #endregion Log
        #endregion Methods..
    }
}
