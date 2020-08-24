using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace SoundboardYourFriends
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private bool IsDebug = Debugger.IsAttached;

        public App()
        {
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (!e.Args.ToList().Contains("-u") && !IsDebug)
            {
                // Close this application and run the updater first
                Process.Start("updater.exe");
                Environment.Exit(0);
            }
        }
    }
}
