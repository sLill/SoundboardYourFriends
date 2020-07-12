using System;
using System.Collections.Generic;
using System.Text;
using System.Management;

namespace SoundboardYourFriends.ViewModel
{
    public class AudioDeviceDialogViewModel
    {
        #region Member Variables..
        #endregion Member Variables..

        #region Properties..
        #region AudioDeviceType
        public AudioDeviceType AudioDeviceType { get; private set; }
        #endregion AudioDeviceType
        #endregion Properties..

        #region Constructors..

        #region AudioDeviceDialogViewModel
        public AudioDeviceDialogViewModel(AudioDeviceType audioDeviceType) 
        {
            AudioDeviceType = audioDeviceType;

            GetWindowsAudioDevices();
        }
        #endregion AudioDeviceDialogViewModel
        #endregion Constructors..

        #region Methods..
        #region GetWindowsAudioDevices
        private void GetWindowsAudioDevices()
        {
            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_SoundDevice");
            ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();

            foreach (var audioDevice in managementObjectCollection)
            {
               foreach(PropertyData audioDevicePropertyData in audioDevice.Properties)
                {

                }
            }
        }
        #endregion GetWindowsAudioDevices
        #endregion Methods..
    }
}
