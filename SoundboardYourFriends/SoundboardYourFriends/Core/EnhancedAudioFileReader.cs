using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoundboardYourFriends.Core
{
    public class EnhancedAudioFileReader : AudioFileReader
    {
        #region Member Variables..
        #endregion Member Variables..

        #region Properties..
        #region IsDisposed
        public bool IsDisposed { get; set; }
        #endregion IsDisposed
        #endregion Properties..

        #region Constructors..
        #region EnhancedAudioFileReader
        public EnhancedAudioFileReader(string fileName)
            : base(fileName) { }
        #endregion EnhancedAudioFileReader
        #endregion Constructors..

        #region Methods..
        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    base.Dispose(disposing);
                }

                IsDisposed = true;
            }
        }
        #endregion Dispose
        #endregion Methods..
    }
}
