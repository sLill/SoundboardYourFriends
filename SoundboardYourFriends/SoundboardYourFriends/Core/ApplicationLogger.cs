using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SoundboardYourFriends.Core
{
    public static class ApplicationLogger
    {
        #region Member Variables..
        private const string _logfileName = "log.txt";
        #endregion Member Variables..

        #region Properties..
        #endregion Properties..

        #region Constructors..
        static ApplicationLogger()
        {
            File.WriteAllText(_logfileName, string.Empty);
        }
        #endregion Constructors..

        #region Methods..
        #region Event Handlers..
        #endregion Event Handlers..
        
        #region Log
        public static void Log(string message)
        {
            File.AppendAllText(_logfileName, $"{Environment.NewLine}> {DateTime.Now} -- {message}");
        }
        #endregion Log

        #region Log
        public static void Log(string message, string stackTrace)
        {
            File.AppendAllText(_logfileName, $"{Environment.NewLine}> {DateTime.Now} -- {message} {Environment.NewLine}{Environment.NewLine}{stackTrace}{Environment.NewLine}");
        }
        #endregion Log
        #endregion Methods..
    }
}
