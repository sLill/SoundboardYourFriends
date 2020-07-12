using SoundboardYourFriends.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SoundboardYourFriends.View
{
    public partial class AudioDeviceDialog : Window, IDisposable
    {
        #region Member Variables..
        private AudioDeviceDialogViewModel _audioDeviceDialogViewModel;
        #endregion Member Variables..

        #region Properties..
        #endregion Properties..

        #region Constructors..
        public AudioDeviceDialog(AudioDeviceType audioDeviceType)
        {
            InitializeComponent();

            _audioDeviceDialogViewModel = new AudioDeviceDialogViewModel(audioDeviceType);
            DataContext = _audioDeviceDialogViewModel;
        }
        #endregion Constructors..

        #region Methods..
        #region Events..
        #region Button_PreviewMouseLeftButtonDown
        private void btnOK_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        #endregion Button_PreviewMouseLeftButtonDown
        #endregion Events..

        public void Dispose()
        {
            
        }
        #endregion Methods..
    }
}
