using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace SoundboardYourFriends.Update
{
    public static class LoggingAgent
    {
        #region Member Variables..
        private static StreamWriter _logWriter = null;
        #endregion Member Variables..

        #region Constructors..
        #endregion Constructors..

        #region Methods..
        #region Close
        public static void Close()
        {
            Dispose();
        }
        #endregion Close

        #region Dispose
        public static void Dispose()
        {
            _logWriter?.Dispose();
            _logWriter = null;
        }
        #endregion Dispose

        public static void Open()
        {
            try
            {
                _logWriter = new StreamWriter("log.txt", true);
            }
            catch { }
        }

        #region Write
        public static void Write(string message)
        {
            Console.Write(message);
        }
        #endregion Write

        #region WriteLine
        public static void WriteLine(string message)
        {
            Console.WriteLine(message);

            try
            {
                string dateTimeString = $"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}";
                _logWriter.WriteLine($"{dateTimeString} - {message}");
            }
            catch { }
        }
        #endregion WriteLine
        #endregion Methods..
    }
}
