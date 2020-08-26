using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace SoundboardYourFriends.Update
{
    public class Program
    {
        #region Member Variables..
        private static object _lockObject = new object();
        private static bool _updatePending = false;
        #endregion Member Variables..

        #region Main
        static void Main(string[] args)
        {
            LogAgent.Open();

            try
            {
                LogAgent.WriteLine("Checking for updates..");
                UpdateAgent.UpdateComplete += OnUpdateComplete;

                string currentVersionParameter = args.FirstOrDefault() ?? "1.0.0.0";
                Version currentVersion = new Version(currentVersionParameter);

                var updateInformation = UpdateAgent.CheckForUpdatesAsync(currentVersion).Result;
                _updatePending = updateInformation != null;

                if (_updatePending)
                {
                    // Close the application if it's already running
                    var process = Process.GetProcessesByName("SoundboardYourFriends");
                    process.Cast<Process>().ToList().ForEach(process => process.Kill());

                    // Not a waiting function
                    UpdateAgent.UpdateApplicationAsync(updateInformation);
                }
                else
                {
                    LogAgent.WriteLine("No updates found.");
                    OnUpdateComplete(null, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                lock(_lockObject)
                {
                    _updatePending = false;
                }

                string message = $"{Environment.NewLine}Update failed: {ex.ToString()}";
                LogAgent.WriteLine(message);
            }
            finally
            {
                // Suppress application startup until the update download completes
                while (_updatePending)
                { }

                // Start the main application
                Process.Start("SoundboardYourFriends.exe", "-u");

                LogAgent.Close();
                Environment.Exit(0);
            }
        }
        #endregion Main

        #region Methods..
        #region Event Handlers..
        #region OnUpdateComplete
        private static void OnUpdateComplete(object sender, EventArgs e)
        {
            UpdateAgent.UpdateComplete -= OnUpdateComplete;

            LogAgent.WriteLine("Update complete.");
            lock(_lockObject)
            {
                _updatePending = false;
            }
        }
        #endregion OnUpdateComplete 
        #endregion Event Handlers..
        #endregion Methods..
    }
}
