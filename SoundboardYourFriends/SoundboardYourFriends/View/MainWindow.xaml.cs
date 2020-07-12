using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SoundboardYourFriends.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnListeningDevice_MouseUp(object sender, MouseButtonEventArgs e)
        {
            using (AudioDeviceDialog audioDeviceDialog = new AudioDeviceDialog(AudioDeviceType.Output))
            {
                audioDeviceDialog.ShowDialog();
            }
        }

        private void btnRecordingDevice_MouseUp(object sender, MouseButtonEventArgs e)
        {
            using (AudioDeviceDialog audioDeviceDialog = new AudioDeviceDialog(AudioDeviceType.Input))
            {
                audioDeviceDialog.ShowDialog();
            }
        }

        private void btnRecordButton_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
