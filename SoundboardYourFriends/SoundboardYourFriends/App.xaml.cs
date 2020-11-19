﻿using SoundboardYourFriends.Core;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
            ApplicationLogger.Log("Application started");

            //if (!e.Args.ToList().Contains("-u") && !IsDebug)
            //{
            //    // Close this application and run the updater first
            //    Process.Start("update.exe", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            //    Environment.Exit(0);
            //}
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            ApplicationLogger.Log("Application Closing");
        }
    }
}
